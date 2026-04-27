using System;
using DG.Tweening;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using UnityEngine;

namespace SquidGame.Minigame18
{
    public class PlayerController : MonoBehaviour
    {
        public static event Action FirstJumpEvent;

        [SerializeField] private Transform _mainTransform;
        [SerializeField] private CharacterAnimationController _animationController;

        private float _baseY;
        private bool _isActive;
        private bool _isJumping;
        private bool _isFirstJump;

        private void Start()
        {
            _baseY = _mainTransform.position.y;
            _animationController.PlayAnimation(EAnimStyle.Idle);
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        public void Jump()
        {
            if (_isJumping) return;

            if (!_isFirstJump)
            {
                FirstJumpEvent?.Invoke();
                _isFirstJump = true;
            }

            _isJumping = true;
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_JUMP);
            _animationController.PlayAnimation(EAnimStyle.Jump);
            _mainTransform.DOKill();
            _mainTransform.DOMoveY(_mainTransform.position.y + 4f, 0.3f)
                .SetEase(Ease.OutExpo)
                .OnComplete(() =>
                {
                    _mainTransform.DOMoveY(_baseY, 0.4f)
                        .SetEase(Ease.InExpo)
                        .OnComplete(() =>
                        {
                            _isJumping = false;
                            VibrationManager.I.Haptic(VibrationManager.EHapticType.SoftImpact);
                            _animationController.PlayAnimation(EAnimStyle.Idle);
                        });
                });
        }

        private void Update()
        {
            if (!_isActive) return;
            if (UIManager.I.IsPointerOverUIObject()) return;

            if (Input.GetMouseButtonDown(0))
            {
                Jump();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isActive) return;
            if (other.gameObject.CompareTag("Dead_Zone"))
            {
                OnHitSpike();
            }
        }

        private void OnHitSpike()
        {
            SetActive(false);
            _isJumping = false;
            _mainTransform.DOKill();
            _mainTransform.DOMoveY(_baseY, 0.3f);
            _animationController.PlayAnimation(EAnimStyle.Die);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_F_HIT_01);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_HIT);
            VibrationManager.I.Haptic(VibrationManager.EHapticType.MediumImpact);
            GameManager.I.Lose();
        }

        public void OnWin()
        {
            SetActive(false);
            _mainTransform.DOKill();
            _mainTransform.DOMoveY(_baseY, 0.3f);
            _mainTransform.DORotate(new Vector3(0f, 180f, 0f), 0.5f);
            _animationController.PlayAnimation(EAnimStyle.Victory_1, EAnimStyle.Victory_2, EAnimStyle.Victory_3);
        }
    }
}