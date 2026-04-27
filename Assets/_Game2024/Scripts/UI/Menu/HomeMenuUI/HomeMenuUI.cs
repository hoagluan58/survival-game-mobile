using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.SaveData;
using SquidGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame
{
    public class HomeMenuUI : BaseUIView
    {
        [SerializeField] private Button _settingsBTN;
        [SerializeField] private Button _trainingBTN;
        [SerializeField] private Button _challengeBTN;
        [SerializeField] private Button _dailyBTN;

        public override void OnOpen()
        {
            base.OnOpen();
            _trainingBTN.onClick.AddListener(OnTrainingButtonClicked);
            _challengeBTN.onClick.AddListener(OnChallengeButtonClicked);
            _settingsBTN.onClick.AddListener(OnSettingsButtonClicked);
            _dailyBTN.onClick.AddListener(OnDailyButtonClicked);
            GameSound.I.PlayBGM(Define.SoundPath.BGM_MAIN);
        }

        public override void OnClose()
        {
            base.OnClose();
            _trainingBTN.onClick.RemoveListener(OnTrainingButtonClicked);
            _challengeBTN.onClick.RemoveListener(OnChallengeButtonClicked);
            _settingsBTN.onClick.RemoveListener(OnSettingsButtonClicked);
            _dailyBTN.onClick.RemoveListener(OnDailyButtonClicked);
        }

        private void OnSettingsButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
        }

        private void OnTrainingButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.TRAINING_MODE_MENU);
        }

        private void OnChallengeButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            GameManager.I.PlayGameMode(EGameMode.Challenge, UserData.I.CurMinigameId);
        }

        private void OnDailyButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.DAILY_REWARD_POPUP);
        }
    }
}
