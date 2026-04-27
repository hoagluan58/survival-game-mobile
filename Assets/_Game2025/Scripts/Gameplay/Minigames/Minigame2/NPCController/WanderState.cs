using SquidGame.LandScape.Game;
using UnityEngine;
using UnityEngine.AI;

namespace SquidGame.LandScape.Minigame2
{
    public class WanderState : INPCState
    {
        private GlassBridgeNPC _npc;
        private NavMeshAgent _agent;
        private CharacterAnimator _animator;

        private Vector3 _targetPosition;
        private float _wanderRadius = 5f;
        private float _minWaitTime = 1f;
        private float _maxWaitTime = 3f;
        private float _waitTime;

        public WanderState(GlassBridgeNPC npc)
        {
            _npc = npc;
        }

        public INPCState.EState StateName => INPCState.EState.Wander;

        public void OnInit()
        {
            _animator = _npc.Model.GetCom<CharacterAnimator>();
            _agent = _npc.Agent;
        }

        public void OnEnter() => SetNewDestination();

        public void OnExit() => _agent.ResetPath();

        public void OnUpdate()
        {
            if (!_agent.isOnNavMesh)
                return;

            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                _waitTime -= Time.deltaTime;
                if (_waitTime <= 0)
                {
                    SetNewDestination();
                }
                _animator.PlayAnimation(EAnimStyle.Idle);
            }
            else
            {
                _animator.PlayAnimation(EAnimStyle.Running);
            }
        }

        private void SetNewDestination()
        {
            _waitTime = Random.Range(_minWaitTime, _maxWaitTime);

            var randomDirection = Random.insideUnitSphere * _wanderRadius;
            randomDirection += _npc.transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _wanderRadius, NavMesh.AllAreas))
            {
                _targetPosition = hit.position;
                _agent.SetDestination(_targetPosition);
            }
        }
    }
}
