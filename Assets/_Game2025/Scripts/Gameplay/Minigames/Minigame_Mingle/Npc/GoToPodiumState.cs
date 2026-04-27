
using UnityEngine;
using UnityEngine.AI;

namespace SquidGame.LandScape.MinigameMingle
{
    public class GoToPodiumState : NpcStateMachine
    {
        private bool _isMoving = false;
        private bool _isCompleted;
        private float _remaningDistance => Mathf.Abs(Vector3.Distance(Transform.position, NpcBase.NavMeshAgent.destination));

        public override void Enter()
        {
            base.Enter();
            NpcBase.NavMeshAgent.enabled = true;
            _isCompleted = false;
            React();
        }


        private void React()
        {
            NpcBase.SetEnableOutline(false);
            Move();
            //var type = NpcBase.RingAreaSpawner.GetRandomPointPodium();// .GetAreaType(Transform.position);
            //switch (type)
            //{
            //    case AreaType.Podium:
            //        StandStill();
            //        break;
            //    case AreaType.Ring:
                    
            //        break;
            //}
        }


        protected override void StandStill()
        {
            _isCompleted = true;
            NpcBase.NavMeshAgent.enabled = false;
            NpcBase.DustFx.SetActive(false);
            base.StandStill();
        }


        public override bool IsCompleted()
        {
            return _isCompleted;
        }


        public override void Exit()
        {
            base.Exit();
            NpcBase.NavMeshAgent.enabled = true;
            NpcBase.DustFx.SetActive(true);
        }

        protected override void Move()
        {
            var target = NpcBase.RingAreaSpawner.GetRandomPositionPodium();
            target = new Vector3(UnityEngine.Random.Range(-1, 1) + target.x, target.y, Random.Range(-1, 1) + target.z);
            if (NavMesh.SamplePosition(target, out var _hit, 22f, NavMesh.AllAreas))
            {
                NpcBase.NavMeshAgent.SetDestination(_hit.position);
                _isMoving = true;
                base.Move();
            }
            else
            {
                Move();
            }
        }


        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            if (_isMoving && _remaningDistance <= 0.05f)
            {
                _isMoving = false;
                StandStill();
                return;
            }
        }


        

    }
}
