using System;
using Animancer;
using DG.Tweening;
using NFramework;
using SquidGame;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.Minigame01;
using UnityEngine;

namespace Game2
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private LayerMask _jumpLayer;
        [SerializeField] private float _timeJump;
        [SerializeField] private float _jumpPower;

        [Header("Animation")]
        [SerializeField] private CharacterAnimationController _animator;

        // References
        private Game2Control _controller;
        private Camera _mainCam;

        // Internal
        private Rigidbody _rigidbody;
        private Collider _collider;
        private bool _isCanJump;
        private bool _isActive;
        private Vector3 _lastedPos;

        private void Awake()
        {
            _mainCam = Camera.main;
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        public void Init(Game2Control controller)
        {
            _controller = controller;
            _animator.PlayAnimation(EAnimStyle.Idle);
        }

        public void Active()
        {
            _isActive = true;
            _isCanJump = true;

            _lastedPos = transform.position;
            _animator.PlayAnimation(EAnimStyle.Idle);
        }

        private void Update()
        {
            if (!_isActive || !_isCanJump) return;

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, _jumpLayer))
                {
                    GlassPiece glassPiece = hit.transform.parent.GetComponent<GlassPiece>();
                    var step = glassPiece.transform.parent.GetComponent<Step>();
                    if (step && step.IsCanJump && glassPiece) JumpTo(glassPiece.PlayerPos, glassPiece.IsTrueMove, glassPiece, step.IsLastStep);
                }
            }
        }


        private void JumpTo(Vector3 pos, bool isTrueMove, GlassPiece glassPiece, bool isLastStep)
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_JUMP);
            _animator.PlayAnimation(EAnimStyle.Jump);
            _isCanJump = false;

            if (isLastStep && isTrueMove) _controller.StopCountTime();
            transform.DOJump(pos, _jumpPower, 1, _timeJump)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
                    _animator.PlayAnimation(EAnimStyle.Idle);
                    glassPiece.OnPlayerJumpIn();
                    if (isTrueMove)
                    {
                        _isCanJump = true;
                        _controller.DeActiveStep();
                        _controller.ActiveStep();
                        _lastedPos = transform.position;
                        VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
                        GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_JUMP_ON_GLASS);
                    }
                    else
                    {
                        glassPiece.Break(true, true);
                        FallDown();
                    }
                });
        }

        public void JumpToWin(Vector3 pos, Action callBack)
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_JUMP);
            _animator.PlayAnimation(EAnimStyle.Jump);

            _isCanJump = false;
            Tween t = transform.DOJump(pos, _jumpPower, 1, _timeJump);
            t.SetEase(Ease.Linear);
            t.OnComplete(() =>
            {
                VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
                _animator.PlayAnimation(EAnimStyle.Idle);
                callBack?.Invoke();
            });
        }

        public void FallDown()
        {
            _collider.enabled = false;
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_SCREAM);
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.useGravity = true;
            GameManager.I.Lose();
            _animator.PlayAnimation(EAnimStyle.Fall);
        }

        public void PlayWinAnimation()
        {
            _animator.PlayAnimation(EAnimStyle.Victory_1, EAnimStyle.Victory_2, EAnimStyle.Victory_3);
        }

        public void Revive()
        {
            _collider.enabled = true;
            _rigidbody.useGravity = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _animator.PlayAnimation(EAnimStyle.Idle);
            _controller.ReviveStep();
            transform.eulerAngles = Vector3.zero;
            transform.position = _lastedPos;
            Active();
        }
    }
}