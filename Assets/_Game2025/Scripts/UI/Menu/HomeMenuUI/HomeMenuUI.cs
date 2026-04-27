using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Data;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.UI
{

    public class HomeMenuUI : BaseUIView
    {
        [SerializeField] private Button _shopBTN;
        [SerializeField] private Button _minigamesBTN;
        [SerializeField] private Button _challengeBTN;
        [SerializeField] private Button _settingsBTN;
        [SerializeField] private CurrencyBarUI _currencyBarUI;


        public override void OnOpen()
        {
            base.OnOpen();
            _shopBTN.onClick.AddListener(OnShopButtonClicked);
            _minigamesBTN.onClick.AddListener(OnMinigamesButtonClicked);
            _challengeBTN.onClick.AddListener(OnChallengeButtonClicked);
            _settingsBTN.onClick.AddListener(OnSettingsButtonClicked);
            CheckShowRateUsPopup();
        }

        public override void OnClose()
        {
            base.OnClose();
            _shopBTN.onClick.RemoveListener(OnShopButtonClicked);
            _minigamesBTN.onClick.RemoveListener(OnMinigamesButtonClicked);
            _challengeBTN.onClick.RemoveListener(OnChallengeButtonClicked);
            _settingsBTN.onClick.RemoveListener(OnSettingsButtonClicked);
        }

        private void OnShopButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.SHOP_MENU);
        }

        private void OnMinigamesButtonClicked()
        {
            CloseSelf();
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.MINIGAMES_MENU);
        }

        private void OnChallengeButtonClicked()
        {
            CloseSelf();
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.CHALLENGE_MENU);
        }

        private void OnSettingsButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open<SettingsPopupUI>(Define.UIName.SETTINGS_POPUP).SetData(SettingsPopupUI.EStyle.Default);
        }

        private void CheckShowRateUsPopup()
        {
            var userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
            if (userData.IsShowRateUsPopup) return;

            var isPlayedEnough = userData.PlayedMinigames.Count >= 1;
            if (isPlayedEnough)
            {
                UIManager.I.Open(Define.UIName.RATE_US_POPUP);
            }
        }
    }
}
