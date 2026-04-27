using Game2;
using NFramework;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class Minigame02MenuUI : BaseUIView
    {
        [SerializeField] private Button _playBTN;

        [Header("PLAYING")]
        [SerializeField] private GameObject _playingPNL;
        [SerializeField] private TextMeshProUGUI _countdownTMP;

        [Header("BOOSTER")]
        [SerializeField] private Button _boosterBTN;
        [SerializeField] private TextMeshProUGUI _boosterDescriptionTMP;

        private Game2Control _controller;

        public override void OnOpen()
        {
            base.OnOpen();
            _playBTN.onClick.AddListener(OnPlayButtonClicked);
            _boosterBTN.onClick.AddListener(OnBoosterButtonClicked);
            Game2Control.OnTimerChanged += OnTimerChanged;
        }

        public override void OnClose()
        {
            base.OnClose();
            _playBTN.onClick.RemoveListener(OnPlayButtonClicked);
            _boosterBTN.onClick.RemoveListener(OnBoosterButtonClicked);
            Game2Control.OnTimerChanged -= OnTimerChanged;
        }

        private void OnPlayButtonClicked()
        {
            GameManager.I.StartMinigame();
            _playingPNL.SetActive(true);
            _playBTN.gameObject.SetActive(false);
        }

        private void OnTimerChanged(int time) => UpdateCountdownText(time);

        public void SetData(Game2Control controller)
        {
            _controller = controller;
            _playBTN.gameObject.SetActive(true);
            _playingPNL.SetActive(false);
            _boosterBTN.gameObject.SetActive(true);
            _boosterDescriptionTMP.text = $"+{_controller.BoosterTimeAdded}s";
        }

        private void UpdateCountdownText(int time) => _countdownTMP.text = $"{time}";

        private void OnBoosterButtonClicked()
        {
            _boosterBTN.gameObject.SetActive(false);
            GameSound.I.PlaySFXButtonClick();
            GameManager.I.UseBooster();
        }
    }
}