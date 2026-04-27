using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Data;
using SquidGame.LandScape.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.UI
{
    public class WinMinigamePopupUI : BaseUIView
    {
        [SerializeField] private Transform _panel;

        [SerializeField] private Button _homeBTN;
        [SerializeField] private Button _nextBTN;
        [SerializeField] private Button _claimAdsBTN;

        [SerializeField] private TextMeshProUGUI _amountTMP;
        [SerializeField] private MultiplierBarUI _barUI;
        [SerializeField] private CurrencyBarUI _currencyBarUI;

        private UserData _userData;

        public override void OnOpen()
        {
            base.OnOpen();
            RegisterButtons();
            SetData();
            PlayAnimation();
        }

        private void PlayAnimation()
        {
            _panel.DOKill();
            _panel.localScale = Vector3.zero;
            _panel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        private void RegisterButtons()
        {
            _homeBTN.onClick.AddListener(OnHomeButtonClicked);
            _nextBTN.onClick.AddListener(OnNextButtonClicked);
            _claimAdsBTN.onClick.AddListener(OnClaimAdsButtonClicked);
        }

        private void UnregisterButtons()
        {
            _homeBTN.onClick.RemoveListener(OnHomeButtonClicked);
            _nextBTN.onClick.RemoveListener(OnNextButtonClicked);
            _claimAdsBTN.onClick.RemoveListener(OnClaimAdsButtonClicked);
        }

        public override void OnClose()
        {
            base.OnClose();
            UnregisterButtons();
        }

        private void OnNextButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UnregisterButtons();
            _currencyBarUI.PlayUpdateCoin(Define.WIN_MINIGAME_COIN, 1f, () =>
            {
                CloseSelf();
                GameManager.I.ReloadMinigame();
            });
        }

        private void OnHomeButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UnregisterButtons();
            _currencyBarUI.PlayUpdateCoin(Define.WIN_MINIGAME_COIN, 1f, () =>
            {
                CloseSelf();
                GameManager.I.Exit();
            });
        }

        private void OnClaimAdsButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            // Reward video removed — multiplier is applied for free locally
            UnregisterButtons();
            _barUI.StopArrow();
            _currencyBarUI.PlayUpdateCoin(Define.WIN_MINIGAME_COIN * _barUI.CurMultiply, 1f, () =>
            {
                CloseSelf();
                GameManager.I.ReloadMinigame();
            });
        }

        private void SetData()
        {
            _userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
            _barUI.SetData(() => _amountTMP.SetText($"{Define.WIN_MINIGAME_COIN * _barUI.CurMultiply}"));
            _barUI.StartArrow();
        }
    }
}
