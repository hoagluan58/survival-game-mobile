using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace SquidGame.Minigame01
{
    public class HunterLaserEyes : MonoBehaviour
    {
        [SerializeField] private Transform _eyePos;
        [SerializeField] private LaserLine _linePf;
        [SerializeField] private Transform _lineHolder;

        private ObjectPool<LaserLine> _linePool;
        private List<LaserLine> _lineList;

        private void Awake() => SetupPool();

        private void SetupPool()
        {
            _lineList = new List<LaserLine>();
            _linePool = new ObjectPool<LaserLine>(CreateLaserLine,
                                                  OnGetFromPool,
                                                  OnReleaseToPool,
                                                  OnDestroyPooledObject);

            LaserLine CreateLaserLine()
            {
                var laserLine = Instantiate(_linePf, _lineHolder);
                laserLine.SetPool(_linePool);
                return laserLine;
            }

            void OnGetFromPool(LaserLine pooledObject) => pooledObject.gameObject.SetActive(true);

            void OnReleaseToPool(LaserLine pooledObject) => pooledObject.gameObject.SetActive(false);

            void OnDestroyPooledObject(LaserLine pooledObject) => Destroy(pooledObject.gameObject);
        }

        public void Shoot(Transform target)
        {
            var line = _linePool.Get();
            var points = new Transform[2] { _eyePos, target };
            line.DrawLine(points);
            _lineList.Add(line);
        }

        public void DisableAllLaser()
        {
            _lineList.ForEach(line => _linePool.Release(line));
            _lineList.Clear();
        }
    }
}
