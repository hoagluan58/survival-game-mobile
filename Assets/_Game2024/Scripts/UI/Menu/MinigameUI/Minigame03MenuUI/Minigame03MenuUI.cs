using NFramework;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.Minigame03;
using SquidGame.Minigame03.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class Minigame03MenuUI : BaseUIView
    {
        [SerializeField] private Button _playBTN;

        [Header("PLAYING")]
        [SerializeField] private GameObject _playingPNL;
        [SerializeField] private TextMeshProUGUI _countdownTMP;
        [SerializeField] private GameObject _countdownPNL;
        [SerializeField] private GameObject _warningPNL;
        [SerializeField] private GameObject _chooseCaseTutorialTMP;
        [SerializeField] private BreakDalgonaPanelUI _breakDalgonaPNL;
        [SerializeField] private TextMeshProUGUI _holdToMoveTutorialTMP;

        [Header("BOOSTER")]
        [SerializeField] private Button _boosterBTN;
        [SerializeField] private TextMeshProUGUI _boosterDescriptionTMP;

        private MinigameController _controller;
        public BreakDalgonaPanelUI BreakDalgonaPNL => _breakDalgonaPNL;

        public override void OnOpen()
        {
            base.OnOpen();
            _playBTN.onClick.AddListener(OnPlayButtonClicked);
            MinigameController.OnTimeChanged += Game3Control_OnTimeChanged;
            MinigameController.OnShowTutorial += Game3Control_OnShowTutorial;
            MinigameController.OnShowWarning += Game3Control_OnShowWarning;
            MinigameController.OnShowCountdownPanel += Game3Control_ShowCountdownPanel;
            MinigameController.OnShowTutorialHoldToMove += Game3Control_OnShowTutorialHoldToMove;
            _boosterBTN.onClick.AddListener(OnBoosterButtonClicked);
        }

        public override void OnClose()
        {
            base.OnClose();
            _playBTN.onClick.RemoveListener(OnPlayButtonClicked);
            MinigameController.OnShowTutorial -= Game3Control_OnShowTutorial;
            MinigameController.OnShowWarning -= Game3Control_OnShowWarning;
            MinigameController.OnShowCountdownPanel -= Game3Control_ShowCountdownPanel;
            MinigameController.OnTimeChanged -= Game3Control_OnTimeChanged;
            MinigameController.OnShowTutorialHoldToMove -= Game3Control_OnShowTutorialHoldToMove;
            _boosterBTN.onClick.RemoveListener(OnBoosterButtonClicked);
        }

        private void OnBoosterButtonClicked()
        {
            _boosterBTN.gameObject.SetActive(false);
            GameSound.I.PlaySFXButtonClick();
            GameManager.I.UseBooster();
        }

        private void OnPlayButtonClicked()
        {
            GameManager.I.StartMinigame();
            _playingPNL.SetActive(true);
            _playBTN.gameObject.SetActive(false);
        }

        private void Game3Control_OnShowTutorial(bool value) => ShowTutorial(value);

        private void Game3Control_OnShowWarning(bool value) => ShowWarning(value);

        private void Game3Control_ShowCountdownPanel(bool value) => ShowCountdownPanel(value);

        private void Game3Control_OnTimeChanged(int value) => UpdateCountdownText(value);

        public void SetData(MinigameController controller)
        {
            _controller = controller;
            _playBTN.gameObject.SetActive(true);
            _playingPNL.SetActive(false);
            _holdToMoveTutorialTMP.gameObject.SetActive(false);
            _breakDalgonaPNL.SetActive(false);
            ShowCountdownPanel(false);
            ShowTutorial(false);
            ShowWarning(false);
            _boosterBTN.gameObject.SetActive(true);
            _boosterDescriptionTMP.text = $"+{_controller.BoosterTimeAdded}s";
        }

        private void UpdateCountdownText(int value) => _countdownTMP.text = value.ToString();

        private void ShowCountdownPanel(bool value) => _countdownPNL.SetActive(value);

        private void ShowTutorial(bool value) => _chooseCaseTutorialTMP.SetActive(value);

        private void ShowWarning(bool value) => _warningPNL.SetActive(value);

        private void Game3Control_OnShowTutorialHoldToMove(bool isShow)
        {
            _holdToMoveTutorialTMP.gameObject.SetActive(isShow);
        }
    }
}
