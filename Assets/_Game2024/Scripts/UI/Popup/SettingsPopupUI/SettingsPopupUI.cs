using NFramework;
using SquidGame.Core;
using SquidGame.LandScape;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class SettingsPopupUI : BaseUIView
    {
        [SerializeField] private Button _closeBTN;
        [SerializeField] private Button _selectLanguageBTN;
        [SerializeField] private TextMeshProUGUI _curLanguageTMP;

        public override void OnOpen()
        {
            base.OnOpen();
            _closeBTN.onClick.AddListener(OnCloseButtonClicked);
            _selectLanguageBTN.onClick.AddListener(OnSelectLanguageButtonClicked);
            SetData();
        }

        public override void OnClose()
        {
            base.OnClose();
            _closeBTN.onClick.RemoveListener(OnCloseButtonClicked);
            _selectLanguageBTN.onClick.RemoveListener(OnSelectLanguageButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            CloseSelf();
        }

        private void OnSelectLanguageButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.SELECT_LANGUAGE_POPUP);
            CloseSelf();
        }

        private void SetData() => _curLanguageTMP.text = GameLocalization.I.UserLocale.Identifier.CultureInfo.NativeName;
    }
}
