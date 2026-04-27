using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.Minigame21.UI
{
    public class Minigame21MenuUI : BaseUIView
    {
        public const string CHECK_TUTORIAL = "TUTORIAL_MINIGAME_21";

        [SerializeField] private Button _playBTN;

        [Header("PLAYING")]
        [SerializeField] private GameObject _playingPNL;
        [SerializeField] private GameObject _timerPNL;
        [SerializeField] private TextMeshProUGUI _timerTMP;
        [SerializeField] private TextMeshProUGUI _groupTMP;
        [SerializeField] private TextMeshProUGUI _roundTMP;
        [SerializeField] private VariableJoystick _joystick;

        [Header("TUTORIAL")]
        [SerializeField] private Minigame21TutorialPanelUI _tutorialPNL;

        private MinigameController _controller;

        public VariableJoystick Joystick => _joystick;

        public override void OnOpen()
        {
            base.OnOpen();
            _playBTN.onClick.AddListener(OnPlayButtonClicked);
        }

        public override void OnClose()
        {
            base.OnClose();
            _playBTN.onClick.RemoveListener(OnPlayButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            _playBTN.gameObject.SetActive(false);
            GameManager.I.StartMinigame();
        }

        public void SetData(MinigameController controller)
        {
            _controller = controller;
            CheckShowTutorial();
        }

        public void OnPrepare()
        {
            _playingPNL.SetActive(false);
            _playBTN.gameObject.SetActive(true);
        }

        public void OnPlaying(int groupCount, int round)
        {
            _playingPNL.SetActive(true);
            SetGroupText(groupCount);
            SetRoundText(round);
        }

        public void SetCountdownText(int time) => _timerTMP.SetText($"{time}");

        public void SetGroupText(int group) => _groupTMP.SetText(GameLocalization.I.GetStringFromTable($"STRING_GROUP_OF", group));

        public void SetRoundText(int round) => _roundTMP.SetText(GameLocalization.I.GetStringFromTable("STRING_ROUND", round));

        private void CheckShowTutorial()
        {
            var isTrainingMode = GameManager.I.CurModeHandler.GameMode == EGameMode.Training;
            var isFirstTimePlayed = PlayerPrefs.GetInt(CHECK_TUTORIAL) == 0;

            _tutorialPNL.gameObject.SetActive(isTrainingMode && isFirstTimePlayed);
        }
    }
}
