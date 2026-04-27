using Animancer;
using DG.Tweening;
using NFramework;
using SquidGame.Core;
using SquidGame.LandScape;
using UnityEngine;

namespace Game2
{
    public class BotController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [Header("ANIMANCER")]
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private ClipTransition _idleClip;
        [SerializeField] private ClipTransition _jumpClip;
        [SerializeField] private ClipTransition _fallClip;

        private int _currentIndex = -1;
        private bool _isAlive;
        private bool _isJumping;
        private GlassPiece _curGlassStanding;

        public bool IsAlive => _isAlive;
        public int CurrentIndex => _currentIndex;

        public void Init()
        {
            _isAlive = true;
            _animancer.Play(_idleClip);
        }

        public void JumpTo(GlassPiece glassPiece, Step step, System.Action onJumpAlive)
        {
            _curGlassStanding?.OnBotJumpOut(transform.position);
            _curGlassStanding = glassPiece;
            var newPosition = _curGlassStanding.GetValidBotPosition();
            _curGlassStanding.OnBotJumpIn(newPosition);
            var isTrueMove = glassPiece.IsTrueMove;
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_JUMP);
            _animancer.Play(_jumpClip);
            _isJumping = true;
            transform.DOJump(newPosition, 2.5f, 1, 1f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _isJumping = false;
                    VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
                    _animancer.Play(_idleClip);
                    if (!isTrueMove)
                    {
                        glassPiece.Break(true, true);
                        FallDown();
                    }
                    else
                    {
                        GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_JUMP_ON_GLASS);
                        _currentIndex = glassPiece.StepIndex;
                        onJumpAlive?.Invoke();
                    }
                });
        }

        public void JumpToWin(Vector3 winPosition)
        {
            _curGlassStanding?.OnBotJumpOut(transform.position);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_JUMP);
            _animancer.Play(_jumpClip);
            _isAlive = false;
            transform.DOJump(winPosition, 2.5f, 1, 1f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
                    _animancer.Play(_idleClip);
                });
        }

        public void FallDown()
        {
            _isAlive = false;
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_SCREAM);
            _animancer.Play(_fallClip);
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
            _rigidbody.constraints = RigidbodyConstraints.None;
        }
    }
}