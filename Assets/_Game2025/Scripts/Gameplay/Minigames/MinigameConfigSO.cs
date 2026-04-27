using NFramework;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Config
{
    public class MinigameConfigSO : GoogleSheetConfigSO<MinigameConfig>
    {
        private Dictionary<int, MinigameConfig> _configs;
        public Dictionary<int, MinigameConfig> Configs => _configs;

        public void Init()
        {
            _configs = new Dictionary<int, MinigameConfig>();
            foreach (var config in _datas)
            {
                _configs.Add(config.Id, config);
            }
        }

        public MinigameConfig GetConfig(int id) => _datas.Find(x => x.Id == id);

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

    [System.Serializable]
    public class MinigameConfig
    {
        public int Id;
        public string Name;
        public string SceneName;
        public Sprite Thumbnail;
    }
}
