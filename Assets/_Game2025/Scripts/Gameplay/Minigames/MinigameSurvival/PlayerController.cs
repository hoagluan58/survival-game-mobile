using Animancer;
using DG.Tweening;
using SquidGame.LandScape.Core;
using System;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class PlayerController : Unit, IInfomation
    {
        public static Action OnPlayerDead, OnPlayerHit, OnPlayerEquipWeapon;
        public static Action<bool> OnPlayerRevive;

        [Space(8)]
        [Header("--- REFERENCES ---")]
        // [SerializeField] private Transform _nameTransform;
        [SerializeField] private CharacterController _controller;

        [Space(8)]
        [Header("--- CONFIG ---")]
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _jumpHeight = 2.6f;
        [SerializeField] private float _gravity = -40f;

        Vector3 _joystickDirection, _forwardDirection, _velocity;/* , _moveDiraction; */
        float _joystickAngle, _rotationAngle;
        bool _canUseJoystick = true, canTakeDamage = true;

        Transform _cameraTransform;
        VariableJoystick _joystick;

        public IdleState IdleState;
        public MoveState MoveState;
        public DamagedState DamagedState;
        public AttackState AttackState;
        public DeadState DeadState;
        public WinState WinState;
        public JumpState JumpState;

        public Vector2 GetJoystickDirection() => _joystick.Direction;
        public bool IsGrounded() => _controller.isGrounded;

        public Transform GetTransform() => transform;

        public override void Awake()
        {
            base.Awake();
            _cameraTransform = Camera.main.transform;
        }

        protected override void InitState()
        {
            IdleState = new(this);
            MoveState = new(this);
            AttackState = new(this);
            DamagedState = new(this);
            DeadState = new(this);
            WinState = new(this);
            JumpState = new(this);

            _stateMachine = new();
            _stateMachine.Initialize(IdleState);
        }

        void Update()
        {
            if (GetJoystickDirection().magnitude >= 0.1f && _canUseJoystick)
                HandleInput();

            _stateMachine.CurrentState.LogicUpdate();
        }

        void FixedUpdate()
        {
            _stateMachine.CurrentState.PhysicUpdate();

            if (IsDead()) return;
            HandleMovement();
        }

        public void Win()
        {
            SetActiveJoystick(false);
            ClearVelocity();
            PlayDanceAnimation();
            _currentEquipedWeapon?.gameObject.SetActive(false);
            canTakeDamage = false;
            transform.LookAt(new Vector3(_cameraTransform.position.x, transform.position.y, _cameraTransform.position.z));

            ChangeState(WinState);
            _stateMachine.SetActive(false);
        }

        public bool IsPlayer()
        {
            return true;
        }

        void HandleInput()
        {
            _joystickDirection = _joystick.Direction;
            _joystickAngle = Mathf.Atan2(_joystickDirection.x, _joystickDirection.y) * Mathf.Rad2Deg;
            _forwardDirection = Quaternion.Euler(0, _joystickAngle, 0) * _cameraTransform.forward;
            _forwardDirection.y = 0;
            _forwardDirection.Normalize();

            _rotationAngle = Quaternion.LookRotation(_forwardDirection).eulerAngles.y;
            transform.eulerAngles = new Vector3(0, _rotationAngle, 0);
        }

        void HandleMovement()
        {
            if (_controller.enabled)
                _controller.Move(_joystickDirection.magnitude * _speed * Time.deltaTime * _forwardDirection);
        }

        public void InitJoystick(VariableJoystick joystick)
        {
            _joystick = joystick;
        }

        public void HandleGravity()
        {
            if (_controller.enabled)
            {
                _velocity.y += _gravity * Time.deltaTime;
                _controller.Move(_velocity * Time.deltaTime);
            }
        }

        public void ClearVelocity()
        {
            _forwardDirection = Vector3.zero;
            _velocity.y = 0;
        }

        public void Jump()
        {
            if (IsInState(JumpState)) return;

            _velocity.y = Mathf.Sqrt(2 * -_gravity * _jumpHeight);
            ParticleDust.Stop();
            ParticleDust.Clear();
            ChangeState(JumpState);
        }

        public void Attack()
        {
            if (IsInState(AttackState) || !IsPickedUp()) return;

            ChangeState(AttackState);
        }

        public override void Die()
        {
            if (!canTakeDamage) return;

            base.Die();
            SetActiveJoystick(false);
            ClearVelocity();
            ChangeState(DeadState);
            _stateMachine.SetActive(false);
            OnPlayerDead?.Invoke();
        }

        public void Revive(bool maximumHealh = true)
        {
            if (maximumHealh) MaximumHealth();
            SetActiveJoystick(true);
            _baseCharacter.ToggleGreyScale(false);
            _currentEquipedWeapon?.gameObject.SetActive(true);
            _stateMachine.SetActive(true);
            ChangeState(IdleState);
            SetActiveHealthBar(true);
            OnPlayerRevive?.Invoke(_currentEquipedWeapon != null);
        }

        public override void Damaged(int damageValue)
        {
            if (!canTakeDamage) return;

            base.Damaged(damageValue);
            if (_currentHealth <= 0) Die();
            else ChangeState(DamagedState);
        }

        public override bool IsDead()
        {
            return IsInState(DeadState);
        }

        public override void EquipWeapon(WeaponEquipment weaponEquipment)
        {
            base.EquipWeapon(weaponEquipment);
            OnPlayerEquipWeapon?.Invoke();
        }

        public override void ReplaceWeapon(Func<WeaponType, WeaponEquipment> callback)
        {
            WeaponEquipment weaponEquipment = callback?.Invoke(_currentEquipedWeapon.WeaponType);
            Destroy(_currentEquipedWeapon.gameObject);
            EquipWeapon(weaponEquipment);
        }

        public override void SetupCombatHandler()
        {
            _combatHandler.tag = "Player";
            _combatHandler.SetIsPlayer(true);
            _combatHandler.SetCanPlaySound(true);
        }

        public void SetActiveJoystick(bool value)
        {
            _canUseJoystick = value;
        }


        public void SetActiveHealthBar(bool value)
        {
            _healthBar?.gameObject.SetActive(value);
        }

        public State DecideStateBasedOnCurrentJoystickInput()
        {
            if (GetJoystickDirection().magnitude < 0.1f)
                return ChangeState(IdleState);
            else
                return ChangeState(MoveState);
        }

        public void SetImmortalInSecond(float seconds)
        {
            canTakeDamage = false;
            DOVirtual.DelayedCall(seconds, () => canTakeDamage = true);
        }

        void OnTriggerEnter(Collider other)
        {
            if (IsDead()) return;

            if (other.CompareTag("Bot"))
            {
                if (other.TryGetComponent(out CombatHandler combatHandler))
                {
                    Damaged(combatHandler.GetIncomingWeaponDamage());
                    combatHandler.PlayHitEffect();
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_WEAPON_DAMAGE);
                    NFramework.VibrationManager.I.Haptic(NFramework.VibrationManager.EHapticType.MediumImpact);
                    OnPlayerHit?.Invoke();
                }
            }
        }
    }
}
