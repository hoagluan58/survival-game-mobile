using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame1
{
    public class InputCharacterBase : MonoBehaviour
    {

        [SerializeField] private LandScape.Game.FreeLookController _freeLookController;
        [SerializeField] private VariableJoystick _joystick;
        [SerializeField] private Button _jumpButton;


        private void OnDisable()
        {
            _joystick.ResetJoystick();
        }

        private void OnEnable()
        {
            _joystick.ResetJoystick();
        }

        public InputCharacterBase SetJumpAction(UnityAction unityAction)
        {
            _jumpButton.onClick.AddListener(unityAction);
            return this;
        }


        public InputCharacterBase InitializeFreeLookController(CinemachineFreeLook cinemachineFreeLook)
        {
            _freeLookController.Init(cinemachineFreeLook);
            return this;
        }


        public VariableJoystick GetJoystick()
        {
            return _joystick;
        }

        /// <summary>
        /// Returns the effective move direction.
        /// On WebGL / desktop WASD keys are used when the joystick is idle.
        /// Arrow keys are also supported.
        /// </summary>
        public Vector2 GetDirection()
        {
#if UNITY_EDITOR || UNITY_WEBGL
            var wasd = GetWASDInput();
            if (wasd.magnitude > 0.01f)
                return wasd;
#endif
            return _joystick.Direction;
        }

        /// <summary>Returns a normalised WASD / arrow-key direction.</summary>
        private static Vector2 GetWASDInput()
        {
            float h = 0f, v = 0f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))  h -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) h += 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))  v -= 1f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))    v += 1f;
            return new Vector2(h, v).normalized;
        }
    }
}
