using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace SquidGame.Minigame21
{
    public class BotManager : MonoBehaviour
    {
        private const int BOT_AMOUNT = 69;

        [SerializeField] private TextMeshPro _remainingText;
        [SerializeField] private Bot _botPf;
        [SerializeField] private SpriteRenderer _spawnArea;
        [SerializeField] private LayerMask _avoidLayer;
        [SerializeField] private Transform _holder;
        [SerializeField] private Transform _deadBotHolder;

        public List<Bot> FollowerBots => _followerBots;

        private ObjectPool<Bot> _botPool;
        private List<Bot> _allBots;
        private List<Bot> _followerBots;
        private int _botLeft;

        private MinigameController _controller;
        private RoomManager _roomManager;
        private RoundController _roundController;

        public int BotLeft => _botLeft;

        public void Init(MinigameController controller, RoomManager roomManager, RoundController roundController)
        {
            _controller = controller;
            _roomManager = roomManager;
            _roundController = roundController;

            _botLeft = BOT_AMOUNT;
            SetupPool();
            ClearPool();
        }

        public void OnPrepare()
        {
            SpawnRemainingBots();
            SetFollowerBots();
            _remainingText.SetText($"{_allBots.Count + 1}");
        }

        public void OnStart()
        {
            _allBots.ForEach(b => b.OnStart());
        }

        public void OnEndRound()
        {
            var aliveBot = 0;
            _allBots.ForEach(b =>
            {
                b.OnEndRound();
                if (!b.IsDead)
                {
                    aliveBot++;
                    _botPool.Release(b);
                }
                else
                {
                    b.transform.SetParent(_deadBotHolder);
                }
            });
            _allBots.Clear();
            _botLeft = aliveBot;
        }

        private void SpawnRemainingBots()
        {
            _allBots = new List<Bot>();
            var bounds = _spawnArea.bounds;
            var maxCheck = 10;

            for (var i = 0; i < _botLeft; i++)
            {
                var rndPos = GetRandomPositionInsideCollider(bounds);
                var rndRotation = new Vector3(0, Random.Range(0, 360f), 0);
                var limitCount = 0;

                while (Physics.CheckSphere(rndPos, 0.015f, _avoidLayer))
                {
                    rndPos = GetRandomPositionInsideCollider(bounds);
                    limitCount++;
                    if (limitCount == maxCheck) break;
                }

                var bot = _botPool.Get();
                bot.OnSpawn(rndPos, rndRotation);
                _allBots.Add(bot);
            }
        }

        private void SetFollowerBots()
        {
            var followerBotsNeeded = _roundController.GroupRequire - 1; // Minus 1 (player)

            _followerBots = new List<Bot>();
            _followerBots = _allBots.Take(followerBotsNeeded).ToList();

            _allBots.ForEach(x => x.Follower.ToggleFollower(false));

            foreach (var follower in _followerBots)
            {
                follower.Follower.ToggleFollower(true);
            }
        }

        private Vector3 GetRandomPositionInsideCollider(Bounds bounds)
        {
            var randomX = Random.Range(bounds.min.x, bounds.max.x);
            var randomZ = Random.Range(bounds.min.z, bounds.max.z);

            return new Vector3(randomX, 1.1f, randomZ);
        }

        private void SetupPool()
        {
            _allBots = new List<Bot>();
            _botPool = new ObjectPool<Bot>(CreateBot,
                                                  OnGetFromPool,
                                                  OnReleaseToPool,
                                                  OnDestroyPooledObject);

            Bot CreateBot()
            {
                var bot = Instantiate(_botPf, _holder);
                bot.Init(this, _roomManager);
                bot.SetPool(_botPool);
                return bot;
            }

            void OnGetFromPool(Bot pooledObject) => pooledObject.gameObject.SetActive(true);

            void OnReleaseToPool(Bot pooledObject) => pooledObject.gameObject.SetActive(false);

            void OnDestroyPooledObject(Bot pooledObject) => Destroy(pooledObject.gameObject);
        }

        private void ClearPool()
        {
            _allBots.ForEach(b => _botPool.Release(b));
            _allBots.Clear();
        }
    }
}
