using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class LookAtCamera : MonoBehaviour
    {

        [SerializeField] private bool _isUpdate = true;
        private Camera _camera;
        void Start()
        {
            _camera = Camera.main; 
        }

        private void LateUpdate()
        {
            if (_isUpdate)
            {
                transform.LookAt(_camera.transform);
            }
        }
    }
}
