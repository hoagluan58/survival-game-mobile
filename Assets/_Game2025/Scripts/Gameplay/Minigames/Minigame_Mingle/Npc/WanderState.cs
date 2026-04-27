using DG.Tweening;
using Spine;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace SquidGame.LandScape.MinigameMingle
{
    public class WanderState : NpcStateMachine
    {
        private NavMeshHit _hit;
        private bool _isMoving;
        private float _chillTime;
        private float _remaningDistance => Mathf.Abs(Vector3.Distance(Transform.position, NpcBase.NavMeshAgent.destination));

        public override NpcStateMachine Init(NpcBase npc)
        {
            return base.Init(npc);
        }


        public override void Enter()
        {
            Move();
        }


        protected override void StandStill()
        {
            base.StandStill();
            _chillTime = Random.Range(3f, 5f);
            _isMoving = false;
        }


        protected override void Move()
        {
            var target = NpcBase.RingAreaSpawner.GetRandomPointArea();
            if (NavMesh.SamplePosition(target, out _hit, 22f, NavMesh.AllAreas))
            {
                _isMoving = true;
                NpcBase.NavMeshAgent.SetDestination(_hit.position);
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

            if (_isMoving && _remaningDistance <= 0.1f)
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
