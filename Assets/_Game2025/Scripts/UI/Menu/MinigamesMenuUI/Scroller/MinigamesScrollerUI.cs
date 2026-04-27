using EnhancedUI.EnhancedScroller;
using SquidGame.LandScape.Config;
using SquidGame.LandScape.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.LandScape.UI
{
    public class MinigamesScrollerUI : MonoBehaviour, IEnhancedScrollerDelegate
    {
        [SerializeField] private EnhancedScroller _scroller;
        [SerializeField] private MinigameItemCellViewUI _cellViewUIPf;

        private List<MinigameConfig> _datas;
        private readonly int _comingSoonItemCount = 1;

        public void SetData()
        {
            _datas = GameConfig.I.MinigameConfigs.Values.ToList();
            for (var i = 0; i < _comingSoonItemCount; i++)
            {
                _datas.Add(null);
            }
            _scroller.Delegate = this;
            _scroller.ReloadData();
        }

        #region EnhancedScroller

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellView = scroller.GetCellView(_cellViewUIPf) as MinigameItemCellViewUI;
            cellView.name = "Cell " + (dataIndex * cellView.NumOfCells).ToString() + " to " + ((dataIndex * cellView.NumOfCells) + cellView.NumOfCells - 1).ToString();
            cellView.SetData(ref _datas, dataIndex, GetNumberOfCells(scroller) - 1);
            return cellView;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) => _cellViewUIPf.RectTransform.rect.width;

        public int GetNumberOfCells(EnhancedScroller scroller) => Mathf.CeilToInt((float)_datas.Count / (float)_cellViewUIPf.NumOfCells);

        #endregion
    }
}
