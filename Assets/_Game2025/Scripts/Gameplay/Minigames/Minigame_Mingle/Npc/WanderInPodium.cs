using UnityEngine;
using UnityEngine.AI;

namespace SquidGame.LandScape.MinigameMingle
{
    public class WanderInPodium : NpcStateMachine
    {
        private bool _isMoving;
        private float _chillTime;


        public override void Enter()
        {
            StandStill();
        }


        protected override void StandStill()
        {
            base.StandStill();
            _chillTime = UnityEngine.Random.Range(5f, 15f);
            _isMoving = false;
        }


        public override bool IsCompleted()
        {
            return true; 
        }


        protected override void Move()
        {
            var target = NpcBase.RingAreaSpawner.GetRandomPointPodium();
            if (NavMesh.SamplePosition(target, out var hit, 22f, NavMesh.AllAreas))
            {
                _isMoving = true;
                NpcBase.NavMeshAgent.SetDestination(hit.position);
                base.Move();
            }
        }


        public override void Exit()
        {
            base.Exit();
        }


        public override void OnUpdate()
        {
            base.OnUpdate();
            _chillTime -= Time.deltaTime;
            if (_chillTime >= 0) return;

            if (_isMoving && NpcBase.NavMeshAgent.remainingDistance <= 0.1f)
            {
                StandStill();
                return;
            }

            if (!_isMoving)
            {
                Move();
            }
        }
    }
}
