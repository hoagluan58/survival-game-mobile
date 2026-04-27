using DG.Tweening;
using NFramework;
using UnityEngine;
namespace SquidGame.LandScape.Minigame1
{
    public class RandomRunState : BaseStateBot
    {
        private Vector2 _rangeX;
        private Vector2 _rangeZ;

        private Vector3 _currentTarget;
        [SerializeField] private float _speed;
        private Tween _delayTween;
        private void MoveToTarget()
        {
            _currentTarget = new Vector3(
                Random.Range(_rangeX.x, _rangeX.y),
                transform.position.y,
                Random.Range(_rangeZ.x, _rangeZ.y)
            );
            transform.LookAt( _currentTarget );
            transform.DOMove(_currentTarget, _speed).SetEase(Ease.Linear).SetSpeedBased(true).OnComplete(PrepareToMove);
            _bot.Animator.PlayAnimation(Game.EAnimStyle.Running);
        }


        private void PrepareToMove()
        {
            _bot.Animator.PlayAnimation(Game.EAnimStyle.Idle);
            var delayTime = Random.Range(1f, 5f);
            _delayTween = DOVirtual.DelayedCall(delayTime, MoveToTarget);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _rangeX = _bot.IsWin ? _bot.Manager.Enviroment.GetRangeXEndgame() : _bot.Manager.Enviroment.GetRangeXStartgame();
            _rangeZ = _bot.IsWin ? _bot.Manager.Enviroment.GetRangeZEndgame() : _bot.Manager.Enviroment.GetRangeZStartgame();
            PrepareToMove();
        }


        public override void OnExit()
        {
            _delayTween.Kill();
            transform.DOKill();
            transform.LookAt(Vector3.forward);
        }

    }

}