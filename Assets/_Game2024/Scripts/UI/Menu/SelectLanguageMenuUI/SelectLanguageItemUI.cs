using EnhancedUI.EnhancedScroller;
using SquidGame.Core;
using SquidGame.LandScape;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace SquidGame
{
    public class SelectLanguageItemUI : EnhancedScrollerCellView
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _flagIMG;
        [SerializeField] private TextMeshProUGUI _nameTMP;
        [SerializeField] private Button _selectBTN;

        [Header("GROUP")]
        [SerializeField] private GameObject _onGroup;
        [SerializeField] private GameObject _offGroup;

        public RectTransform RectTransform => _rectTransform;
        private Locale _locale;
        private Action _onSelect;

        private void OnEnable() => _selectBTN.onClick.AddListener(OnSelectButtonClicked);

        private void OnDisable() => _selectBTN.onClick.RemoveListener(OnSelectButtonClicked);

        private void OnSelectButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            GameLocalization.I.ChangeLanguage(_locale);
            _onSelect?.Invoke();
        }

        public void SetData(Locale locale, Action onSelect = null)
        {
            _locale = locale;
            _onSelect = onSelect;

            var isSelected = LocalizationSettings.SelectedLocale == _locale;

            _nameTMP.text = _locale.Identifier.CultureInfo.NativeName;
            _nameTMP.color = isSelected ? Color.white : Color.black;
            _flagIMG.sprite = GameLocalization.I.GetFlagSprite(_locale.Identifier.Code);

            _offGroup.SetActive(!isSelected);
            _onGroup.SetActive(isSelected);
        }
    }
}
