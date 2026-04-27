using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame16
{
    public class Minigame16MenuUI : BaseUIView
    {
        [SerializeField] private Button _playBTN;

        [Header("PLAYING")]
        [SerializeField] private GameObject _playingPNL;
        [SerializeField] private GameObject _focusTMP;
        [SerializeField] private GameObject _repeatItTMP;
        [SerializeField] private LightPanelUI _lightPanelUI;
        [SerializeField] private Button _settingButton;
        [SerializeField] private TextMeshProUGUI _levelText;

        public LightPanelUI LightPanelUI => _lightPanelUI;
        private MinigameController _controller;

        public override void OnOpen()
        {
            base.OnOpen();
            _playBTN.onClick.AddListener(OnPlayButtonClicked);
            _settingButton.onClick.AddListener(OnSettingButtonClicked);
            
        }


        public override void OnClose()
        {
            base.OnClose();
            _playBTN.onClick.RemoveListener(OnPlayButtonClicked);
            _settingButton.onClick.RemoveListener(OnSettingButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            _playingPNL.SetActive(true);
            _playBTN.gameObject.SetActive(false);
            _lightPanelUI.gameObject.SetActive(false);
            ShowFocusText(true);
            ShowRepeatItText(false);
            GameManager.I.StartMinigame();
        }


        private void OnSettingButtonClicked()
        {
            UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
            GameSound.I.PlaySFXButtonClick();
        }


        public void SetData(MinigameController controller)
        {
            _controller = controller;
            _playBTN.gameObject.SetActive(true);
            _playingPNL.SetActive(false);
            _levelText.SetText($"Level {_controller.Level + 1}");
        }

        public void ShowLightPanelUI(bool value) => _lightPanelUI.gameObject.SetActive(value);

        public void ShowFocusText(bool value) => _focusTMP.gameObject.SetActive(value);

        public void ShowRepeatItText(bool value) => _repeatItTMP.gameObject.SetActive(value);
    }
}
