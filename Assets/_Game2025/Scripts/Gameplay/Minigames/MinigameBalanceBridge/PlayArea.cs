using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.BalanceBridge
{
    public class PlayArea : MonoBehaviour
    {
        [SerializeField] private Transform _pointerTransform;
        [SerializeField] private BalanceBridgeManager _manager;

        Vector3 _lookAtPosition = new();
        Transform _cameraTransform;

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            _lookAtPosition.x = _cameraTransform.forward.x;
            _lookAtPosition.z = _cameraTransform.forward.z;
            _lookAtPosition.Normalize();

            _pointerTransform.rotation = Quaternion.LookRotation(-_lookAtPosition);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CameraControl.I.ActivePlayCamera(true);
                _manager.StartCountingToPlay(true, () =>
                {
                    gameObject.SetActive(false);
                });
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CameraControl.I.ActivePlayCamera(false);
                _manager.StartCountingToPlay(false);
            }
        }
    }
}
