using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class CharacterName : MonoBehaviour
    {
        private Transform _cameraTransform;
        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        void LateUpdate()
        {
            transform.LookAt(_cameraTransform);
        }
    }
}
