using Cinemachine;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Lobby
{
    public class LobbyUI : BaseUIView
    {
        [SerializeField] private VariableJoystick _joystick;
        [SerializeField] private FreeLookController _freeLookController;
        [SerializeField] private Button _jumpButton, _settingsButton;

        [SerializeField] private GameObject _controllerObject, _headerObject, _outroObject;
        [SerializeField] private TMP_Text _outroText, _outroShadowText;

        public VariableJoystick Joystick => _joystick;

        private void OnEnable()
        {
            _joystick.OnPointerUp(null);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _settingsButton.onClick.AddListener(OnSettingButtonClicked);
            GameSound.I.PlayBGM(Define.SoundPath.BGM_LOBBY);
        }

        public override void OnClose()
        {
            base.OnClose();
            _jumpButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveListener(OnSettingButtonClicked);
            GameSound.I.StopBGM();
        }

        public void Init(PlayerController playerController, CinemachineFreeLook cinemachineFreeLook)
        {
            _jumpButton.onClick.AddListener(playerController.Jump);
            playerController.Init(Joystick);
            _freeLookController.Init(cinemachineFreeLook);

            _controllerObject.SetActive(true);
            gameObject.SetActive(false);
            _headerObject.SetActive(false);
            _outroObject.SetActive(false);
        }

        private void OnSettingButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
        }

        public void ShowVictoryScreen()
        {
            gameObject.SetActive(true);
            _controllerObject.SetActive(false);
            _headerObject.SetActive(true);
        }

        public void ShowOutro(int seasonId)
        {
            string outroText = $"You Won Season <color=#F72933>{seasonId}";
            _outroObject.SetActive(true);

            _outroText.text = outroText;
            _outroShadowText.text = outroText;
        }
    }
}
