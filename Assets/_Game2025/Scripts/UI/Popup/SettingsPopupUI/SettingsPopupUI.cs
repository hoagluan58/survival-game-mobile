using NFramework;
using SquidGame.LandScape.Game;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.UI
{
    public class SettingsPopupUI : BaseUIView
    {
        public static event Action<bool> OnPopupVisible;

        [SerializeField] private Button _closeBTN;

        [Header("DEFAULT")]
        [SerializeField] private RectTransform _rectDefaultPopup;

        [Header("HOME")]
        [SerializeField] private RectTransform _rectHomePopup;
        [SerializeField] private Button _homeBTN;

        private EStyle _style;

        public override void OnOpen()
        {
            base.OnOpen();
            _closeBTN.onClick.AddListener(OnCloseButtonClicked);
            _homeBTN.onClick.AddListener(OnHomeButtonClicked);
            Time.timeScale = 0f;
            SetData();
            PunchScalePopup();
            OnPopupVisible?.Invoke(true);
        }

        public override void OnClose()
        {
            base.OnClose();
            _closeBTN.onClick.RemoveListener(OnCloseButtonClicked);
            _homeBTN.onClick.RemoveListener(OnHomeButtonClicked);
            Time.timeScale = 1f;
            OnPopupVisible?.Invoke(false);
        }

        private void OnCloseButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            CloseSelf();
        }

        private void OnHomeButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            GameManager.I.Exit();
            CloseSelf();
        }

        public void SetData(EStyle style = EStyle.Home)
        {
            _style = style;
            _rectDefaultPopup.gameObject.SetActive(_style == EStyle.Default);
            _rectHomePopup.gameObject.SetActive(_style == EStyle.Home);
        }

        private void PunchScalePopup()
        {
            switch (_style)
            {
                case EStyle.Default:
                    _rectDefaultPopup.DOPunchScalePopup();
                    break;
                case EStyle.Home:
                    _rectHomePopup.DOPunchScalePopup();
                    break;
            }
        }

        public enum EStyle
        {
            Default,
            Home,
        }
    }
}
