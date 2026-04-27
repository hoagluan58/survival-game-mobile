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


    }
}
