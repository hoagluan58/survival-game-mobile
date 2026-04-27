using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.Minigame01
{
    public class KeepDistanceBackground : MonoBehaviour
    {
        [SerializeField] private Transform _objectToKeepDistance;

        private float _desiredDistanceZ;

        private void Awake() => _desiredDistanceZ = transform.position.z - _objectToKeepDistance.position.z;

        private void Update()
        {
            if (_objectToKeepDistance != null)
            {
                var currentPosition = transform.position;
                transform.position = new Vector3(
                    currentPosition.x,
                    currentPosition.y,
                    _objectToKeepDistance.position.z + _desiredDistanceZ
                );
            }
        }
    }
}
