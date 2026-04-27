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
            _lookInput = _touchInput.PlayerJoystickOutputVector();
            if (axisName == _touchXMapTo)
                return _lookInput.x * _touchSpeedSensitivityX;

            if (axisName == _touchYMapTo)
                return _lookInput.y * _touchSpeedSensitivityY;

            return Input.GetAxis(axisName);
        }
    }
}
