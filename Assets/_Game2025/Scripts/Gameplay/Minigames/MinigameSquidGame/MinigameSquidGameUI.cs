using System;
using NFramework;
using SquidGame.LandScape.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.SquidGame
{
    public class MinigameSquidGameUI : BaseUIView
    {
        [SerializeField] private GameObject _notiTimerPanel, _playTimerPanel, _inputObject;
        [SerializeField] private Button _jumpButton, _attackButton, _settingButton;
        [SerializeField] private TMP_Text _countText, _timeText, _levelText;
        [SerializeField] private UITouchPanel _uiTouchPanel;
        [SerializeField] private VariableJoystick _joystick;

        void OnEnable()
        {
            _joystick.OnPointerUp(null);
        }

        void SetDefault()
        {
            _notiTimerPanel.SetActive(true);
            _jumpButton.gameObject.SetActive(true);
            _settingButton.gameObject.SetActive(true);
            _inputObject.SetActive(false);
            _playTimerPanel.SetActive(false);
            _attackButton.gameObject.SetActive(false);
        }

        public void Init(PlayerController playerController, CinemachineFreeLookInput cinemachineFreeLookInput, int currentLevel)
        {
            _levelText.text = $"Level {currentLevel}";

            cinemachineFreeLookInput.InitTouchPanel(_uiTouchPanel);
            playerController.InitJoystick(_joystick);

            _jumpButton.onClick.RemoveAllListeners();
            _attackButton.onClick.RemoveAllListeners();
            _settingButton.onClick.RemoveAllListeners();

            _jumpButton.onClick.AddListener(playerController.Jump);
            _attackButton.onClick.AddListener(playerController.Attack);
            _settingButton.onClick.AddListener(OnSettingButtonClicked);

            SetDefault();
        }

        public void OnPlayerEquipWeapon()
        {
            _attackButton.gameObject.SetActive(true);
            _jumpButton.gameObject.SetActive(false);
            _playTimerPanel.gameObject.SetActive(false);
        }

        public void UpdateCountText(string value)
        {
            _countText.text = $"{value}";
        }

        public void UpdateTimerText(string value)
        {
            _timeText.text = $"{value}";
        }

        public void ActiveNotiTimeCount(bool value)
        {
            _playTimerPanel.SetActive(value);
        }

        public void StartMinigame()
        {
            _notiTimerPanel.SetActive(false);
            _inputObject.SetActive(true);
            _playTimerPanel.SetActive(true);
        }

        private void OnSettingButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
        }

        // public void OnPlayerDead()
        // {
        //     _settingButton.gameObject.SetActive(false);
        //     _inputObject.SetActive(false);
        // }

        // public void OnPlayerRevive()
        // {
        //     _settingButton.gameObject.SetActive(true);
        //     _inputObject.SetActive(true);
        // }
    }
}
