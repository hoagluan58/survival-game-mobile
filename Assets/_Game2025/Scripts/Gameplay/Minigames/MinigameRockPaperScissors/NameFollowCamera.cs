using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class NameFollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;

        void LateUpdate()
        {
            transform.LookAt(_cameraTransform);
        }
    }
}
