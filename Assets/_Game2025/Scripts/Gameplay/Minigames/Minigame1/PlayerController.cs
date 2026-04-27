using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System.Collections;
using UnityEngine;

namespace SquidGame.LandScape.Minigame1
{

    #region State machine character animation
    public enum CharacterAnimationState
    {
        None,
        Idle,
        Jump,
        Move,
        Die,
        Dance,
        Stun
    }

    public class AnimationState
    {
        protected CharacterAnimator _animator;
        protected CharacterAnimationState _state;
        public virtual AnimationState Init(CharacterAnimator animator, CharacterAnimationState state)
        {
            _animator = animator;
            _state = state;
            return this;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
    }


    public class IdleAnimationState : AnimationState
    {

        public override void Enter()
        {
            base.Enter();
            _animator.PlayAnimation(EAnimStyle.Idle);
        }
    }

    public class StunAnimationState : AnimationState
    {
        public override void Enter()
        {
            base.Enter();
            _animator.PlayAnimation(EAnimStyle.Stand_Still_Pose_1, EAnimStyle.Stand_Still_Pose_2);
        }
    }

    public class JumpAnimationState : AnimationState
    {
        public override void Enter()
        {
            base.Enter();
            _animator.PlayAnimation(EAnimStyle.Jump, 0.2f, Animancer.FadeMode.FromStart);
        }

    }

    public class MoveAnimationState : AnimationState
    {
        public override void Enter()
        {
            base.Enter();
            _animator.PlayAnimation(EAnimStyle.Running);
        }
    }

    public class DieAnimationState : AnimationState
    {
        public override void Enter()
        {
            base.Enter();
            _animator.PlayAnimation(EAnimStyle.Die);
        }
    }

    public class DanceAnimationState : AnimationState
    {
        public override void Enter()
        {
            base.Enter();
            _animator.PlayAnimation(EAnimStyle.Victory_1, EAnimStyle.Victory_2, EAnimStyle.Victory_3);
        }
    }

    #endregion


    public class PlayerController : MonoBehaviour
    {
        [Header(" Control Settings ")]
        public float maxX;
        public float maxZ;
        public static bool canMove;
        public bool IsPlaying, IsDie, IsWin, IsRunning;
        [SerializeField] private bool _isActive, _isJumping;
        [Header("CONFIG")]
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _jumpHeight = 5f;
        [SerializeField] private float _gravity = -9.8f;
        [Header("REFERENCES")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private CharacterAnimator _characterAnimator;

        [SerializeField] private Transform _cameraTarget;
        [SerializeField] private Transform _headPos;
        [SerializeField] private GameObject _winFX;
        [SerializeField] private GameObject _fxBloodSplat;
        [SerializeField] private GameObject _fxBloodPool;
        [SerializeField] private GameObject _aimPointGo;
        [SerializeField] private GameObject _duskGo;
        [SerializeField] private BaseCharacter _baseCharacter;
        [SerializeField] private CharacterAnimationState _currentCharacterAnimationState;
        private Minigame01MenuUI _ui;
        private HunterController _hunter;
        private MinigameController _controller;

        private VariableJoystick _joystick; // kept for legacy; use _ui.Input.GetDirection() for movement
        private Vector3 _joystickDirection;
        private Vector3 _velocity;
        private Vector3 _move;
        private AudioSource _runSource;

        public Transform TargetCamera => _cameraTarget;

        private AnimationState _idleAnimationState;
        private AnimationState _jumpAnimationState;
        private AnimationState _stunAnimationState;
        private AnimationState _runAnimationState;
        private AnimationState _danceAnimationState;
        private AnimationState _dieAnimationState;

        private AnimationState _currentAnimationState;

        private void Start()
        {
            ChangeAnimationState(CharacterAnimationState.Idle);
        }


        public void Init(MinigameController controller, Minigame01MenuUI ui)
        {
            _ui = ui;
            _joystick = _ui.Input.GetJoystick(); // legacy reference kept
            _controller = controller;
            _hunter = _controller.HunterController;
            _characterAnimator.PlayAnimation(EAnimStyle.Idle);
            _fxBloodPool.SetActive(false);
            _fxBloodSplat.SetActive(false);
            canMove = true;
            IsRunning = false;

            _idleAnimationState = new IdleAnimationState().Init(_characterAnimator, CharacterAnimationState.Idle);
            _jumpAnimationState = new JumpAnimationState().Init(_characterAnimator, CharacterAnimationState.Jump);
            _stunAnimationState = new StunAnimationState().Init(_characterAnimator, CharacterAnimationState.Stun);
            _runAnimationState = new MoveAnimationState().Init(_characterAnimator, CharacterAnimationState.Move);
            _danceAnimationState = new DanceAnimationState().Init(_characterAnimator, CharacterAnimationState.Dance);
            _dieAnimationState = new DieAnimationState().Init(_characterAnimator, CharacterAnimationState.Die);
        }

        public void ChangeAnimationState(CharacterAnimationState state)
        {
            if (_currentCharacterAnimationState == state) return;
            _currentCharacterAnimationState = state;
            _currentAnimationState?.Exit();
            switch (_currentCharacterAnimationState)
            {
                case CharacterAnimationState.Idle:
                    _currentAnimationState = _idleAnimationState;
                    break;
                case CharacterAnimationState.Jump:
                    _currentAnimationState = _jumpAnimationState;
                    break;
                case CharacterAnimationState.Move:
                    _currentAnimationState = _runAnimationState;
                    break;
                case CharacterAnimationState.Die:
                    _currentAnimationState = _dieAnimationState;
                    break;
                case CharacterAnimationState.Dance:
                    _currentAnimationState = _danceAnimationState;
                    break;
                case CharacterAnimationState.Stun:
                    _currentAnimationState = _stunAnimationState;
                    break;
                default:
                    _currentAnimationState = _idleAnimationState;
                    break;
            }
            _currentAnimationState.Enter();
        }

        public void PlayFootStepSound(bool isPlay)
        {
            if (isPlay)
            {
                if (_runSource == null && !_isJumping) _runSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_FOOT_STEP, true);
            }
            else
            {
                if (_runSource != null)
                {
                    _runSource.Stop();
                    _runSource = null;
                }
            }
        }


        private void Update()
        {
            HandleUserInput();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            _duskGo.SetActive(!_isJumping);
        }



        private void HandleIdleUpdate()
        {
            //idle
            if (_move != Vector3.zero)
            {
                _move = Vector3.zero;
            }

            if (IsRunning)
            {
                if (!_isJumping)
                {
                    ChangeAnimationState(CharacterAnimationState.Stun);
                }
            }
            else
            {
                if (!_isJumping)
                {
                    ChangeAnimationState(CharacterAnimationState.Idle);
                }
            }

            PlayFootStepSound(false);
        }

        private void GravityEffect()
        {
            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
            if (_characterController.isGrounded)
            {
                _isJumping = false;
            }
        }


        private void HandleUserInput()
        {

            if (IsDie) return;

            GravityEffect();

            if (!IsPlaying) return;

            // Use GetDirection() so WASD keys work on WebGL in addition to the joystick
            _joystickDirection = _ui.Input.GetDirection();
            if (_joystickDirection.magnitude > 0.1f)
            {
                HandleMoveUpdate();
            }
            else
            {
                HandleIdleUpdate();
            }
        }

        private Vector3 _forwardDirection;
        private float _joystickAngle, _rotationAngle;
        private void HandleMoveUpdate()
        {
            //move
            if (_hunter.IsSilent && _hunter.IsRotateToBot && IsRunning)
            {
                StartCoroutine(DieCoroutine());
                return;
            }
            PlayFootStepSound(true);
            if (!_isJumping) ChangeAnimationState(CharacterAnimationState.Move);

            _joystickAngle = Mathf.Atan2(_joystickDirection.x, _joystickDirection.y) * Mathf.Rad2Deg;
            _forwardDirection = Quaternion.Euler(0, _joystickAngle, 0) * Camera.main.transform.forward;

            _rotationAngle = Quaternion.LookRotation(_forwardDirection).eulerAngles.y;
            transform.eulerAngles = new Vector3(0, _rotationAngle, 0);

            _move = _forwardDirection * _joystickDirection.magnitude;

            //var targetAngle = Mathf.Atan2(_joystickDirection.x, _joystickDirection.y) * Mathf.Rad2Deg;
            //transform.eulerAngles = new Vector3(0, targetAngle, 0);

            //_move = transform.forward * _joystickDirection.magnitude;
            _characterController.Move(_speed * Time.deltaTime * _move);
        }



        public void Jump()
        {
            if (IsDie) return;

            if (_hunter.IsSilent && IsRunning)
            {
                StartCoroutine(DieCoroutine());
                return;
            }
            PlayFootStepSound(false);
            if (_characterController.isGrounded)
            {
                _isJumping = true;
                ChangeAnimationState(CharacterAnimationState.Jump);
                _velocity.y = Mathf.Sqrt(2 * -_gravity * _jumpHeight);
            }
        }

        public void Revive()
        {
            IsDie = false;
            _fxBloodPool.SetActive(false);
            _fxBloodSplat.SetActive(false);
            _hunter.SetActive(true);
            IsPlaying = true;
            ChangeAnimationState(CharacterAnimationState.Idle);
            _baseCharacter.ToggleGreyScale(false);
        }


        private void OnTriggerEnter(Collider other)
        {
            var playerCollect = other.GetComponent<PlayerCollect>();
            if (playerCollect != null)
            {
                switch (playerCollect.TypeItem)
                {
                    case TypeItemCollect.Start:
                        if (IsRunning) return;
                        IsRunning = true;
                        break;
                    case TypeItemCollect.Win:
                        Win();
                        break;
                    case TypeItemCollect.Obstacle:
                        PlayFootStepSound(false);
                        IsDie = true;
                        IsPlaying = false;
                        ChangeAnimationState(CharacterAnimationState.Die);
                        GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
                        _fxBloodSplat.SetActive(true);
                        _hunter.SetActive(false);
                        _baseCharacter.ToggleGreyScale(true);

                        this.InvokeDelay(0.5f, () => { _fxBloodPool.SetActive(true); });
                        this.InvokeDelay(1f, () => GameManager.I.Lose());
                        break;
                    default:
                        break;
                }
            }
        }


        private void Win()
        {
            
            if (!IsPlaying) return;
            PlayFootStepSound(false);
            _hunter.OnWin();
            IsWin = true;
            IsPlaying = false;
            transform.eulerAngles = new Vector3(0, 180, 0);
            ChangeAnimationState(CharacterAnimationState.Dance);
            _controller.PlayCountSound(false);
            _hunter.SetActive(false);
            _winFX.SetActive(true);
            DOVirtual.DelayedCall(0.1f, () => GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE));
            DOVirtual.DelayedCall(3, () => GameManager.I.Win());

        }


        public IEnumerator DieCoroutine()
        {
            if (!IsPlaying) yield break;

            IsPlaying = false;
            IsDie = true;

            PlayFootStepSound(false);
            ChangeAnimationState(CharacterAnimationState.Die);
            _baseCharacter.ToggleGreyScale(true);

            var guard = _hunter.GetFreeEnemy().LookAtTarget(_headPos).GetBaseGuard();
            guard.PlayShootAnim();
            guard.ShowLine(0, _headPos).ClearLine(0.1f);

            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_M4_SHOOT);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);

            _fxBloodSplat.SetActive(true);
            _hunter.SetActive(false);

            this.InvokeDelay(0.5f, () => { _fxBloodPool.SetActive(true); });
            this.InvokeDelay(1f, () => _controller.LoseGame());
        }
    }
}