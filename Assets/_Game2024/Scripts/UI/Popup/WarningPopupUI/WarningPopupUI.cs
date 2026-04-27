using NFramework;
using SquidGame.LandScape;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class WarningPopupUI : BaseUIView
    {
        [SerializeField] private RectTransform _popupRt;
        [SerializeField] private Button _noBTN;
        [SerializeField] private Button _yesBTN;

        private Model _model;

        public void SetData(Model model) => _model = model;

        public override void OnOpen()
        {
            base.OnOpen();
            _noBTN.onClick.AddListener(OnNoButtonClicked);
            _yesBTN.onClick.AddListener(OnYesButtonClicked);
            _popupRt.DOPunchScalePopup();
        }

        public override void OnClose()
        {
            base.OnClose();
            _noBTN.onClick.RemoveListener(OnNoButtonClicked);
            _yesBTN.onClick.RemoveListener(OnYesButtonClicked);
        }

        private void OnYesButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            _model.OnYesClicked?.Invoke();
            CloseSelf();
        }

        private void OnNoButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            _model.OnNoClicked?.Invoke();
            CloseSelf();
        }

        public class Model
        {
            public Action OnYesClicked;
            public Action OnNoClicked;
        }
    }
}
