using Redcode.Extensions;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System.Collections;
using UnityEngine;

namespace SquidGame.LandScape.Minigame2
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private BaseCharacter _model;

        [Header("CONFIG")]
        [SerializeField] private float _speed = 4.5f;
        [SerializeField] private float _jumpHeight = 2.2f;
        [SerializeField] private float _jumpSpeedMultiplier = 0.2f;
        [SerializeField] private float _gravity = -9.8f;
        [SerializeField] private float _fallGravityMultiplier = 1.5f;

        private GlassPanel _lastValidGlassPanel;
        private VariableJoystick _joystick;
        private Vector3 _joystickDirection, _forwardDirection;
        private float _joystickAngle, _rotationAngle;
        private Vector3 _velocity;
        private Vector3 _move;

        private CharacterAnimator _animator;
        private bool _isActive, _isJumping;
        private Transform _cameraTransform;

        private MinigameController _controller;
        private bool _isWin;
        private bool _isFalling;
        private bool _isDie = false;

        public bool IsWin => _isWin;

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
            _animator = _model.GetCom<CharacterAnimator>();
        }

        public void Init(VariableJoystick joystick, MinigameController controller)
        {
            _joystick = joystick;
            _controller = controller;
            _lastValidGlassPanel = null;
            _characterController.enabled = false;
        }

        public bool HasValidGlassPanel => _lastValidGlassPanel != null;

        public void OnRevive()
        {
            StartCoroutine(CROnRevive());

            IEnumerator CROnRevive()
            {
                _characterController.enabled = false;
                var revivePos = _lastValidGlassPanel.transform.position + new Vector3(0, 1f, 0f);
                _velocity.y = 0f;
                transform.position = revivePos;
                _isFalling = false;
                _isDie = false;
                _animator.PlayAnimation(EAnimStyle.Idle);
                yield return null; // Wait a frame then turn on character controller -- ??
                _characterController.enabled = true;
                SetActive(true);
                _model.ToggleGreyScale(false);
            }
        }

        private void FixedUpdate()
        {
            HandleUserInput();
            ApplyMovement();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        private void HandleUserInput()
        {
            if (!_isActive) return;

            _joystickDirection = _joystick.Direction;
            if (_joystickDirection.magnitude > 0.1f)
            {
                _joystickAngle = Mathf.Atan2(_joystickDirection.x, _joystickDirection.y) * Mathf.Rad2Deg;
                _forwardDirection = Quaternion.Euler(0, _joystickAngle, 0) * _cameraTransform.forward;

                _rotationAngle = Quaternion.LookRotation(_forwardDirection).eulerAngles.y;
                transform.eulerAngles = new Vector3(0, _rotationAngle, 0);

                _move = _forwardDirection * _joystickDirection.magnitude;
            }
            else
            {
                if (_move != Vector3.zero)
                {
                    _move = Vector3.zero;
                }
            }
        }

        private void ApplyMovement()
        {
            var isGrounded = _characterController.isGrounded;

            if (!isGrounded)
            {
                _velocity.y += _gravity * Time.fixedDeltaTime;

                // Apply higher gravity when falling
                if (_velocity.y < 0)
                {
                    _velocity.y += _gravity * _fallGravityMultiplier * Time.fixedDeltaTime;
                }
            }

            _characterController.Move(_speed * Time.deltaTime * _move);
            _characterController.Move(_velocity * Time.deltaTime);

            if (_isDie || _isFalling || _isWin) return;

            if (!_isJumping)
            {
                _animator.PlayAnimation(_joystickDirection.magnitude > 0.1f ? EAnimStyle.Running : EAnimStyle.Idle);
            }
            else
            {
                if (_characterController.isGrounded)
                {
                    _isJumping = false;
                    _animator.PlayAnimation(EAnimStyle.Idle);
                }
            }
        }

        public void Jump()
        {
            if (!_isActive || !_characterController.isGrounded) return;

            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_JUMP);
            _isJumping = true;
            _animator.PlayAnimation(EAnimStyle.Jump, 0.2f, Animancer.FadeMode.FromStart);

            _velocity.y = Mathf.Sqrt(2 * -_gravity * _jumpHeight);

            // Give an initial boost for better feeling
            _move += transform.forward * _jumpSpeedMultiplier;
        }

        public void OnStartGame()
        {
            SetActive(true);
            _characterController.enabled = true;
        }

        public void SetActive(bool value)
        {
            _isActive = value;
            if (!value)
            {
                _move = Vector2.zero;
            }
        }

        public void Falling()
        {
            if (_isFalling) return;

            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_SCREAM);
            _move = Vector2.zero;
            _isActive = false;
            _isFalling = true;
            _animator.PlayAnimation(EAnimStyle.Falling);
        }

        public void Die()
        {
            _isDie = true;
            _move = Vector2.zero;
            _animator.PlayAnimation(EAnimStyle.Die);
            _model.ToggleGreyScale(true);
            _controller.Lose();
        }

        public void Win()
        {
            _isWin = true;
        }

        public void PlayDanceAnim()
        {
            _isActive = false;
            _move = Vector2.zero;
            _controller.Win();
            _animator.PlayAnimation(EAnimStyle.Victory_1, EAnimStyle.Victory_2, EAnimStyle.Victory_3);
            transform.LookAt(_cameraTransform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<CollidableType>(out var component))
            {
                switch (component.Type)
                {
                    case CollidableType.EObjectType.GlassPanel:
                        GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_JUMP_ON_GLASS);
                        if (component.TryGetComponentInParent<GlassPanel>(out var glassPanel))
                        {
                            var isTrueMove = glassPanel.Data.IsTrueMove;
                            if (!isTrueMove)
                            {
                                glassPanel.Break();
                                Falling();
                            }
                            else
                            {
                                _lastValidGlassPanel = glassPanel;
                            }
                        }
                        return;
                    case CollidableType.EObjectType.TriggerFalling:
                        Falling();
                        return;
                    case CollidableType.EObjectType.TriggerWin:
                        Win();
                        return;
                    case CollidableType.EObjectType.TriggerDeath:
                        Die();
                        return;
                    case CollidableType.EObjectType.DanceZone:
                        PlayDanceAnim();
                        return;
                }
            }
        }
    }
}
