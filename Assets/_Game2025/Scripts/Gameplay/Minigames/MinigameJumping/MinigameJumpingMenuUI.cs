using System;
using System.Collections;
using System.Collections.Generic;
using NFramework;
using SquidGame.LandScape.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Jumping
{
    public class MinigameJumpingMenuUI : BaseUIView
    {
        [SerializeField] private GameObject _notiTimerPanel, _playTimerPanel, _inputObject;
        [SerializeField] private Button _jumpButton, _settingButton;
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
            _inputObject.SetActive(true);
            _playTimerPanel.SetActive(false);
        }

        public void Init(PlayerController playerController, CinemachineFreeLookInput cinemachineFreeLookInput, int currentLevel)
        {
            _jumpButton.onClick.RemoveAllListeners();
            _settingButton.onClick.RemoveAllListeners();

            _levelText.text = $"Level {currentLevel}";
            cinemachineFreeLookInput.InitTouchPanel(_uiTouchPanel);
            playerController.InitJoystick(_joystick);
            _jumpButton.onClick.AddListener(playerController.Jump);
            _settingButton.onClick.AddListener(OnSettingButtonClicked);
            SetDefault();
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
            _playTimerPanel.SetActive(true);
        }

        private void OnSettingButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
        }
    }
}
