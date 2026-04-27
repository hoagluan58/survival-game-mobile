using Animancer;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.Survival;
using System;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class Unit : MonoBehaviour, IPickupable, IDamageable
    {
        [Header("--- UNIT ---")]
        [SerializeField] protected BaseCharacter _baseCharacter;
        [SerializeField] protected Survival.CharacterAnimator _animator;
        [SerializeField] protected WeaponAnimator _weaponAnimator;
        [SerializeField] protected HealthBar _healthBar;
        [SerializeField] private ParticleSystem _bloodFx;
        public ParticleSystem ParticleDust;


        [Space(8)]
        [Header("--- HEALTH ---")]
        [SerializeField] protected int _maxHealth = 30;
        [SerializeField] protected int _currentHealth = 30;

        protected ANIMATION _currentAnimation = ANIMATION.Idle;
        protected WeaponEquipment _currentEquipedWeapon;
        protected StateMachine _stateMachine;
        protected CombatHandler _combatHandler;

        public CombatHandler CombatHandler => _combatHandler;
        public WeaponEquipment CurrentWeapon => _currentEquipedWeapon;

        public virtual void Awake()
        {
            InitState();
            MaximumHealth();
        }

        protected void MaximumHealth()
        {
            _currentHealth = _maxHealth;
            _healthBar.Init(_maxHealth);
        }

        public void MaximumHealth(int value)
        {
            _maxHealth = value;
            MaximumHealth();
        }

        public void SetHealth(int value)
        {
            value = Mathf.Clamp(value, 0, _maxHealth);

            _currentHealth = value;
            UpdateHealthBarUI();
        }

        public void Heal(int value)
        {
            _currentHealth += value;
            if (_currentHealth > _maxHealth)
                _currentHealth = _maxHealth;

            UpdateHealthBarUI();
        }

        public bool IsInState(State state)
        {
            return _stateMachine.IsInState(state);
        }

        public State ChangeState(State state)
        {
            _stateMachine.ChangState(state);
            return state;
        }

        public AnimancerState PlayAnimation(ANIMATION animation, FadeMode fadeMode = FadeMode.FixedSpeed, float fadeDuration = 0.2f)
        {
            _currentAnimation = animation;
            AnimancerState playerAnimationState = _animator.PlayAnimation(GetWeaponType(), animation, fadeMode, fadeDuration);
            _weaponAnimator.PlayAnimation(IsPickedUp() ? _currentEquipedWeapon.WeaponType : WeaponType.Default, animation, fadeMode, fadeDuration);
            return playerAnimationState;
        }

        private WeaponType GetWeaponType()
        {
            if (IsDead()) return WeaponType.Default;
            if (!IsPickedUp()) return WeaponType.Default;
            return _currentEquipedWeapon.WeaponType;
        }

        public AnimancerState PlayDeadAnimation()
        {
            _currentAnimation = ANIMATION.Dead;
            return _animator.PlayAnimation(WeaponType.Default, _currentAnimation);
        }

        public void PlayDanceAnimation()
        {
            _currentAnimation = _animator.PlayDanceAnimation();
        }

        public virtual void EquipWeapon(WeaponEquipment weaponEquipment)
        {
            _currentEquipedWeapon = weaponEquipment;
            _combatHandler = weaponEquipment.CombatHandler;
            SetupCombatHandler();
            _combatHandler.SetWeapon(_currentEquipedWeapon);
            _weaponAnimator.SetAnimancer(_currentEquipedWeapon.Animancer);

            weaponEquipment.transform.SetParent(_weaponAnimator.transform);
            weaponEquipment.transform.localPosition = Vector3.zero;
            weaponEquipment.transform.localEulerAngles = Vector3.zero;
            weaponEquipment.transform.localScale = Vector3.one;

            PlayAnimation(_currentAnimation, FadeMode.FixedDuration, 0f);
        }

        public virtual void SetupCombatHandler()
        {

        }

        public void ActiveHitbox(float delayToActive, float duration)
        {
            _combatHandler.ActiveHitbox(delayToActive, duration);
        }

        public void CancelHitbox()
        {
            _combatHandler?.CancleHitBox();
        }

        public virtual void ReplaceWeapon(Func<WeaponType, WeaponEquipment> callback) { }

        protected virtual void InitState() { }

        public bool IsPickedUp()
        {
            return _currentEquipedWeapon != null;
        }

        public virtual void Die()
        {
            CancelHitbox();
            _baseCharacter.ToggleGreyScale(true);
            _healthBar?.gameObject.SetActive(false);
            _currentEquipedWeapon?.gameObject.SetActive(false);
        }

        public virtual bool IsDead()
        {
            return false;
        }

        public virtual void Damaged(int damageValue)
        {
            _bloodFx.Play();
            _currentHealth -= damageValue;
            if (_currentHealth <= 0) _currentHealth = 0;
            UpdateHealthBarUI();
        }

        private void UpdateHealthBarUI()
        {
            _healthBar.UpdateUI(_currentHealth, _currentHealth / (float)_maxHealth);
        }
    }
}
