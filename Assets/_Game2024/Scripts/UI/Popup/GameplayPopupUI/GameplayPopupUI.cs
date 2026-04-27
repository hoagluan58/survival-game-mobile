using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class GameplayPopupUI : BaseUIView
    {
        [SerializeField] private Button _homeBTN;
        [SerializeField] private Button _backBTN;

        [SerializeField] private Button _settingsBTN;
        [SerializeField] private GameObject _dayTrackPNL;

        public override void OnOpen()
        {
            base.OnOpen();
            _homeBTN.onClick.AddListener(OnHomeButtonClicked);
            _backBTN.onClick.AddListener(OnBackButtonClicked);
            _settingsBTN.onClick.AddListener(OnSettingsButtonClicked);

            SetData();
        }

        public override void OnClose()
        {
            base.OnClose();
            _homeBTN.onClick.RemoveListener(OnHomeButtonClicked);
            _backBTN.onClick.RemoveListener(OnBackButtonClicked);
            _settingsBTN.onClick.RemoveListener(OnSettingsButtonClicked);
        }

        private void OnHomeButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();

            var popup = UIManager.I.Open<WarningPopupUI>(Define.UIName.WARNING_POPUP);
            popup.SetData(new WarningPopupUI.Model()
            {
                OnNoClicked = OnNoClicked,
            });

            void OnNoClicked()
            {
                GameManager.I.Exit();
                this.InvokeDelay(TransitionUI.DELAY_TIME, () => CloseSelf());
            }
        }

        private void OnBackButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            GameManager.I.Exit();
            this.InvokeDelay(TransitionUI.DELAY_TIME, () => CloseSelf());
        }

        private void OnSettingsButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
        }

        public void SetData()
        {
            var gameMode = GameManager.I.CurModeHandler.GameMode;
            _backBTN.gameObject.SetActive(gameMode == EGameMode.Training);
            _homeBTN.gameObject.SetActive(gameMode == EGameMode.Challenge);
        }
    }
}