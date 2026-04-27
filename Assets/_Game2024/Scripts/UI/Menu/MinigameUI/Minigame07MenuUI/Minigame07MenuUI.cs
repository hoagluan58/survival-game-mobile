using Game7;
using NFramework;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class Minigame07MenuUI : BaseUIView
    {
        [SerializeField] private Button _playBTN;

        [Header("PLAYING")]
        [SerializeField] private GameObject _playingPNL;
        [SerializeField] private Image _fillIMG;
        [SerializeField] private TextMeshProUGUI _progressTMP;
        [SerializeField] private TextMeshProUGUI _timeTMP;
        [SerializeField] private GameObject _tutorialPNL;

        [Header("BOOSTER")]
        [SerializeField] private Button _boosterBTN;
        [SerializeField] private TextMeshProUGUI _boosterDescriptionTMP;

        private Game7Control _controller;

        public override void OnOpen()
        {
            base.OnOpen();
            _playBTN.onClick.AddListener(OnPlayButtonClicked);
            Game7Control.OnProgressChanged += Game7Control_OnProgressChanged;
            Game7Control.OnTimeChanged += Game7Control_OnTimeChanged;
            _boosterBTN.onClick.AddListener(OnBoosterButtonClicked);
        }

        public override void OnClose()
        {
            base.OnClose();
            _playBTN.onClick.RemoveListener(OnPlayButtonClicked);
            Game7Control.OnProgressChanged -= Game7Control_OnProgressChanged;
            Game7Control.OnTimeChanged -= Game7Control_OnTimeChanged;
            _boosterBTN.onClick.RemoveListener(OnBoosterButtonClicked);
        }

        private void OnBoosterButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            _boosterBTN.gameObject.SetActive(false);
            GameManager.I.UseBooster();
        }

        private void Game7Control_OnTimeChanged(string value) => UpdateTimeText(value);

        private void Game7Control_OnProgressChanged(int current, int total) => UpdateProgress(current, total);

        private void OnPlayButtonClicked()
        {
            GameManager.I.StartMinigame();
            _playingPNL.SetActive(true);
            _playBTN.gameObject.SetActive(false);
        }

        public void SetData(Game7Control controller)
        {
            _controller = controller;
            _playBTN.gameObject.SetActive(true);
            _playingPNL.SetActive(false);
            InitBoosterButton();
            ToggleTutorial(true);
        }

        private void UpdateProgress(int current, int total)
        {
            _fillIMG.fillAmount = (float)current / total;
            _progressTMP.text = $"{current}/{total}";
        }

        private void UpdateTimeText(string str) => _timeTMP.text = str;

        private void InitBoosterButton()
        {
            _boosterBTN.gameObject.SetActive(true);
            _boosterDescriptionTMP.text = $"+{_controller.BoosterTimeAdded}s";
        }

        public void ToggleTutorial(bool value) => _tutorialPNL.SetActive(value);
    }
}
