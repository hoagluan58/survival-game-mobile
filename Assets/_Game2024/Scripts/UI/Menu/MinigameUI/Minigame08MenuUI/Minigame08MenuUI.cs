using Game8;
using NFramework;
using SquidGame.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class Minigame08MenuUI : BaseUIView
    {
        [SerializeField] private Button _playBTN;

        [Header("PLAYING")]
        [SerializeField] private GameObject _playingPNL;
        [SerializeField] private GameObject _countdownPNL;
        [SerializeField] private TextMeshProUGUI _countdownTMP;
        [SerializeField] private Image _timerImage;
        [SerializeField] private RectTransform _jumpButtonsPanel;
        [SerializeField] private UIButtonShape[] _shapeButtons;

        private Game8Control _controller;

        public RectTransform JumpButtonPanel => _jumpButtonsPanel;

        public override void OnOpen()
        {
            base.OnOpen();
            _playBTN.onClick.AddListener(OnPlayButtonClicked);
            Game8Control.OnTimeChanged += Game8Control_OnTimeChanged;
        }

        public override void OnClose()
        {
            base.OnClose();
            _playBTN.onClick.RemoveListener(OnPlayButtonClicked);
            Game8Control.OnTimeChanged -= Game8Control_OnTimeChanged;
        }

        private void OnPlayButtonClicked()
        {
            GameManager.I.StartMinigame();
            _playingPNL.SetActive(true);
            _playBTN.gameObject.SetActive(false);
        }

        private void Game8Control_OnTimeChanged(float secondLeft) => UpdateCountdownText(secondLeft);

        public void SetData(Game8Control controller)
        {
            _controller = controller;
            _playBTN.gameObject.SetActive(true);
            _playingPNL.SetActive(false);
            ToggleCountdownPanel(false);
            foreach (var button in _shapeButtons)
            {
                button.Init(controller, this);
            }
        }

        public void ToggleCountdownPanel(bool value) => _countdownPNL.SetActive(value);

        private void UpdateCountdownText(float secondLeft)
        {
            _timerImage.fillAmount = secondLeft / 2f;
            _countdownTMP.text = $"{secondLeft:N0}";
        }
    }
}