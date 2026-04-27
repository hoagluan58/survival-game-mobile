using Game11;
using NFramework;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class Minigame11MenuUI : BaseUIView
    {
        [SerializeField] private Button _playBTN;

        [Header("PLAYING")]
        [SerializeField] private GameObject _playingPNL;
        [SerializeField] private GameObject _warningPNL;
        [SerializeField] private TextMeshProUGUI _timeTMP;

        [Header("BOOSTER")]
        [SerializeField] private Button _boosterBTN;
        [SerializeField] private TextMeshProUGUI _boosterDescriptionTMP;

        private Game11Control _controller;

        public override void OnOpen()
        {
            base.OnOpen();
            _playBTN.onClick.AddListener(OnPlayButtonClicked);
            Game11Control.OnShowWarning += Game11Control_OnShowWarning;
            Game11Control.OnTimeChanged += Game11Control_OnTimeChanged;
            _boosterBTN.onClick.AddListener(OnBoosterButtonClicked);
        }

        public override void OnClose()
        {
            base.OnClose();
            _playBTN.onClick.RemoveListener(OnPlayButtonClicked);
            Game11Control.OnShowWarning += Game11Control_OnShowWarning;
            Game11Control.OnTimeChanged += Game11Control_OnTimeChanged;
            _boosterBTN.onClick.RemoveListener(OnBoosterButtonClicked);
        }

        private void Game11Control_OnShowWarning(bool value) => SetActiveWarning(value);

        private void Game11Control_OnTimeChanged(string value) => UpdateTimeText(value);

        private void OnPlayButtonClicked()
        {
            GameManager.I.StartMinigame();
            _playingPNL.SetActive(true);
            _playBTN.gameObject.SetActive(false);
        }

        private void OnBoosterButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            _boosterBTN.gameObject.SetActive(false);
            GameManager.I.UseBooster();
        }

        public void SetData(Game11Control controller)
        {
            _controller = controller;
            _playBTN.gameObject.SetActive(true);
            _playingPNL.SetActive(false);
            SetActiveWarning(false);
            InitBoosterButton();
        }

        private void SetActiveWarning(bool value) => _warningPNL.SetActive(value);

        private void UpdateTimeText(string str) => _timeTMP.text = str;

        private void InitBoosterButton()
        {
            _boosterBTN.gameObject.SetActive(true);
            _boosterDescriptionTMP.text = $"+{_controller.BoosterTimeAdded}s";
        }
    }
}
