using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame
{
    public class SimpleCameraFollow : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        public float smoothTime = 0.2f; 

        private Vector3 velocity = Vector3.zero;

        void LateUpdate()
        {
            if (target == null) return;

            Vector3 targetPosition = new Vector3(target.position.x + offset.x, transform.position.y, target.position.z + offset.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
