using EnhancedUI.EnhancedScroller;
using SquidGame.LandScape.Config;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Data;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.UI
{
    public class MinigameItemCellViewUI : EnhancedScrollerCellView
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private MinigameItemUI[] _items;

        public RectTransform RectTransform => _rectTransform;
        public int NumOfCells => _items.Length;

        public void SetData(ref List<MinigameConfig> datas, int startingIndex, int numberOfCells)
        {
            var userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
            for (var i = 0; i < _items.Length; i++)
            {
                var config = startingIndex + i < datas.Count ? datas[startingIndex + i] : null;
                var isAds = config == null || !userData.IsMinigamePlayed(config.Id);
                _items[i].SetData(config, isAds);
                startingIndex += numberOfCells;
            }
        }
    }
}
