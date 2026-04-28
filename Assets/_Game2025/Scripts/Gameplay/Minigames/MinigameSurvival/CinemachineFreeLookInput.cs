using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class CinemachineFreeLookInput : MonoBehaviour
    {
        private UITouchPanel _touchInput;
        [SerializeField] private float _touchSpeedSensitivityX = 1f;
        [SerializeField] private float _touchSpeedSensitivityY = 1f;

        // WebGL / desktop: hold Right Mouse Button and drag to look around
        [SerializeField] private float _mouseSpeedSensitivityX = 3f;
        [SerializeField] private float _mouseSpeedSensitivityY = 1f;

        private Vector2 _lookInput;
        private string _touchXMapTo = "Mouse X";
        private string _touchYMapTo = "Mouse Y";

        private void OnEnable()
        {
            CinemachineCore.GetInputAxis = GetInputAxis;
        }

        void OnDisable()
        {
            CinemachineCore.GetInputAxis = Input.GetAxis;
        }

        public void InitTouchPanel(UITouchPanel uiTouchPanel)
        {
            _touchInput = uiTouchPanel;
        }

        private float GetInputAxis(string axisName)
        {
            // --- Touch-panel path (mobile / on-screen drag) ---
            if (_touchInput != null)
            {
                _lookInput = _touchInput.PlayerJoystickOutputVector();
                if (_lookInput.magnitude > 0.01f)
                {
                    if (axisName == _touchXMapTo) return _lookInput.x * _touchSpeedSensitivityX;
                    if (axisName == _touchYMapTo) return _lookInput.y * _touchSpeedSensitivityY;
                }
            }

#if !UNITY_ANDROID && !UNITY_IOS
            // --- Mouse path (WebGL / desktop) ---
            if (Input.GetMouseButton(1)) // right-mouse-button held
            {
                if (axisName == _touchXMapTo) return Input.GetAxis("Mouse X") * _mouseSpeedSensitivityX;
                if (axisName == _touchYMapTo) return Input.GetAxis("Mouse Y") * _mouseSpeedSensitivityY;
            }
#endif

            return Input.GetAxis(axisName);
        }
    }
}
