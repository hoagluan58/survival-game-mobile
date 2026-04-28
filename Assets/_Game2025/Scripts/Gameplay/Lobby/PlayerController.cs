using Animancer;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using UnityEngine;

namespace SquidGame.LandScape.Lobby
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private ParticleSystem _dustFx;
        [SerializeField] private Transform _nameTransform;

        [Header("CONFIG")]
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _jumpHeight = 5f;
        [SerializeField] private float _gravity = -9.8f;

        LobbyUI _lobbyUI;
        Vector3 _joystickDirection, _forwardDirection, _velocity, _move;
        float _joystickAngle, _rotationAngle;
        bool _isActive, _isJumping;
        Transform _cameraTransform;
        AudioSource _footStepSound;

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
            _animator.PlayAnimation(EAnimStyle.Idle);
        }

        void OnDisable()
        {
            PlayFootStepSound(false);
            _isJumping = false;
        }

        public void Init(LobbyUI lobbyUI)
        {
            _lobbyUI = lobbyUI;
            // _isActive = true;
        }

        private void FixedUpdate()
        {
            if (!_isActive) return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            HandleUserInput();
        }

        void LateUpdate()
        {
            _nameTransform.LookAt(_cameraTransform);
        }

        private void HandleUserInput()
        {
            _joystickDirection = _lobbyUI.GetDirection();
            if (_joystickDirection.magnitude > 0.1f)
            {
                _joystickAngle = Mathf.Atan2(_joystickDirection.x, _joystickDirection.y) * Mathf.Rad2Deg;
                _forwardDirection = Quaternion.Euler(0, _joystickAngle, 0) * _cameraTransform.forward;

                _rotationAngle = Quaternion.LookRotation(_forwardDirection).eulerAngles.y;
                transform.eulerAngles = new Vector3(0, _rotationAngle, 0);

                _move = _forwardDirection * _joystickDirection.magnitude;
                PlayFootStepSound(true);
            }
            else
            {
                _move = Vector3.zero;
                PlayFootStepSound(false);
            }

            if (!_characterController.isGrounded)
            {
                _velocity.y += _gravity * Time.fixedDeltaTime;
            }

            _characterController.Move(_speed * Time.deltaTime * _move);
            _characterController.Move(_velocity * Time.deltaTime);

            if (!_isJumping)
            {
                _animator.PlayAnimation(_joystickDirection.magnitude > 0.1f ? EAnimStyle.Running : EAnimStyle.Idle, 0.2f);
            }
            else
            {
                if (_characterController.isGrounded)
                {
                    _isJumping = false;
                    _animator.PlayAnimation(EAnimStyle.Idle, 0.2f);
                    _dustFx.Play();
                }
            }
        }

        public void Jump()
        {
            if (_characterController.isGrounded && !_isJumping)
            {
                _isJumping = true;
                _animator.PlayAnimation(EAnimStyle.Jump, 0.2f, FadeMode.FromStart);
                _velocity.y = Mathf.Sqrt(2 * -_gravity * _jumpHeight);
                _dustFx.Stop();
                _dustFx.Clear();
                PlayFootStepSound(false);
            }
        }

        public void SetActive()
        {
            _isActive = true;
        }

        public void PlayAnimationDance()
        {
            _animator.PlayAnimation(EAnimStyle.Victory_1);
        }

        public void PlayFootStepSound(bool isPlay)
        {
            if (isPlay)
            {
                if (_footStepSound == null && !_isJumping) _footStepSound = GameSound.I.PlaySFX(Define.SoundPath.SFX_FOOT_STEP, true);
            }
            else
            {
                if (_footStepSound != null)
                {
                    _footStepSound.Stop();
                    _footStepSound = null;
                }
            }
        }
    }
}
