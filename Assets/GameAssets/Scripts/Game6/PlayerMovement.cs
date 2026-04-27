using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game6
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speedMoveZ;
        [SerializeField] private float _speedMoveX;
        [SerializeField] private Vector2 _clampX;
        [SerializeField] private Rigidbody _rb;

        private Vector2 _posTouch;
        private Vector2 _deltaTouch;
        private VariableJoystick _joystick;

        private bool _isActive;

        public void Init(VariableJoystick joystick) => _joystick = joystick;

        public void SetActive(bool b)
        {
            _isActive = b;
        }

        private void Update()
        {
            if (!_isActive) return;
            ReadInput();
            HandleMovement();
        }

        private void ReadInput()
        {
            if (TouchUtility.TouchCount >= 1)
            {
                Touch touch = TouchUtility.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    _posTouch = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    _deltaTouch = touch.position - _posTouch;
                    _posTouch = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    _deltaTouch = Vector2.zero;
                }
            }
        }

        private void HandleMovement()
        {
            Vector3 wantedPos = transform.position;
            wantedPos += Vector3.forward * _speedMoveZ * Time.deltaTime;
            wantedPos += _joystick.Horizontal * Time.deltaTime * Vector3.right * _speedMoveX;
            wantedPos.x = Mathf.Clamp(wantedPos.x, _clampX.x, _clampX.y);
            transform.position = wantedPos;
        }
    }
}
