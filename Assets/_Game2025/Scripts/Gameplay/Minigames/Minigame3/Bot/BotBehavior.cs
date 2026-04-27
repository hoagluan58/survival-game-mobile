using Redcode.Extensions;
using Sirenix.OdinInspector;
using Spine;
using SquidGame.LandScape.Game;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SquidGame.LandScape.Minigame3
{
    public class BotBehavior : MonoBehaviour
    {
        [SerializeField] private CharacterAnimator _characterAnimator;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private ParticleSystem _fxSand;
        [SerializeField] private Collider _collider;
        [SerializeField] private float _destinationRadius = 0.1f;
        [SerializeField] private float _distanceToDoor = 1.2f;
        private float _searchRadius = 10f;
        private float _randomWalkAroundTime;
        private Timer _walkAroundTimer = new Timer();
        private MinigameController _controller;
        private Vector3 _endPosition;

        public void Init(MinigameController minigameController)
        {
            _controller = minigameController;
        }
        public void StartBot()
        {
            _collider.enabled = true;
            StartCoroutine(CRWalkAround());
        }

        private IEnumerator CRWalkAround()
        {
            _fxSand.Play();
            RandomWalkAroundTime();
            _walkAroundTimer.SetCooldownTime(_randomWalkAroundTime);
            _characterAnimator.PlayAnimation(EAnimStyle.Running);
            while (!_walkAroundTimer.CheckTimer())
            {
                yield return CRWalkAround();
            }
            _agent.isStopped = true;
            StartCoroutine(CRMoveToDoor());

            void RandomWalkAroundTime()
            {
                _randomWalkAroundTime = Random.Range(1f, 3f);
            }

            IEnumerator CRWalkAround()
            {
                NavMeshHit hit = PickRandomPosition();
                _agent.SetDestination(hit.position);                
                yield return new WaitUntil(() =>
                {
                    if(_walkAroundTimer.CheckTimer()) return true;
                    return _agent.remainingDistance < 0.5f;
                });
            }

            NavMeshHit PickRandomPosition()
            {
                Vector3 randomDirection = Random.insideUnitSphere * _searchRadius;
                randomDirection += transform.position;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, _searchRadius, NavMesh.AllAreas))
                {
                    return hit;
                }
                else
                {
                    return PickRandomPosition();
                }
                
            }
            
        }

        private IEnumerator CRMoveToDoor()
        {
            _agent.isStopped = false;
            Door door = _controller.DoorController.GetRandomDoor();
            Vector3 randomDirection = Random.insideUnitSphere * _destinationRadius;
            _endPosition = door.transform.position + randomDirection;
            _agent.SetDestination(_endPosition);
            yield return new WaitUntil(() => Vector3.Distance(transform.position, _endPosition) < _distanceToDoor);
            _agent.isStopped = true;
            _agent.enabled = false;
            _collider.enabled = false;
            _characterAnimator.PlayAnimation(EAnimStyle.Idle);
            _fxSand.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        [Button]
        public void LoadParticleSystem()
        {
            _fxSand = GetComponentInChildren<ParticleSystem>();
        }

        [Button]
        public void LoadCollider()
        {
            _collider = GetComponentInChildren<Collider>();
        }

        [Button]
        public void RandomRotation()
        {
            float yRotation = Random.Range(0f, 360f);
            transform.SetEulerAnglesY(yRotation);
        }
    }
}
