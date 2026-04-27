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

        /// <summary>
        /// Returns the effective move direction for Editor and WebGL (WASD / arrows),
        /// falling back to the on-screen joystick on mobile.
        /// </summary>
        public Vector2 GetDirection()
        {
#if UNITY_EDITOR || UNITY_WEBGL
            float h = 0f, v = 0f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))  h -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) h += 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))  v -= 1f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))    v += 1f;
            var wasd = new Vector2(h, v).normalized;
            if (wasd.magnitude > 0.01f)
                return wasd;
#endif
            return _joystick.Direction;
        }

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
            playerController.Init(this);   // pass LobbyUI so controller uses GetDirection()
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
