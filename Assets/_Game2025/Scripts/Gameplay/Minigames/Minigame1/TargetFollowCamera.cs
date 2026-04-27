using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class TargetFollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform _target3D;
        [SerializeField] private float _range;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            if (_target3D == null || _camera == null) return;

            Vector3 cameraPos = _camera.transform.position;
            Vector3 targetPos = _target3D.position;

            transform.position = Vector3.Lerp(cameraPos, targetPos, _range);

            transform.LookAt(Camera.main.transform);
        }
    }
}
