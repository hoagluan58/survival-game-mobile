using DG.Tweening;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using System;
using UnityEngine;

namespace SquidGame.Minigame18
{
    public class BotController : MonoBehaviour
    {
        public event Action DieEvent;

        [SerializeField] private CharacterAnimationController _animator;
        [SerializeField] private Transform _mainTransform;

        private bool _isActive;
        private bool _isFirstJump;
        private float _jumpTimer;
        private int _jumpCount; // Current jump count

        private int _noJumpTime = 3; // Time (as jump count) bot not jump
        private float _firstJumpTime = 1f; // Interval from start to first jump
        private float _jumpTime = 5f; // Interval between 2 jumps

        public bool IsAlive => _isActive;

        private void OnDisable()
        {
            _mainTransform.DOKill();
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        public void Init(int noJumpTime, float firstJumpTime, float jumpTime)
        {
            _noJumpTime = noJumpTime;
            _firstJumpTime = firstJumpTime;
            _jumpTime = jumpTime;
            _animator.PlayAnimation(EAnimStyle.Idle);
        }

        private void Jump()
        {
            _jumpCount++;
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_JUMP);
            _animator.PlayAnimation(EAnimStyle.Jump);
            _mainTransform.DOKill();
            _mainTransform.DOMoveY(_mainTransform.position.y + 4f, 0.3f)
                .SetEase(Ease.OutExpo)
                .SetDelay(_jumpCount == _noJumpTime ? 0.1f : 0) // Jump wrong when in wrong jump time
                .OnComplete(() =>
                {
                    _mainTransform.DOMoveY(0, 0.4f)
                        .SetEase(Ease.InExpo)
                        .OnComplete(() => { _animator.PlayAnimation(EAnimStyle.Idle); });
                });
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
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_F_HIT_01);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_HIT);
            _mainTransform.DOKill();
            _mainTransform.DOMoveY(0, 0.3f)
                .OnComplete(() =>
                {
                    _mainTransform.DOMoveY(-1f, 3f)
                        .SetDelay(2f);
                });
            _animator.PlayAnimation(EAnimStyle.Die);
            DieEvent?.Invoke();
        }

        private void FixedUpdate()
        {
            if (!_isActive) return;

            _jumpTimer += Time.fixedDeltaTime;

            if (!_isFirstJump)
            {
                if (_jumpTimer >= _firstJumpTime)
                {
                    _jumpTimer = 0;
                    _isFirstJump = true;
                    Jump();
                }
            }
            else
            {
                if (_jumpTimer >= _jumpTime)
                {
                    _jumpTimer = 0;
                    Jump();
                }
            }
        }
    }
}