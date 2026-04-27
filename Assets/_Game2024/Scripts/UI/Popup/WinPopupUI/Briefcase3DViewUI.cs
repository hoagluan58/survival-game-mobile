using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace SquidGame.UI
{
    public class Briefcase3DViewUI : MonoBehaviour
    {
        [SerializeField] private GameObject _cashItemPf;
        [SerializeField] private Transform _spawnPos;
        [SerializeField] private bool _isSpawning;
        [SerializeField] private float _delayBetweenSpawn = 0.1f;

        private int _spawnAmount;
        private List<GameObject> _cashItems = new List<GameObject>();
        private ObjectPool<GameObject> _poolCashItems;
        private float _delay = 0;

        private void Awake()
        {
            _poolCashItems = new(
               () => Instantiate(_cashItemPf, _spawnPos.transform),
               item => item.gameObject.SetActive(true),
               item => item.gameObject.SetActive(false),
               item => Destroy(item.gameObject));
        }

        [Button]
        public void Init(int spawnAmount)
        {
            _spawnAmount = spawnAmount;
            _isSpawning = true;
            ReturnToPool();
        }

        private void Update()
        {
            if (_delay > 0) _delay -= Time.deltaTime;

            if (!_isSpawning) return;
            if (_delay > 0) return;

            SpawnCashItem();
        }

        private void ReturnToPool()
        {
            foreach (var item in _cashItems)
            {
                _poolCashItems.Release(item);
            }
            _cashItems.Clear();
        }

        private void SpawnCashItem()
        {
            if (_cashItems.Count >= _spawnAmount)
            {
                _isSpawning = false;
                return;
            }

            _isSpawning = true;

            var obj = _poolCashItems.Get();
            obj.transform.localPosition = Vector3.zero;
            obj.transform.rotation = Random.rotation;
            _cashItems.Add(obj);
            _delay = _delayBetweenSpawn;
        }
    }
}
