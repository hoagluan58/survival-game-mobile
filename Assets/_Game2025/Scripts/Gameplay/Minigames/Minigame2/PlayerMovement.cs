using Sirenix.OdinInspector;
using SquidGame.LandScape.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame2
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private BaseCharacter _model;
        [SerializeField] private VariableJoystick _joystick;
        [SerializeField] private Button _jumpBTN;

        [Header("CONFIG")]
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _gravity = -9.81f;

        private bool _isInit;
        private Rigidbody _rigidbody;
        private Vector3 _joystickDirection, _forwardDirection;
        private float _joystickAngle, _rotationAngle;
        private bool _isJumping;
        private bool _isGrounded;
        private float _verticalVelocity;

        private CharacterAnimator _animator;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _animator = _model.GetCom<CharacterAnimator>();
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _jumpBTN.onClick.AddListener(Jump);
            Init();
        }

        private void Update()
        {
            if (!_isInit) return;

            HandleUserInput();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        private void FixedUpdate()
        {
            if (!_isInit) return;

            // Apply movement 
            Vector3 targetPosition = transform.position + _forwardDirection * _joystickDirection.magnitude * _speed * Time.fixedDeltaTime;
            transform.position = targetPosition;

            // Apply gravity 
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.5f);
            if (!_isGrounded)
            {
                _verticalVelocity += _gravity * Time.fixedDeltaTime;
                transform.position += new Vector3(0, _verticalVelocity * Time.fixedDeltaTime, 0);
            }
            else
            {
                if (_isJumping)
                {
                    _isJumping = false;
                    _animator.PlayAnimation(EAnimStyle.Idle);
                }
                _verticalVelocity = 0;
            }

            // Handle animations
            if (!_isJumping)
            {
                _animator.PlayAnimation(_joystickDirection.magnitude > 0.1f ? EAnimStyle.Running : EAnimStyle.Idle);
            }
        }

        [Button]
        public void Init()
        {
            _isInit = true;
        }

        private void HandleUserInput()
        {
            _joystickDirection = _joystick.Direction;
            if (_joystickDirection.magnitude > 0.1f)
            {
                _joystickAngle = Mathf.Atan2(_joystickDirection.x, _joystickDirection.y) * Mathf.Rad2Deg;
                _forwardDirection = Quaternion.Euler(0, _joystickAngle, 0) * _camera.transform.forward;
                _forwardDirection.y = 0;
                _rotationAngle = Quaternion.LookRotation(_forwardDirection).eulerAngles.y;
                transform.eulerAngles = new Vector3(0, _rotationAngle, 0);
            }
        }

        public void Jump()
        {
            if (_isGrounded)
            {
                _isJumping = true;
                _isGrounded = false;
                _animator.PlayAnimation(EAnimStyle.Jump);
                _verticalVelocity = Mathf.Sqrt(-2 * _gravity * _jumpForce);
            }
        }
    }
}
