using Animancer;
using Cysharp.Threading.Tasks;
using SquidGame.LandScape.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace SquidGame.LandScape.Survival
{
    public class Bot : Unit, IInfomation
    {
        [Space(8)]
        [Header("--- REFERENCES ---")]
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Collider _collider;
        [SerializeField] private CombatRootNode _combatNode;
        [SerializeField] private FindWeaponRootNode _findWeaponNode;
        // public ParticleSystem ParticleDust;

        [Space(8)]
        [Header("--- CONFIG ---")]
        [SerializeField] private float _randomWalkAroundTime = 3f;
        [SerializeField] private float _searchRadius = 10f;
        [SerializeField] private float _timeToReloadOpponent = 2f;
        private float _attackRadius;

        private BotController _botController;
        private Minigame3.Timer _walkAroundTimer = new Minigame3.Timer();
        private Coroutine _behaviorCoroutine;

        public float AttackRadius => _attackRadius;
        public float TimeToReloadOpponent => _timeToReloadOpponent;
        public BotController BotController => _botController;
        public NavMeshAgent Agent => _agent;
        public BotIdleState IdleState;
        public BotMoveState MoveState;
        public BotAttackState AttackState;
        public BotDamagedState BotDamagedState;
        public BotDeadState BotDeadState;

        public Transform GetTransform() => transform;

        public override void Awake()
        {
            base.Awake();
            PlayerController.OnPlayerDead += OnPlayerDead;
            PlayerController.OnPlayerRevive += OnPlayerRevive;
        }

        private void OnDestroy()
        {
            PlayerController.OnPlayerDead -= OnPlayerDead;
            PlayerController.OnPlayerRevive -= OnPlayerRevive;
        }

        public bool IsPlayer()
        {
            return false;
        }

        private void OnPlayerDead()
        {
            if (IsDead()) return;
            if (_behaviorCoroutine != null)
            {
                StopCoroutine(_behaviorCoroutine);
                _behaviorCoroutine = null;
            }
            ChangeState(IdleState);
            _healthBar?.gameObject.SetActive(false);
            _agent.isStopped = true;
        }

        private void OnPlayerRevive(bool weaponEquipped)
        {
            if (IsDead()) return;
            _agent.isStopped = false;
            _healthBar?.gameObject.SetActive(true);
            _behaviorCoroutine = StartCoroutine(CRHandleCombat());
        }

        public void Init(BotController botController)
        {
            _botController = botController;
            IdleState = new(this);
            MoveState = new(this);
            AttackState = new(this);
            BotDamagedState = new(this);
            BotDeadState = new(this);

            _stateMachine = new();
            _stateMachine.Initialize(IdleState);

            _findWeaponNode.SetBot(this);
            _findWeaponNode.Init(null);
            _combatNode.SetBot(this);
            _combatNode.Init(null);

            _behaviorCoroutine = StartCoroutine(CRWalkAround());
        }

        public override void SetupCombatHandler()
        {
            _combatHandler.tag = "Bot";
            _combatHandler.SetIsPlayer(false);
            _combatHandler.SetCanPlaySound(false);
        }

        #region BEHAVIOR

        //step 1
        public IEnumerator CRWalkAround()
        {
            _walkAroundTimer.SetCooldownTime(_randomWalkAroundTime);
            ChangeState(MoveState);
            while (!_walkAroundTimer.CheckTimer())
            {
                yield return CRWalkAround();
            }
            ChangeState(IdleState);
            _behaviorCoroutine = StartCoroutine(CRScreachingForWeapon());

            IEnumerator CRWalkAround()
            {
                BotDamagedState.SetOnDone(async () =>
                {
                    _agent.isStopped = true;
                    await UniTask.DelayFrame(1);
                    ChangeState(MoveState);
                    _agent.isStopped = false;
                });
                NavMeshHit hit = PickRandomPosition();
                _agent.SetDestination(hit.position);
                yield return new WaitUntil(() =>
                {
                    if (_walkAroundTimer.CheckTimer()) return true;
                    return _agent.remainingDistance < 0.5f;
                });
                BotDamagedState.SetOnDone(null);
            }

            NavMeshHit PickRandomPosition()
            {
                Vector3 randomDirection = Random.insideUnitSphere * _searchRadius;
                randomDirection += transform.position;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, _searchRadius, NavMesh.AllAreas))
                {
                    return hit;
                }
                else
                {
                    return PickRandomPosition();
                }

            }

        }

        //step 2
        private IEnumerator CRScreachingForWeapon()
        {
            ChangeState(MoveState);
            while (_currentEquipedWeapon == null)
            {
                _findWeaponNode.UpdateNode();
                yield return null;
            }
            _attackRadius = _currentEquipedWeapon.Stats.Range;
            _behaviorCoroutine = StartCoroutine(CRHandleCombat());

        }

        // step 3
        private IEnumerator CRHandleCombat()
        {
            while (GetListUserInfor().Count > 0 && !IsDead())
            {
                _combatNode.UpdateNode();
                yield return null;
            }

            List<IInfomation> GetListUserInfor()
            {
                List<IInfomation> userList = _botController.SurvivalManager.Userlist;
                userList.Remove(this);
                return userList;
            }
        }

        #endregion

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Bot"))
            {
                if (other.TryGetComponent(out CombatHandler combatHandler))
                {
                    if (_combatHandler == combatHandler) return;
                    Damaged(combatHandler.GetIncomingWeaponDamage());
                    combatHandler.PlayHitEffect();
                }
            }
            if (other.CompareTag("Player"))
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_WEAPON_DAMAGE);
            }
        }

        public override void Die()
        {
            base.Die();
            if (_behaviorCoroutine != null)
            {
                StopCoroutine(_behaviorCoroutine);
                _behaviorCoroutine = null;
            }
            _collider.enabled = false;
            _botController.OnBotDie(this);
            _agent.isStopped = true;
        }

        public override bool IsDead()
        {
            return IsInState(BotDeadState);
        }

        public override void Damaged(int damageValue)
        {
            base.Damaged(damageValue);
            if (_currentHealth <= 0)
            {
                ChangeState(BotDeadState);
            }
            else
                ChangeState(BotDamagedState);
        }
    }

    public abstract class BotState : State
    {
        public Bot Bot;
        protected BotState(Bot bot)
        {
            Bot = bot;
        }
    }

    public class BotIdleState : BotState
    {
        public BotIdleState(Bot bot) : base(bot) { }

        public override void Enter()
        {
            //Bot.ClearVelocity();
            Bot.PlayAnimation(ANIMATION.Idle);
        }

        public override void LogicUpdate()
        {
            //if (Character.GetJoystickDirection().magnitude >= 0.1f)
            //    Character.ChangeState(Character.MoveState);
        }
    }

    public class BotMoveState : BotState
    {
        public BotMoveState(Bot bot) : base(bot) { }

        public override void Enter()
        {
            // Bot.ParticleDust.Play();
            Bot.CancelHitbox();
            Bot.PlayAnimation(ANIMATION.Move);
            // GameSound.I.PlaySFX(Define.SoundPath.SFX_FOOT_STEP, true);
        }

        public override void Exit()
        {
            // GameSound.I.PlaySFX(Define.SoundPath.SFX_FOOT_STEP, false);
        }

        public override void LogicUpdate()
        {
            //if (Character.GetJoystickDirection().magnitude < 0.1f)
            //    Character.ChangeState(Character.IdleState);
        }
    }

    public class BotAttackState : BotState
    {
        private Action _onCompleteAttack;
        AnimancerState attackAnimation;

        public BotAttackState(Bot bot) : base(bot) { }

        override public void Enter()
        {
            //Character.ClearVelocity();
            Bot.ActiveHitbox(Bot.CurrentWeapon.Stats.DelayTime, Bot.CurrentWeapon.Stats.Duration);
            attackAnimation = Bot.PlayAnimation(ANIMATION.Attack, FadeMode.FixedDuration, 0f);
            attackAnimation.Events.OnEnd = () =>
            {
                _onCompleteAttack?.Invoke();
                Bot.ChangeState(Bot.IdleState);
            };
        }

        public void SetOnCompleteAttack(Action onCompleteAttack)
        {
            _onCompleteAttack = onCompleteAttack;
        }



        public override void LogicUpdate()
        {
            //if (Character.GetJoystickDirection().magnitude >= 0.1f)
            //    Character.ChangeState(Character.MoveState);
        }
    }

    public class BotDamagedState : BotState
    {
        private Action _onDone;
        AnimancerState damagedAnimation;

        public BotDamagedState(Bot bot) : base(bot) { }

        public override void Enter()
        {
            //Character.ParticleDust.Play();
            Bot.CancelHitbox();
            Bot.CombatHandler?.CancleSlashEffect();
            damagedAnimation = Bot.PlayAnimation(ANIMATION.Damaged, FadeMode.FromStart, 0f);
            damagedAnimation?.Events.Add(0.6f, () =>
            {
                _onDone?.Invoke();
                damagedAnimation.Events.Clear();
                Bot.ChangeState(Bot.IdleState);
            });
            // GameSound.I.PlaySFX(Define.SoundPath.SFX_FOOT_STEP, true);
        }

        public void SetOnDone(Action onDone)
        {
            _onDone = onDone;
        }

    }

    public class BotDeadState : BotState
    {
        public BotDeadState(Bot bot) : base(bot) { }

        public override void Enter()
        {
            Bot.Die();
            Bot.PlayDeadAnimation();
        }
    }
}
