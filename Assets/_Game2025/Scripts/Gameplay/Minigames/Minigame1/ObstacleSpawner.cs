using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame1
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _minPosition;
        [SerializeField] private Transform _maxPosition;

        [SerializeField] private GameObject _obstaclePrefab;


        public void OnInit() { }

        public void SpawnObstacle(int value)
        {
            for (int i = 0; i < value; i++)
            {
                var pos = new Vector3(
                    Random.Range(_minPosition.position.x,_maxPosition.position.x),
                    0,
                    Random.Range(_minPosition.position.z, _maxPosition.position.z)
                );

                Instantiate(_obstaclePrefab,pos, Quaternion.identity,transform);
            }
        }
    }
}
