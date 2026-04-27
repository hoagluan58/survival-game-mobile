using EnhancedUI.EnhancedScroller;
using SquidGame.Config;
using SquidGame.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.UI
{
    public class TrainingItemScrollerUI : MonoBehaviour, IEnhancedScrollerDelegate
    {
        [SerializeField] private EnhancedScroller _scroller;
        [SerializeField] private TrainingItemCellViewUI _cellViewUIPf;

        private List<MinigameConfig> _datas;

        public void SetData()
        {
            _datas = ConfigManager.I.MinigameConfig.Values.ToList();
            _scroller.Delegate = this;
            _scroller.ReloadData();
        }

        #region EnhancedScroller

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellView = scroller.GetCellView(_cellViewUIPf) as TrainingItemCellViewUI;
            cellView.name = "Cell " + (dataIndex * cellView.NumOfCells).ToString() + " to " + ((dataIndex * cellView.NumOfCells) + cellView.NumOfCells - 1).ToString();
            cellView.SetData(ref _datas, dataIndex * cellView.NumOfCells);
            return cellView;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) => _cellViewUIPf.RectTransform.rect.height;

        public int GetNumberOfCells(EnhancedScroller scroller) => Mathf.CeilToInt((float)_datas.Count / (float)_cellViewUIPf.NumOfCells);

        #endregion
    }
}
