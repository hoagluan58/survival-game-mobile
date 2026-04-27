using Cinemachine;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame2
{
    public class MinigameUI : BaseUIView
    {
        [SerializeField] private TextMeshProUGUI _levelTMP;
        [SerializeField] private Button _settingBTN;
        [SerializeField] private GameObject _timerPNL;
        [SerializeField] private TextMeshProUGUI _timeTMP;
        [SerializeField] private MinigameTutorialPanelUI _tutorialPanel;

        [Header("PLAYER INPUT UI")]
        [SerializeField] private VariableJoystick _joystick;
        [SerializeField] private FreeLookController _freeLookController;
        [SerializeField] private Button _jumpButton;

        public VariableJoystick Joystick => _joystick;

        public override void OnOpen()
        {
            base.OnOpen();
            _settingBTN.onClick.AddListener(OnSettingButtonClicked);
            _joystick.ResetJoystick();
        }

        public override void OnClose()
        {
            base.OnClose();
            _settingBTN.onClick.RemoveListener(OnSettingButtonClicked);
            _joystick.ResetJoystick();
        }

        private void OnSettingButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
        }

        public void Init(PlayerController playerController, CinemachineFreeLook cinemachineFreeLook, MinigameController controller)
        {
            _jumpButton.onClick.AddListener(playerController.Jump);

            playerController.Init(_joystick, controller);
            _freeLookController.Init(cinemachineFreeLook);
            SetActiveTimerPanel(false);
        }

        public void UpdateLevelText(int level) => _levelTMP.SetText($"Level {level}");

        public void UpdateTutorialText(string content) => _tutorialPanel.UpdateText(content);

        public void SetActiveTutorialPanel(bool value)
        {
            if (value) _tutorialPanel.Show();
            else _tutorialPanel.Hide();
        }

        public void UpdateTimeText(float time) => _timeTMP.SetText(time.ToString());

        public void SetActiveTimerPanel(bool value) => _timerPNL.SetActive(value);
    }
}
