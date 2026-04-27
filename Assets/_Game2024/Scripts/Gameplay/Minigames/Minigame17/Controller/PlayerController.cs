using NFramework;
using Redcode.Extensions;
using System.Collections;
using UnityEngine;

namespace SquidGame.Minigame17
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;

        private BoxController _boxController;
        private bool _isUpdate;
        private Camera _camera;

        public void Init(BoxController boxController)
        {
            _camera = Camera.main;
            _boxController = boxController;
            EnableInput(false);
        }

        public void EnableInput(bool value) => _isUpdate = value;

        private void Update()
        {
            if (!_isUpdate) return;

            HandleMouseInput();
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask))
                {
                    if (hit.collider.TryGetComponentInParent<Box>(out var newBox))
                    {
                        if (newBox.IsFullyOpen) return;

                        StartCoroutine(CROpenBox());

                        IEnumerator CROpenBox()
                        {
                            _isUpdate = false;
                            VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
                            yield return _boxController.CRHandleBoxClicked(newBox);
                            _isUpdate = true;
                        }
                    }
                }
            }


        }
    }
}
