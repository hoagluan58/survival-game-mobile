using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System;
using UnityEngine;

namespace SquidGame.LandScape.Minigame4
{
    public class PlayerController : BaseMarblesController
    {
        public event Action OnStartThrowMarble;

        [Header("Config Throw Ball")]
        [SerializeField] private float _minThreshold;
        [SerializeField] private BoundaryLimits _bounds;

        [Header("Refs")]
        [SerializeField] private Transform _posCreateBall;
        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private BaseCharacter _baseCharacter;
        [SerializeField] private ParticleSystem _confettiFx; 

        [SerializeField] private float _forceMultiple = 1.2f;

        private Ball _currentBall;
        private bool _canThrowBall;
        private bool _isSwiping;
        private Vector3 _startTouchPosition;
        private Vector3 _endTouchPosition;

        public void OnInitialized()
        {
            _baseCharacter.ToggleGreyScale(false);
            PlayAnim(EAnimStyle.Idle);
        }


        public void PlayAnim(params EAnimStyle[] style)
        {
            _baseCharacter.GetCom<CharacterAnimator>().PlayAnimation(style);
        }


        public PlayerController SetActiveModel(bool value)
        {
            _baseCharacter.gameObject.SetActive(value);
            return this;
        }

        public PlayerController RotateCharacter(Vector3 euler)
        {
            _baseCharacter.transform.eulerAngles = euler;
            return this; 
        }


        public override void StartTurn()
        {
            base.StartTurn();
            CreateNewBall(); 
        }


        private void PlayFx()
        {
            _confettiFx.Play();
            GameSound.I.PlaySFX(Define.SoundPath.SFX_CONGRA);
        }


        public void CreateNewBall()
        {
            _currentBall = Instantiate(_ballPrefab, _posCreateBall.position, Quaternion.identity, _posCreateBall);
            _currentBall.OnEnterHoldAction += PlayFx;
            _currentBall.Initialized();
            
            _canThrowBall = true;
        }

        private void Update()
        {
            if (!_canThrowBall) return;

            if (Input.GetMouseButtonDown(0))
            {
                _isSwiping = true;
                _startTouchPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0) && _isSwiping && _currentBall != null)
            {
                _isSwiping = false;
                _endTouchPosition = Input.mousePosition;

                var swipe = _endTouchPosition - _startTouchPosition;

                if (swipe.y >= _minThreshold)
                {
                    ThrowBall(swipe.x, swipe.y);
                }
            }
        }

        private void ThrowBall(float directionX, float distanceZ)
        {
            OnStartThrowMarble?.Invoke();

            _canThrowBall = false;
            _marblesCount = Mathf.Max(0, _marblesCount - 1);

            var direction = Mathf.Clamp(directionX, _bounds.MinX, _bounds.MaxX);
            var distance = Mathf.Clamp(distanceZ, _bounds.MinZ, _bounds.MaxZ);

            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG04_THROW_MARBLE);
            
            _currentBall?.ThrowBall(new Vector3(direction, 0, distance)* _forceMultiple).OnCompleted(OnCompletedThrowball);
        }


        private void OnCompletedThrowball(bool isInHole)
        {
            _isInHole = isInHole;
            if (_isInHole) ScoredMarble(); 
            EndTurn();
        }


        public override void Dance()
        {
            base.Dance();
            PlayAnim(EAnimStyle.Victory_1, EAnimStyle.Victory_2, EAnimStyle.Victory_3);
        }


        public override void Dead()
        {
            base.Dead();
            _baseCharacter.ToggleGreyScale(true);
            PlayAnim(EAnimStyle.Die);
        }


        [Serializable]
        public class BoundaryLimits
        {
            public float MinX;
            public float MaxX;
            public float MinZ;
            public float MaxZ;
        }
    }
}
