using EnhancedUI.EnhancedScroller;
using SquidGame.LandScape.Config;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.LandScape.UI
{
    public class ShopScrollerUI : MonoBehaviour, IEnhancedScrollerDelegate
    {
        [SerializeField] private NotificationPanelUI _notiPanel;
        [SerializeField] private EnhancedScroller _scroller;
        [SerializeField] private ShopItemUI _itemPf;

        private UserData _userData;
        private List<HairConfig> _configs;

        public void SetData()
        {
            _userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
            _configs = GameConfig.I.HairConfigs.Values.ToList();
            _scroller.Delegate = this;
            _scroller.ReloadData();

            var curHair = _userData.UserHair;
            var dataIndex = _configs.FindIndex(x => x.Id == curHair);
            _scroller.JumpToDataIndex(dataIndex, 0.5f, 0.5f);
        }

        #region EnhancedScroller
        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellview = scroller.GetCellView(_itemPf) as ShopItemUI;
            cellview.name = $"Cell {cellIndex}";
            cellview.SetData(_configs[dataIndex], OnSelectHair, OnNotEnoughMoney);
            return cellview;

            void OnSelectHair() => _scroller.RefreshActiveCellViews();

            void OnNotEnoughMoney() => _notiPanel.SetData();
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) => _itemPf.RectTransform.rect.width;

        public int GetNumberOfCells(EnhancedScroller scroller) => _configs.Count;

        #endregion
    }
}
