using System;
using Animancer;
using NFramework;
using Sirenix.OdinInspector;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game4
{
    public class PlayerController : MonoBehaviour
    {
        public event Action OnEndTurn;

        [Header("Config")]
        [SerializeField] private int _totalBall = 5;
        [SerializeField] private float _rangleThrowSuccess;

        [Header("Config Throw Ball")]
        [SerializeField] private float _minThreshold;
        [SerializeField] private BoundaryLimits _bounds;

        [Header("Refs")]
        [SerializeField] private Transform _posCreateBall;
        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private CharacterAnimationController _animator;

        [Header("FX")]
        [SerializeField] private ParticleSystem _bloodFx;

        private bool _isActive;
        private bool _canThrowBall;

        private int _dirControlValueForce;
        private float _forceValue;
        private Ball _currentBall;

        private Vector2 _startEndSuccess;
        private Game4Control _controller;
        private BallInfoPanelUI _ballInfoPanel;
        private GameObject _tutorialPanel;
        private bool _isSwiping;
        private Vector3 _startTouchPosition;
        private Vector3 _endTouchPosition;

        private int _currentBallsCount;
        public bool IsThrewAllBalls => _currentBallsCount == 0;


        public void Init(Game4Control controller, BallInfoPanelUI ballInfoPanelUI, GameObject tutorialPanel)
        {
            _controller = controller;
            _ballInfoPanel = ballInfoPanelUI;
            _tutorialPanel = tutorialPanel;

            _currentBallsCount = 3;
        }

        public void OnStartTurn()
        {
            _isActive = true;
            _tutorialPanel.SetActive(true);
            _animator.PlayAnimation(EAnimStyle.Idle);
            CreateNewBall();
        }

        public void CreateNewBall()
        {
            _currentBall = Instantiate(_ballPrefab, _posCreateBall.position, Quaternion.identity);
            _currentBall.Init(_controller);

            _canThrowBall = true;
            _dirControlValueForce = 1;
            _forceValue = 0f;

            _startEndSuccess.x = Random.Range(0f, 1f - _rangleThrowSuccess);
            _startEndSuccess.y = _startEndSuccess.x + _rangleThrowSuccess;
        }

        private void Update()
        {
            if (!_isActive || !_canThrowBall) return;

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
            _tutorialPanel.SetActive(false);
            _canThrowBall = false;
            _currentBallsCount--;
            _ballInfoPanel.UpdateCurrentBallAmount(_currentBallsCount);

            var direction = Mathf.Clamp(directionX, _bounds.MinX, _bounds.MaxX);
            var distance = Mathf.Clamp(distanceZ, _bounds.MinZ, _bounds.MaxZ);

            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG04_THROW_MARBLE);
            _currentBall?.ThrowBall(new Vector3(direction, 0, distance), this);
            Invoke(nameof(EndTurn), 3f);
        }

        public void Lose()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
            _bloodFx.Play();
            _animator.PlayAnimation(EAnimStyle.Die);
        }

        public void Win() => _animator.PlayAnimation(EAnimStyle.Victory_1, EAnimStyle.Victory_2, EAnimStyle.Victory_3);

        private void EndTurn()
        {
            OnEndTurn?.Invoke();
        }

        public void ResetBall()
        {
            _currentBallsCount = 3;
            _ballInfoPanel.UpdateCurrentBallAmount(_currentBallsCount);
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