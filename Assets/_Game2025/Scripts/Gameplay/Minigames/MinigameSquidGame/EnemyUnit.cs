using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Survival;
using UnityEngine;
using UnityEngine.AI;

namespace SquidGame.LandScape
{
    public class EnemyUnit : Unit
    {
        public static Action OnEnemyDead;

        [Space(8)]
        [Header("--- REFERENCES ---")]
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Collider _collider;

        [Space(8)]
        [Header("--- CONFIG ---")]
        [SerializeField] private float _attackRange;
        [SerializeField] private float _distanceToReact;

        public EnemyIdleState EnemyIdleState;
        public EnemyMoveState EnemyMoveState;
        public EnemyAttackState EnemyAttackState;
        public EnemyDamagedState EnemyDamagedState;
        public EnemyDeadState EnemyDeadState;
        public EnemyDanceState EnemyDanceState;

        public bool HasTarget() => _targetTransform != null;

        private void Update()
        {
            _stateMachine.CurrentState?.LogicUpdate();
        }

        private void FixedUpdate()
        {
            _stateMachine.CurrentState?.PhysicUpdate();
        }

        Transform _playerTransform;

        public void OnPlayerDead()
        {
            _playerTransform = _targetTransform;
            _targetTransform = null;
            Dance();
        }

        public void OnPlayerRevive()
        {
            _targetTransform = _playerTransform;
            ChangeState(EnemyIdleState);
            _currentEquipedWeapon?.gameObject.SetActive(true);
        }

        void Start()
        {
            EquipRandomWeapon();
        }

        protected override void InitState()
        {
            EnemyIdleState = new(this);
            EnemyMoveState = new(this);
            EnemyAttackState = new(this);
            EnemyDamagedState = new(this);
            EnemyDeadState = new(this);
            EnemyDanceState = new(this);

            _stateMachine = new();
            _stateMachine.Initialize(null);
            PlayAnimation(ANIMATION.Idle);
        }

        public void SetAgentDestination()
        {
            _agent.isStopped = false;
            _agent.SetDestination(_targetTransform.position);
        }

        public bool CheckIfReactTarget()
        {
            _distanceToReact = Vector3.Distance(transform.position, _targetTransform.position);
            return Vector3.Distance(transform.position, _targetTransform.position) <= _attackRange;
        }

        public void ClearVelocity()
        {
            _agent.velocity = Vector3.zero;
            _agent.isStopped = true;
        }

        public void SetActiveHealthBar(bool value)
        {
            _healthBar.gameObject.SetActive(value);
        }

        public void FacingTarget()
        {
            if (!HasTarget()) return;

            Vector3 direction = (_targetTransform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = lookRotation;
        }

        public void StartCombat(Transform target)
        {
            _targetTransform = target;
            _stateMachine.ChangState(EnemyIdleState);
        }

        public override void SetupCombatHandler()
        {
            _combatHandler.tag = "Bot";
            _combatHandler.SetIsPlayer(false);
            _combatHandler.SetCanPlaySound(true);
        }

        void EquipRandomWeapon()
        {
            EquipWeapon(WeaponController.I.GetRandomWeapon());
            _attackRange = _currentEquipedWeapon.Stats.Range;
        }

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

        public override void Damaged(int damageValue)
        {
            base.Damaged(damageValue);
            ChangeState(_currentHealth <= 0 ? EnemyDeadState : EnemyDamagedState);
        }

        public override void Die()
        {
            base.Die();
            ClearVelocity();
            PlayDeadAnimation();
            _collider.enabled = false;
            OnEnemyDead?.Invoke();
        }

        public void Dance()
        {
            ChangeState(EnemyDanceState);
            ClearVelocity();
            PlayDanceAnimation();
            SetActiveHealthBar(false);
            _currentEquipedWeapon?.gameObject.SetActive(false);
        }
    }

    //--- STATE ------------------------------------
    public abstract class EnemyState : State
    {
        public EnemyUnit Enemy;
        protected EnemyState(EnemyUnit enemy)
        {
            Enemy = enemy;
        }
    }

    public class EnemyIdleState : EnemyState
    {
        public EnemyIdleState(EnemyUnit enemy) : base(enemy) { }

        public override void Enter()
        {
            Enemy.PlayAnimation(ANIMATION.Idle);
            if (Enemy.HasTarget())
            {
                Enemy.ChangeState(Enemy.CheckIfReactTarget() ? Enemy.EnemyAttackState : Enemy.ChangeState(Enemy.EnemyMoveState));
            }

        }
    }

    public class EnemyMoveState : EnemyState
    {
        public EnemyMoveState(EnemyUnit enemy) : base(enemy) { }

        public override void Enter()
        {
            Enemy.SetAgentDestination();
            Enemy.CancelHitbox();
            Enemy.PlayAnimation(ANIMATION.Move);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (Enemy.HasTarget())
            {
                Enemy.SetAgentDestination();
                if (Enemy.CheckIfReactTarget())
                    Enemy.ChangeState(Enemy.EnemyAttackState);
            }
            else
            {
                Enemy.ChangeState(Enemy.EnemyIdleState);
            }
        }
    }

    public class EnemyAttackState : EnemyState
    {
        public EnemyAttackState(EnemyUnit enemy) : base(enemy) { }

        AnimancerState attackAnimation;

        override public void Enter()
        {
            Enemy.ClearVelocity();
            Enemy.ActiveHitbox(Enemy.CurrentWeapon.Stats.DelayTime, Enemy.CurrentWeapon.Stats.Duration);
            attackAnimation = Enemy.PlayAnimation(ANIMATION.Attack, FadeMode.FromStart, 0f);
            attackAnimation.Events.OnEnd = () =>
            {
                attackAnimation.Events.Clear();
                if (Enemy.HasTarget())
                {
                    if (Enemy.CheckIfReactTarget())
                    {
                        Enemy.FacingTarget();
                        Enemy.ChangeState(Enemy.EnemyAttackState);
                    }
                    else
                    {
                        Enemy.ChangeState(Enemy.EnemyMoveState);
                    }
                }
            };
        }
    }

    public class EnemyDamagedState : EnemyState
    {
        public EnemyDamagedState(EnemyUnit enemy) : base(enemy) { }

        AnimancerState damagedAnimation;

        public override void Enter()
        {
            Enemy.ClearVelocity();
            Enemy.CancelHitbox();
            Enemy.CombatHandler?.CancleSlashEffect();
            damagedAnimation = Enemy.PlayAnimation(ANIMATION.Damaged, FadeMode.FromStart, 0f);
            damagedAnimation.Events.OnEnd = () =>
            {
                damagedAnimation.Events.Clear();
                Enemy.ChangeState(Enemy.EnemyIdleState);
            };
        }
    }

    public class EnemyDeadState : EnemyState
    {
        public EnemyDeadState(EnemyUnit enemy) : base(enemy) { }

        public override void Enter()
        {
            Enemy.Die();
        }
    }

    public class EnemyDanceState : EnemyState
    {
        public EnemyDanceState(EnemyUnit enemy) : base(enemy) { }
    }
}
