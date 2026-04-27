using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame3
{
    public class PlayerPanelUI : MonoBehaviour
    {
        public static Action OnJump;
        [SerializeField] private VariableJoystick _joystick;
        [SerializeField] private Game.FreeLookController _freeLookController;
        [SerializeField] private Button _jumpButton;
        [SerializeField] private GameObject _goHeaderNoti;
        public VariableJoystick Joystick => _joystick;

        private void OnEnable()
        {
            _goHeaderNoti.SetActive(true);
            StartCoroutine(CRUnactiveHeaderNoti());
        }

        private IEnumerator CRUnactiveHeaderNoti()
        {
            yield return new WaitForSeconds(3f);
            _goHeaderNoti.SetActive(false);
        }

        public void Init(MinigameController minigameController, PlayerController playerController, CinemachineFreeLook cinemachineFreeLook)
        {
            _jumpButton.onClick.AddListener(() =>
            {
                OnJump?.Invoke();
            });

            playerController.Init(minigameController, Joystick);
            _freeLookController.Init(cinemachineFreeLook);
        }
        private void OnDestroy()
        {
            _jumpButton.onClick.RemoveAllListeners();
        }
    }
}
