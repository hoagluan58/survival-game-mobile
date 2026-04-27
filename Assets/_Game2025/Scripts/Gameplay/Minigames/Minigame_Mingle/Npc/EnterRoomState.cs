using UnityEngine;
using UnityEngine.AI;

namespace SquidGame.LandScape.MinigameMingle
{
    public class EnterRoomState : NpcStateMachine
    {
        private float _remaningDistance => Mathf.Abs(Vector3.Distance(Transform.position, NpcBase.NavMeshAgent.destination));
        private bool _isMoving = false;
        public override void Enter()
        {
            base.Enter();
            Move();
        }


        protected override void StandStill()
        {
            base.StandStill();
            NpcBase.SwitchState(NpcState.WanderInRoom);
        }


        protected override void Move()
        {
            var room = NpcBase.Room;
            if (NavMesh.SamplePosition(room.GetRandomPoint(), out var hit, 22f, NavMesh.AllAreas))
            {
                _isMoving = true;
                NpcBase.NavMeshAgent.SetDestination(hit.position);
                base.Move();
            }
            else
            {
                Move();
            }
        }


        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_isMoving && _remaningDistance <= 0.05f)
            {
                _isMoving = false;
                StandStill();
                return;
            }
        }


    }
}
