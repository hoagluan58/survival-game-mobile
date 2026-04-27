using System;
using Animancer;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game4
{
    public class BotController : MonoBehaviour
    {
        public event Action OnEndTurn;

        [Header("BALL")]
        [SerializeField] private EnemyBall _enemyBallPrefab;
        [SerializeField] private Transform _ballSpawnPoint;

        [Header("ANIMATION")]
        [SerializeField] private CharacterAnimationController _animator;
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private ClipTransition _throwClip;
        [SerializeField] private ClipTransition _idleClip;
        [SerializeField] private ClipTransition _winClip;

        [Header("FX")]
        [SerializeField] private ParticleSystem _bloodFx;


        private int _currentBallCount;
        private Game4Control _game4Controller;
        private BallInfoPanelUI _ballInfoPanel;

        public bool IsThrewAllBalls => _currentBallCount == 0;


        public void Init(Game4Control controller, BallInfoPanelUI ballInfoPanelUI)
        {
            _animancer.Play(_idleClip);
            _game4Controller = controller;
            _ballInfoPanel = ballInfoPanelUI;
            _currentBallCount = 3;
        }

        public void OnStartTurn()
        {
            // Play animation
            var throwAnimState = _animancer.Play(_throwClip);
            throwAnimState.Events.OnEnd += () => _animancer.Play(_idleClip);

            this.InvokeDelay(0.75f, ThrowBall);
        }

        private void ThrowBall()
        {
            // Wait 
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG04_THROW_MARBLE);
            var newBall = Instantiate(_enemyBallPrefab, _ballSpawnPoint);
            newBall.Init(_game4Controller);
            var randomForce = Vector3.zero;
            randomForce.x = 2.5f;
            randomForce.z = Random.Range(-5f, -25f);
            randomForce.y = 5f;
            newBall.AddThrowForce(randomForce);

            _currentBallCount--;
            _ballInfoPanel.UpdateCurrentBallAmount(_currentBallCount);
            this.InvokeDelay(3.5f, EndTurn);
        }

        private void EndTurn()
        {
            _animancer.Play(_idleClip);
            OnEndTurn?.Invoke();
        }

        public void ResetBall()
        {
            _currentBallCount = 3;
            _ballInfoPanel.UpdateCurrentBallAmount(_currentBallCount);
        }

        public void PerformDie()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
            _bloodFx.Play();
            _animator.PlayAnimation(EAnimStyle.Die);
        }

        public void PerformVictory()
        {
            _animancer.Play(_winClip);
        }
    }
}