using DG.Tweening;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.Minigame17.UI
{
    public class Minigame17MenuUI : MonoBehaviour
    {
        [SerializeField] private Button _playBTN;

        [Header("PLAYING PANEL")]
        [SerializeField] private GameObject _playingPNL;
        [SerializeField] private GameObject _timerPNL;
        [SerializeField] private TextMeshProUGUI _tutorialTMP;
        [SerializeField] private TextMeshProUGUI _showAllCountdownTMP;
        [SerializeField] private TextMeshProUGUI _countdownTMP;

        [Header("BOOSTER")]
        [SerializeField] private Button _boosterBTN;
        [SerializeField] private TextMeshProUGUI _boosterDescriptionTMP;

        private MinigameController _controller;

        private void OnEnable()
        {
            _playBTN.onClick.AddListener(OnPlayButtonClicked);
            _boosterBTN.onClick.AddListener(OnBoosterButtonClicked);
        }

        private void OnDisable()
        {
            _playBTN.onClick.RemoveListener(OnPlayButtonClicked);
            _boosterBTN.onClick.RemoveListener(OnBoosterButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            _playBTN.gameObject.SetActive(false);
            _playingPNL.SetActive(true);
            GameManager.I.StartMinigame();
        }

        private void OnBoosterButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            _boosterBTN.gameObject.SetActive(false);
            GameManager.I.UseBooster();
        }

        public void Init(MinigameController controller)
        {
            _controller = controller;
            _playBTN.gameObject.SetActive(true);
            _playingPNL.SetActive(false);
            ShowTutorialText(false);
            ShowCountdownText(0, false);
            ShowAllCountdown(0, false);
            InitBoosterButton();
        }

        public void SetActive(bool value) => gameObject.SetActive(value);

        public void ShowTutorialText(bool value) => _tutorialTMP.gameObject.SetActive(value);

        public void ShowAllCountdown(int duration, bool value)
        {
            _timerPNL.SetActive(value);
            _showAllCountdownTMP.gameObject.SetActive(value);
            if (value)
            {
                _showAllCountdownTMP?.DOKill();
                UpdateText($"{duration}");
                DOTween.To(() => duration, x =>
                {
                    UpdateText($"{(int)x}");
                }, 0f, duration).SetEase(Ease.Linear);
            }

            void UpdateText(string str) => _showAllCountdownTMP.text = str;
        }

        public void ShowCountdownText(int duration, bool value)
        {
            _timerPNL.SetActive(value);
            _countdownTMP.gameObject.SetActive(value);
            if (value)
            {
                _countdownTMP?.DOKill();
                UpdateText($"{duration}");
                DOTween.To(() => duration, x =>
                {
                    UpdateText($"{(int)x}");
                }, 0f, duration).SetEase(Ease.Linear);
            }

            void UpdateText(string str) => _countdownTMP.text = str;
        }

        public void ShowBoosterButton(bool value) => _boosterBTN.gameObject.SetActive(value);

        private void InitBoosterButton() => _boosterDescriptionTMP.text = $"+{_controller.BoosterTimeAdded}s";
    }
}
