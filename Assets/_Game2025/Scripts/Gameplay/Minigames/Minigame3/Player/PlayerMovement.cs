using Redcode.Extensions;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System.Collections;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _jumpHeight = 5f;
        [SerializeField] private float _gravity = -9.8f;
        [SerializeField] private float _speed = 5f;

        private PlayerController _playerController;
        private Transform _cameraTransform;
        private Vector3 _forwardDirection;
        private Vector3 _move;
        private Vector3 _joystickDirection;
        private Vector3 _velocity;
        private float _joystickAngle;
        private float _rotationAngle;
        private bool _isInit;
        private bool _isJumping;
        AudioSource _footStepSound;

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
            PlayerPanelUI.OnJump += Jump;
        }

        private void OnDestroy()
        {
            PlayerPanelUI.OnJump -= Jump;
        }

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
            _isInit = true;
        }

        private void Update()
        {
            if (!_isInit) return;
            HandleMovement();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        private void HandleMovement()
        {
            _joystickDirection = _playerController.Joystick.Direction;
            if (_joystickDirection.magnitude > 0.1f)
            {
                _joystickAngle = Mathf.Atan2(_joystickDirection.x, _joystickDirection.y) * Mathf.Rad2Deg;
                _forwardDirection = Quaternion.Euler(0, _joystickAngle, 0) * _cameraTransform.forward;

                _rotationAngle = Quaternion.LookRotation(_forwardDirection).eulerAngles.y;
                _playerController.transform.eulerAngles = new Vector3(0, _rotationAngle, 0);

                _move = _forwardDirection * _joystickDirection.magnitude;
                PlayFootStepSound(true);
            }
            else
            {
                PlayFootStepSound(false);
                _move = Vector3.zero;
            }

            _playerController.CharacterController.Move(_speed * Time.deltaTime * _move);

            _velocity.y += _gravity * Time.deltaTime;
            _playerController.CharacterController.Move(_velocity * Time.deltaTime);

            if (!_isJumping)
            {
                _playerController.SwitchPlayerAnimation(_joystickDirection.magnitude > 0.1f ? EAnimStyle.Running : EAnimStyle.Idle);
            }
            else
            {
                if (_playerController.CharacterController.isGrounded)
                {
                    _isJumping = false;
                    _playerController.SwitchPlayerAnimation(EAnimStyle.Idle);
                    _playerController.FxSand.Play();
                }
            }
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

        public void Jump()
        {
            if (_playerController.CharacterController.isGrounded)
            {
                _isJumping = true;
                _playerController.SwitchPlayerAnimation(EAnimStyle.Jump, fadeMode: Animancer.FadeMode.FromStart);
                _velocity.y = Mathf.Sqrt(2 * -_gravity * _jumpHeight);
                _playerController.FxSand.Stop();
                _playerController.FxSand.Clear();
                PlayFootStepSound(false);
            }
        }

        public void DisableComponent()
        {
            PlayerPanelUI.OnJump -= Jump;
            this.enabled = false;
            StartCoroutine(CRGravity());
            _footStepSound?.Stop();
            _playerController.SwitchPlayerAnimation(EAnimStyle.Idle);

            IEnumerator CRGravity()
            {
                while (!_playerController.CharacterController.isGrounded)
                {
                    _velocity.y += _gravity * Time.deltaTime;
                    _playerController.CharacterController.Move(_velocity * Time.deltaTime);
                    yield return null;
                }
            }
        }

    }
}
