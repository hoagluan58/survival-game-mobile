using EnhancedUI.EnhancedScroller;
using SquidGame.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.UI
{
    public class TrainingItemCellViewUI : EnhancedScrollerCellView
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TrainingItemUI[] _items;

        public RectTransform RectTransform => _rectTransform;
        public int NumOfCells => _items.Length;

        public void SetData(ref List<MinigameConfig> datas, int startingIndex)
        {
            for (var i = 0; i < _items.Length; i++)
            {
                _items[i].SetData(startingIndex + i < datas.Count ? datas[startingIndex + i] : null);
            }
        }
    }
}
