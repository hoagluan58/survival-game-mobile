using NFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.Config
{
    public class MinigameConfigSO : GoogleSheetConfigSO<MinigameConfig>
    {
        private Dictionary<int, MinigameConfig> _config;

        public Dictionary<int, MinigameConfig> Config => _config;

        public void Init()
        {
            _config = new Dictionary<int, MinigameConfig>();
            foreach (var data in _datas)
            {
                _config.Add(data.Id, data);
            }
        }

#if UNITY_EDITOR
        protected override void OnSynced(List<MinigameConfig> googleSheetData)
        {
            base.OnSynced(googleSheetData);
            foreach (var data in _datas)
            {
                data.Thumbnail = FileUtils.LoadFirstAssetWithName<Sprite>($"Thumbnail_Minigame_{data.Id}");
            }
        }
#endif
    }


    [Serializable]
    public class MinigameConfig
    {
        public int Id;
        public string SceneName;
        public bool CanRevive;
        public bool IsBooster;

        [Header("UI")]
        public Sprite Thumbnail;
        public bool IsHot;
        public bool IsNew;
        public bool IsAds;
    }
}