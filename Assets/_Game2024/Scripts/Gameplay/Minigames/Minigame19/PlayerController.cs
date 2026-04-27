using System;
using CnControls;
using DG.Tweening;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using UnityEngine;

namespace SquidGame.Minigame19
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _runSpeed = 10f;

        [SerializeField] private CharacterAnimationController _animator;
        [SerializeField] private Transform _playerModel;
        [SerializeField] private Transform _winCameraPosition;
        
        // Refs
        private MapLimit _mapLimit;
        private SimpleJoystick _joystick;

        // Model
        private bool _isActive;

        public void Init(MapLimit mapLimit, SimpleJoystick joystick)
        {
            _mapLimit = mapLimit;
            _joystick = joystick;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        private void Update()
        {
            if (!_isActive) return;
            HandleMove();
        }

        private void HandleMove()
        {
            var dir = Vector3.zero;
            dir.z = _joystick.VerticalAxis.Value;
            dir.x = _joystick.HorizintalAxis.Value;
            if (dir != Vector3.zero)
            {
                _animator.PlayAnimation(EAnimStyle.Run);
                _playerModel.rotation = Quaternion.LookRotation(dir);

                var lastPosition = transform.position;
                transform.Translate(dir * _runSpeed * Time.deltaTime, Space.World);
                var currentPosition = transform.position;
                if (Vector3.Distance(currentPosition, _mapLimit.MapCenterPosition) > _mapLimit.MaxDistance)
                {
                    transform.position = lastPosition;
                }
            }
            else
            {
                _animator.PlayAnimation(EAnimStyle.Idle);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isActive) return;
            if (other.gameObject.CompareTag("Dead_Zone")) Die();
        }

        public void Revive()
        {
            SetActive(true);
            _animator.PlayAnimation(EAnimStyle.Idle);
        }

        private void Die()
        {
            _animator.PlayAnimation(EAnimStyle.Die);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
            VibrationManager.I.Haptic(VibrationManager.EHapticType.MediumImpact);
            GameManager.I.Lose();
        }

        public void OnWin()
        {
            var camera = Camera.main;
            camera.transform.DOMove(_winCameraPosition.position, 0.5f);
            camera.transform.DORotate(_winCameraPosition.eulerAngles, 0.5f);

            _playerModel.DOLocalRotate(Vector3.up * 180f, 0.5f);
            _animator.PlayAnimation(EAnimStyle.Victory_1, EAnimStyle.Victory_2, EAnimStyle.Victory_3);
        }
    }
}