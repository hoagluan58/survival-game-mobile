using Animancer;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System;
using UnityEngine;

namespace SquidGame.LandScape.Minigame4
{
    public class EnemyController : BaseMarblesController
    {

        [Header("BALL")]
        [SerializeField] private EnemyBall _enemyBallPrefab;
        [SerializeField] private Transform _ballSpawnPoint;

        [Header("ANIMATION")]
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private BaseCharacter _baseCharacter;
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private AnimationClip _throwClip;
        [Header("FX")]
        [SerializeField] private ParticleSystem _bloodFx;


        private LevelContent _levelContent;
        public void OnInitialized(LevelContent content)
        {
            _levelContent = content;
            _animator.PlayAnimation(Game.EAnimStyle.Idle);
        }


        public override void StartTurn()
        {
            base.StartTurn();
            var throwAnimState = _animancer.Play(_throwClip);
            throwAnimState.Events.OnEnd += () => _animator.PlayAnimation(Game.EAnimStyle.Idle);

            this.InvokeDelay(0.75f, ThrowBall);
        }

 
        private void ThrowBall()
        {
            // Wait 
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG04_THROW_MARBLE);
            var newBall = Instantiate(_enemyBallPrefab, _ballSpawnPoint);
            newBall.Initialize();

            var rate = _levelContent.SuccessRate;
            var isHit = UnityEngine.Random.Range(0f, 1f) <= rate;
            var randomForce = GetForce(isHit);
            newBall.AddThrowForce(randomForce).OnCompleted(OnThrowMarbleCompleted);
            _marblesCount = Math.Max(0, _marblesCount - 1);
        }

        private Vector3 GetForce(bool isHit)
        {
            return new Vector3
            {
                x = isHit ? UnityEngine.Random.Range(-0.5f, 2) : 2.5f,
                y = isHit ? 0 : 5f,
                z = isHit ? UnityEngine.Random.Range(-14.75f , -13.5f) : UnityEngine.Random.Range(-5f, -25f)
            };
        }


        private void OnThrowMarbleCompleted(bool isSuccess)
        {
            _isInHole = isSuccess;
            if (isSuccess)
            {
                ScoredMarble();
            }
            this.InvokeDelay(0.5f, EndTurn);
        }


        public override void EndTurn()
        {
            _animator.PlayAnimation(Game.EAnimStyle.Idle);
            base.EndTurn();
        }


        public override void Dance()
        {
            base.Dance();
            _animator.PlayAnimation(Game.EAnimStyle.Victory_1, Game.EAnimStyle.Victory_2, Game.EAnimStyle.Victory_3);
        }


        override public void Dead()
        {
            base.Dead();
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
            _bloodFx.Play();
            _baseCharacter.ToggleGreyScale(true);
            _animator.PlayAnimation(Game.EAnimStyle.Die);
        }
    }
}
