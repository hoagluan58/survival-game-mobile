using SquidGame.LandScape.Core;
using System.Collections;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class CombatHandler : MonoBehaviour
    {
        private Collider _hitboxCollider;
        [SerializeField] private WeaponEquipment _currentEquipedWeapon;
        [SerializeField] private ParticleSystem _particleHit, _particleSlash;
        private bool _isPlayer;
        private bool _canPlaySound;
        private Coroutine _hitboxCoroutine;

        public int GetIncomingWeaponDamage() => _currentEquipedWeapon.Stats.Damage;
        public void PlayHitEffect() => _particleHit.Play();

        public void SetWeapon(WeaponEquipment weaponEquipment)
        {
            _currentEquipedWeapon = weaponEquipment;
        }

        public void SetCanPlaySound(bool canPlay) => _canPlaySound = canPlay;
        public void SetIsPlayer(bool isPlayer) => _isPlayer = isPlayer;

        private void Awake()
        {
            _hitboxCollider = GetComponent<Collider>();
        }

        public void ActiveHitbox(float delayToActive, float duration)
        {
            _hitboxCoroutine = StartCoroutine(ActiveHitbox());
            _particleSlash.Play();

            IEnumerator ActiveHitbox()
            {
                yield return new WaitForSeconds(delayToActive);
                PlayAttackSound();
                _hitboxCollider.enabled = true;
                yield return new WaitForSeconds(duration);
                _hitboxCollider.enabled = false;
            }
        }

        public void CancleHitBox()
        {
            _particleSlash.Clear();
            if (_hitboxCoroutine != null)
            {
                StopCoroutine(_hitboxCoroutine);
                _hitboxCoroutine = null;
            }
            _hitboxCollider.enabled = false;
        }

        public void CancleSlashEffect()
        {
            _particleSlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        public void PlayAttackSound()
        {
            if (!_isPlayer && !_canPlaySound) return;
            switch (_currentEquipedWeapon.WeaponType)
            {
                case WeaponType.Axe:
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_AXE_SLASH);
                    break;
                case WeaponType.Baseball:
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_BASEBALL_BAT_SWING);
                    break;
                case WeaponType.Sword:
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_SWORD_SLASH);
                    break;
                case WeaponType.Katana:
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_KATANA_SLASH);
                    break;
            }
        }
    }
}
