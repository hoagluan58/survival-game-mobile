using EnhancedUI.EnhancedScroller;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.SaveData;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class SelectLanguagePopupUI : BaseUIView, IEnhancedScrollerDelegate
    {
        [SerializeField] private Button _confirmBTN;
        [SerializeField] private Scrollbar _scrollBar;
        [SerializeField] private EnhancedScroller _scroller;
        [SerializeField] private SelectLanguageItemUI _selectLanguageCellView;

        private List<Locale> _datas;

        public override void OnOpen()
        {
            base.OnOpen();
            _confirmBTN.onClick.AddListener(OnConfirmButtonClicked);
            SetData();
        }

        public override void OnClose()
        {
            base.OnClose();
            _confirmBTN.onClick.RemoveListener(OnConfirmButtonClicked);
        }

        private void OnConfirmButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            CloseSelf();
            if (UserData.I.IsFirstTimePlayed)
            {
                UserData.I.IsFirstTimePlayed = false;
                GameManager.I.PlayGameMode(EGameMode.Challenge, UserData.I.CurMinigameId);
            }
        }

        private void SetData()
        {
            _scroller.Delegate = this;
            _datas = GameLocalization.I.GetAvailableLocales();
            _scroller.ReloadData();
        }

        #region EnhancedScroller

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellView = scroller.GetCellView(_selectLanguageCellView) as SelectLanguageItemUI;
            var data = _datas[dataIndex];
            cellView.SetData(data, () => _scroller.ReloadData(1 - _scrollBar.value));
            return cellView;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) => _selectLanguageCellView.RectTransform.rect.height;

        public int GetNumberOfCells(EnhancedScroller scroller) => _datas.Count;

        #endregion
    }
}
