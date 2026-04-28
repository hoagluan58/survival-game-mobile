using Animancer;
using Cinemachine;
using DG.Tweening;
using NFramework;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    public interface IDead
    {
        public void Dead();
    }

    public class PlayerController : Character, IDead
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private BaseCharacter _baseCharacter;
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private ParticleSystem _dustFx;
        [SerializeField] private Transform _nameTransform;
        [SerializeField] private Transform _headTransform;
        [SerializeField] private ParticleSystem _fxBloodSplat;
        [SerializeField] private ParticleSystem _fxBloodPool;
        [SerializeField] private CinemachineFreeLook _freeLook;
        [Header("CONFIG")]
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _jumpHeight = 5f;
        [SerializeField] private float _gravity = -9.8f;

        private VariableJoystick _joystick;
        private Vector3 _joystickDirection, _forwardDirection, _velocity, _move;
        private float _joystickAngle, _rotationAngle;
        private bool _isActive, _isJumping;
        private Transform _cameraTransform;
        private AudioSource _footStepSound;
        private bool _isJoystickMove = false;
        public Transform Head => _headTransform;

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;

            _fxBloodPool.gameObject.SetActive(false);
            _fxBloodSplat.gameObject.SetActive(false);
        }

        private void Start()
        {
            _animator.PlayAnimation(EAnimStyle.Idle);
        }


        void OnDisable()
        {
            PlayFootStepSound(false);
            _isJumping = false;
        }

        public void Init(VariableJoystick joystick)
        {
            _joystick = joystick;
            //_isActive = true;
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


        private Vector2 GetDirection()
        {
#if UNITY_EDITOR || UNITY_WEBGL
            float h = 0f, v = 0f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))  h -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) h += 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))  v -= 1f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))    v += 1f;
            var wasd = new Vector2(h, v).normalized;
            if (wasd.magnitude > 0.01f) return wasd;
#endif
            return _joystick.Direction;
        }

        private void HandleUserInput()
        {
            _joystickDirection = GetDirection();

            _isJoystickMove = _joystickDirection.magnitude > 0.1f;
            _dustFx.gameObject.SetActive(_isJoystickMove);
            if (_isJoystickMove)
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
                _animator.PlayAnimation(_isJoystickMove ? EAnimStyle.Running : EAnimStyle.Idle, 0.2f);

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

        public void SetActive(bool value)
        {
            _isActive = value;
            if (_isActive == false) _joystickDirection = Vector3.zero;
        }

        public void PlayAnimation(EAnimStyle animation)
        {
            _animator.PlayAnimation(animation, 0);
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

        public bool IsInPodium()
        {
            return true;
        }

        public void SetParent(Transform transform, bool isFollow)
        {
            this.transform.parent = transform;
        }

        public void Dead()
        {
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Warning);
            _isActive = false;
            _baseCharacter.ToggleGreyScale(true);
            _animator.PlayAnimation(EAnimStyle.Die);
            PlayFootStepSound(false);
            _fxBloodSplat.gameObject.SetActive(true);
            _fxBloodSplat.Play();
            this.InvokeDelay(0.5f, () =>
            {
                _fxBloodPool.gameObject.SetActive(true);
                _fxBloodPool.Play();
            });
        }

        public void Revive()
        {
            _fxBloodSplat.Stop();
            _fxBloodPool.Stop();
            _fxBloodPool.gameObject.SetActive(false);
            _fxBloodSplat.gameObject.SetActive(false);
            SetEnableOutline(false);
            _baseCharacter.ToggleGreyScale(false);
            _animator.PlayAnimation(EAnimStyle.Idle);
        }

        public CharacterAnimator GetInstantiateCharacter()
        {
            return Instantiate(_animator);
        }

        [Button]
        public void Dance()
        {
            _animator.PlayAnimation(EAnimStyle.Victory_1, EAnimStyle.Victory_2, EAnimStyle.Victory_3, 0);
        }


        internal void Stanstill()
        {
            PlayFootStepSound(false);
            _animator.PlayAnimation(EAnimStyle.Idle, 0);
        }

        public void EnterRoom(Transform roomCamera)
        {
            //disable rotate 
            //_freeLook.m_Follow = null;
            //_freeLook.m_LookAt = null;
            //_freeLook.enabled = false;
            //_freeLook.gameObject.SetActive(false);
            //_freeLook.transform.position = roomCamera.transform.position;
            //_freeLook.transform.eulerAngles = roomCamera.transform.eulerAngles;

        }


        public void ExitRoom()
        {
            //_freeLook.m_Follow = this.transform;
            //_freeLook.m_LookAt = this.transform;
            //_freeLook.enabled = true;
            //_freeLook.gameObject.SetActive(true);
        }


    }
}
