using Animancer;
using SquidGame.Core;
using SquidGame.LandScape.Game;
using UnityEngine;

namespace SquidGame.LandScape.Minigame5
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private BaseCharacter _baseCharacter;

        [Header("Animation")]
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private bool _isPullLeft;

        private Game5Controller _controller;
        private bool _isFall;
        private Rigidbody _rigidbody;
        private Collider _collider;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        public void Init(Game5Controller controller) => _controller = controller;

        private void OnEnable()
        {
            _animator.PlayAnimation(EAnimStyle.Idle);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(Tag.Dead_Zone))
            {
                _animator.PlayAnimation(EAnimStyle.Die);
                _baseCharacter.ToggleGreyScale(true);
                if (!_controller.IsWin)
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag(Tag.Ground))
            {
                Fall();
            }
        }

        private void Fall()
        {
            _isFall = true;
            _animator.PlayAnimation(EAnimStyle.Falling);
            transform.SetParent(_controller.DeadCharacterParent);
        }

        public void PullAnimation()
        {
            _animator.PlayAnimation(_isPullLeft ? EAnimStyle.Pull_Left : EAnimStyle.Pull_Right);
        }

        private void FixedUpdate()
        {
            if (_isFall)
            {
                _rigidbody.AddForce(Vector3.down * 50f);
            }
        }

        public void Win()
        {
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
            _animator.PlayAnimation(EAnimStyle.Victory_1);
        }

        public void Lose()
        {
            if (!_isFall)
                _rigidbody.AddRelativeForce((Vector3.forward + Vector3.up /* * 0.5f */) * 1800f);
        }
    }
}