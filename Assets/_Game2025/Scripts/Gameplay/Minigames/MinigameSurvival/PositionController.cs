using System;
using System.Collections.Generic;
using NFramework;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace SquidGame.LandScape
{
    public class PositionController : SingletonMono<PositionController>
    {
        [SerializeField] private Transform _tfMinMap;
        [SerializeField] private Transform _tfMaxMap;
        [SerializeField] private float _minimunDistanceBetweenWeapon = 5f;
        [SerializeField] private float _searchRadius = 10f;

        public Vector3 GetRandomPosition(LayerMask obstacleMask, List<SavePosition> savePosList)
        {
            int maxAttempts = 100;
            for (int i = 0; i < maxAttempts; i++)
            {
                float x = Random.Range(_tfMinMap.position.x, _tfMaxMap.position.x);
                float z = Random.Range(_tfMinMap.position.z, _tfMaxMap.position.z);
                Vector3 randomPos = new Vector3(x, 0, z);

                if (!IsValidPosition(randomPos)) continue;
                if (IsWeaponLyingNearOtherObject(randomPos, savePosList)) continue;
                if (IsTouchingObstacle(randomPos, obstacleMask)) continue;
                return randomPos;
            }

            return Vector3.zero;

            bool IsValidPosition(Vector3 center)
            {
                if (NavMesh.SamplePosition(center, out _, _searchRadius, NavMesh.AllAreas))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            bool IsWeaponLyingNearOtherObject(Vector3 targetPos, List<SavePosition> savePosList)
            {
                foreach (var position in savePosList)
                {
                    if (IsNearOtherWeapon(targetPos, position.Position))
                    {
                        return true;
                    }
                }
                return false;
            }

            bool IsNearOtherWeapon(Vector3 targetPos, Vector3 comparePos)
            {
                return Vector3.Distance(targetPos, comparePos) < _minimunDistanceBetweenWeapon;
            }

            bool IsTouchingObstacle(Vector3 targetPos, LayerMask obstacleMask)
            {
                return Physics.CheckSphere(targetPos, 0.5f, obstacleMask);
            }
        }
    }

    [Serializable]
    public class SavePosition
    {
        public Vector3 Position;
        public bool IsRemoved;

        public SavePosition(Vector3 position)
        {
            Position = position;
        }
    }
}
