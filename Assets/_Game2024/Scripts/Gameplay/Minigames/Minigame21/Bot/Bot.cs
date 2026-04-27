using SquidGame.Gameplay;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

namespace SquidGame.Minigame21
{
    public class Bot : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private BaseCharacter _model;
        [SerializeField] private BotFollower _follower;

        [Header("CONFIG")]
        [SerializeField] private float _stoppingDistance = 0.5f;

        private BotManager _botManager;
        private RoomManager _roomManager;

        private bool _isDead;
        private bool _isInRoom;
        private bool _isActive;

        private Vector3 _roomDestination;
        private RoomController _room;
        private ObjectPool<Bot> _pool;

        public BaseCharacter Model => _model;
        public BotFollower Follower => _follower;
        public NavMeshAgent Agent => _agent;
        public bool IsInRoom => _isInRoom;
        public bool IsDead => _isDead;

        public void Init(BotManager botManager, RoomManager roomManager)
        {
            _botManager = botManager;
            _roomManager = roomManager;
        }

        public void OnSpawn(Vector3 position, Vector3 rotation)
        {
            _isDead = false;
            _isActive = false;
            _isInRoom = false;
            _model.Animator.PlayAnimation(EAnimStyle.Idle);
            _agent.Warp(position);
            transform.eulerAngles = rotation;
            _follower.Init(this);
            _room = null;
        }

        public void OnStart()
        {
            _isActive = true;
            _follower.OnStart();

            if (_follower.IsFollower)
                return;

            AutoNavigation();
        }

        public void OnEndRound()
        {
            _isActive = false;
            if (!_isInRoom)
            {
                _isDead = true;
                _agent.ResetPath();
                _model.Animator.PlayAnimation(EAnimStyle.Die);
            }
        }

        private void AutoNavigation()
        {
            TryGetRoom();
            if (_room == null)
            {
                MoveToRandomPosition();
            }
            else
            {
                MoveToRoom();
            }
        }

        public void SetPool(ObjectPool<Bot> pool) => _pool = pool;

        private void Update()
        {
            if (_isDead) return;
            if (_follower.IsFollower) return;

            _model.Animator.PlayAnimation(_agent.velocity != Vector3.zero ? EAnimStyle.Run : EAnimStyle.Idle);

            if (_isInRoom) return;
            if (!_isActive) return;

            if (_agent.IsAgentReachDestination())
            {
                HandleReachDestination();
            }
        }

        private void TryGetRoom()
        {
            if (_room != null) return;

            _room = _roomManager.GetValidRoom();

            if (_room != null)
            {
                _room.Queue(this);
                _roomDestination = _room.RandomPositionInRoom();
            }
        }

        private void HandleReachDestination()
        {
            if (_room == null)
            {
                MoveToRandomPosition();
            }
        }

        public void AddBotToRoom()
        {
            _isInRoom = true;
        }

        public void ExitRoom()
        {
            _isInRoom = false;
        }

        private void MoveToRandomPosition()
        {
            var rndPosition = GetRandomNavMeshPosition(transform.position, 10f);

            if (rndPosition != null)
            {
                _agent.destination = rndPosition;
            }
        }

        private void MoveToRoom()
        {
            _agent.SetDestination(_roomDestination);
        }

        private Vector3 GetRandomNavMeshPosition(Vector3 origin, float radius)
        {
            // Get a random point within a sphere of the given radius
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += origin;

            // Check if the random point is on the NavMesh
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }

            // If no valid position was found, return Vector3.zero
            return Vector3.zero;
        }
    }
}
