using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Game;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.UI
{
    public class LoseChallengePopupUI : BaseUIView
    {
        [SerializeField] private Button _homeBTN;
        [SerializeField] private Button _reviveBTN;
        [SerializeField] private Button _noThanksBTN;
        [SerializeField] private Button _replayButton;

        [SerializeField] private GameObject _revivePopupGo;
        [SerializeField] private GameObject _replayPopupGo;

        private Tween _tweener;

        public override void OnOpen()
        {
            base.OnOpen();
            _homeBTN.onClick.AddListener(OnHomeButtonClicked);
            _reviveBTN.onClick.AddListener(OnReviveButtonClicked);
            _noThanksBTN.onClick.AddListener(OnNoThanksButtonClicked);
            _replayButton.onClick.AddListener(OnReplayButtonClicked);
            ShowRevivePopup(true);
            _noThanksBTN.gameObject.SetActive(false);
            _tweener = DOVirtual.DelayedCall(3, () => _noThanksBTN.gameObject.SetActive(true));
        }

        public override void OnClose()
        {
            base.OnClose();
            _homeBTN.onClick.RemoveListener(OnHomeButtonClicked);
            _reviveBTN.onClick.RemoveListener(OnReviveButtonClicked);
            _noThanksBTN.onClick.RemoveListener(OnNoThanksButtonClicked);
            _replayButton.onClick.RemoveListener(OnReplayButtonClicked);
            _tweener.Kill();
        }

        private void OnHomeButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            CloseSelf();
            GameManager.I.Exit();
        }

        private void OnReviveButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            // Reward video removed — revive is granted for free locally
            CloseSelf();
            GameManager.I.Revive();
        }

        private void OnNoThanksButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            CloseSelf();
            GameManager.I.ReloadMinigame();
        }

        private void OnReplayButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            CloseSelf();
            GameManager.I.ReturnToLobby();
        }

        public void ShowRevivePopup(bool value)
        {
            _replayPopupGo.SetActive(!value);
            _revivePopupGo.SetActive(value);
        }
    }
}
