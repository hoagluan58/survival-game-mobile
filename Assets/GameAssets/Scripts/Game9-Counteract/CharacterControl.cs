using Animancer;
using CnControls;
using DG.Tweening;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using System.Collections;
using UnityEngine;

namespace Game9
{
    public class CharacterControl : MonoBehaviour
    {
        [SerializeField] private float _speedMove;
        [SerializeField] private float _rangeAttack;
        [SerializeField] private Vector2 _rangleX;
        [SerializeField] private Vector2 _rangleZ;
        [SerializeField] private float _intervalAttack;

        [SerializeField] private GameObject _fxBlood;
        [SerializeField] private CharacterAnimationController _animator;
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private ClipTransition _attackAnimClip;
        [SerializeField] private ClipTransition _winAnimClip;
        [SerializeField] private AudioSource _footstepAudioSrc;

        [SerializeField] private GameObject _knifeObject;

        private Rigidbody _rigidBody;
        private SimpleJoystick _joystick;
        private Game9Control _controller;
        private bool _isActive;
        private bool _isCanShoot;
        private float _timeShootPass;

        public void Init(Game9Control controller, SimpleJoystick joystick)
        {
            _controller = controller;
            _joystick = joystick;
            _animator.PlayAnimation(EAnimStyle.IdleGun);
            _fxBlood.SetActive(false);
        }

        public void SetActive(bool b)
        {
            _isActive = b;
            _isCanShoot = b;
        }

        private void Update()
        {
            if (!_isActive) return;

            Vector3 dir = Vector3.zero;
            dir.z = _joystick.VerticalAxis.Value;
            dir.x = _joystick.HorizintalAxis.Value;
            if (dir != Vector3.zero)
            {
                if (!_footstepAudioSrc.isPlaying) _footstepAudioSrc.Play();

                _animator.PlayAnimation(EAnimStyle.RunGun);
                transform.rotation = Quaternion.LookRotation(dir);
                transform.Translate(dir * _speedMove * Time.deltaTime, Space.World);

                Vector3 currentPos = transform.position;
                currentPos.x = Mathf.Clamp(currentPos.x, _rangleX.x, _rangleX.y);
                currentPos.z = Mathf.Clamp(currentPos.z, _rangleZ.x, _rangleZ.y);
                transform.position = currentPos;
            }
            else
            {
                _footstepAudioSrc.Stop();
                _animator.PlayAnimation(EAnimStyle.IdleGun);
            }


            if (_isCanShoot && Time.time >= _timeShootPass + _intervalAttack)
            {
                _timeShootPass = Time.time;
                EnemyBot enemyBot = _controller.GetEnemyInRange(transform.position, _rangeAttack);
                if (enemyBot)
                {
                    Shoot(enemyBot);
                }
            }
        }

        private void Shoot(EnemyBot enemyBot)
        {
            StartCoroutine(IE_Shoot(enemyBot));
        }

        private IEnumerator IE_Shoot(EnemyBot enemyBot)
        {
            _footstepAudioSrc.Stop();
            _animancer.Play(_attackAnimClip);
            _isActive = false;
            transform.rotation = Quaternion.LookRotation(enemyBot.transform.position - transform.position);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG09_KNIFE_SLASH);
            VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
            yield return new WaitForSeconds(0.25f);
            _isActive = true;
            enemyBot.TakeDamage(transform.position);
        }

        public void ShowWin()
        {
            _isActive = false;

            _knifeObject.SetActive(false);
            _animancer.Play(_winAnimClip);

            Vector3 dirToCam = CameraControl.I.transform.position - transform.position;
            dirToCam.y = 0f;
            transform.DORotateQuaternion(Quaternion.LookRotation(dirToCam), 0.25f);
        }

        public void Die()
        {
            _isActive = false;
            _animator.PlayAnimation(EAnimStyle.Die);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
            _fxBlood.SetActive(true);
        }
    }
}
