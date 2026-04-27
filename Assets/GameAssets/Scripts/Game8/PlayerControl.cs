using Animancer;
using DG.Tweening;
using NFramework;
using SquidGame;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using UnityEngine;

namespace Game8
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private float _timeJump = 1f;
        [SerializeField] private float _heightJump = 2.5f;
        [SerializeField] private Transform _posJumpWin;

        [Header("ANIMATION")]
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private ClipTransition _jumpAnim;
        [SerializeField] private ClipTransition _idleAnim;
        [SerializeField] private ClipTransition[] _winAnims;
        [SerializeField] private ClipTransition _fallAnim;

        private Vector3 _prevPosition;
        private Game8Control _controller;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Init(Game8Control controller)
        {
            _controller = controller;
            _prevPosition = transform.position;
            _animancer.Play(_idleAnim);
        }

        public bool JumpToShape(TypeShape typeShape)
        {
            var boardController = _controller.BoardControl;
            if (_controller.BoardControl.IsCanJump)
            {
                Tile tile = boardController.GetTileJump(typeShape);
                if (tile.IsBroken) return false;
                boardController.IsCanJump = false;
                JumpTo(tile);
                return true;
            }
            return false;
        }

        public void JumpToWin()
        {
            _controller.SetCountDown(false);
            _animancer.Play(_jumpAnim);
            transform.DOJump(_posJumpWin.position, _heightJump, 1, _timeJump)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _animancer.Play(_winAnims.RandomItem());
                    transform.DORotate(new Vector3(0f, 180f, 0f), 0.25f);
                    _controller.Win();
                });
        }

        private void JumpTo(Tile tile)
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_JUMP);
            _controller.SetCountDown(false);
            _animancer.Play(_jumpAnim);
            _prevPosition = transform.position;
            Tween t = transform.DOJump(tile.transform.position, _heightJump, 1, _timeJump);
            t.SetEase(Ease.Linear);
            t.OnComplete(() =>
            {
                _controller.SetCountDown(true);
                if (tile.IsTrue)
                {
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_JUMP_ON_GLASS);
                    VibrationManager.I.Haptic(VibrationManager.EHapticType.Selection);
                    _controller.BoardControl.ShowNextLine();
                    _animancer.Play(_idleAnim);
                }
                else
                {
                    Fall();
                    tile.Break();
                    GameManager.I.Lose();
                }
            });
        }

        public void Fall()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_SCREAM);
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Warning);
            _rigidbody.isKinematic = false;
            _animancer.Play(_fallAnim);
        }

        public void Revive()
        {
            _animancer.Play(_idleAnim);
            transform.position = _prevPosition;
            transform.eulerAngles = Vector3.zero;
            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector3.zero;
        }
    }
}