using NFramework;
using SquidGame.LandScape;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class NoInternetPopupUI : BaseUIView
    {
        [SerializeField] private RectTransform _contentPanel;
        [SerializeField] private Button _okButton;

        public override void OnOpen()
        {
            base.OnOpen();
            _contentPanel.DOPunchScalePopup();
            _okButton.onClick.AddListener(OnOkButtonClicked);
        }

        public override void OnClose()
        {
            base.OnClose();
            _okButton.onClick.RemoveListener(OnOkButtonClicked);
        }

        private void OnOkButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            DeviceInfo.OpenDeviceWifiSetting();
        }

        private void Update() => TryClosePopup();

        private void TryClosePopup()
        {
            var hasInternet = DeviceInfo.HasInternet();
            if (hasInternet)
            {
                CloseSelf();
            }
        }
    }
}
