using NFramework;
using UnityEngine;

namespace Game7
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private Vector3 _hammerOffset;

        [SerializeField] private HammerControl _hammerControl;
        [SerializeField] private LayerMask _layerSelect;

        private bool _isActive;
        private Camera _camera;
        private Game7Control _controller;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Init(Game7Control controller) => _controller = controller;

        public void Active()
        {
            _isActive = true;
        }
        public void DeActive()
        {
            _isActive = false;
        }

        private void Update()
        {
            if (!_isActive) return;

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, _layerSelect))
                {
                    Hole hole = hit.transform.GetComponent<Hole>();
                    if (hole && hole.IsOpen)
                    {
                        _hammerControl.transform.position = hole.transform.position + _hammerOffset;
                        _hammerControl.Hit();
                        hole.Hit();
                        _controller.HitOne();
                        VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
                    }
                }
            }
        }
    }
}