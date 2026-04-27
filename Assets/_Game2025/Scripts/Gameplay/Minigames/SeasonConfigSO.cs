using NFramework;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Config
{
    public class SeasonConfigSO : GoogleSheetConfigSO<SeasonConfig>
    {
        private Dictionary<int, SeasonConfig> _configs;
        public Dictionary<int, SeasonConfig> Configs => _configs;

        public void Init()
        {
            _configs = new Dictionary<int, SeasonConfig>();
            foreach (var data in _datas)
            {
                _configs[data.Season] = data;
            }
        }

        public SeasonConfig GetSeasonConfig(int seasonId) => _configs.GetValueOrDefault(seasonId);

#if UNITY_EDITOR
        [Header("EDITOR")]
        [SerializeField] private MinigameConfigSO _minigameConfigSO;

        protected override void OnSynced(List<SeasonConfig> googleSheetData)
        {
            base.OnSynced(googleSheetData);

            foreach (var data in _datas)
            {
                var parseMinigameIds = Utilities.ParseStringToList<int>(data.MinigameListString, ",");
                data.MinigameList = new List<MinigameConfig>();

                foreach (var id in parseMinigameIds)
                {
                    data.MinigameList.Add(_minigameConfigSO.GetConfig(id));
                }

                data.Thumbnail = FileUtils.LoadFirstAssetWithName<Sprite>($"Thumbnail_Season_{data.Season}");
            }
        }
#endif
    }

    [System.Serializable]
    public class SeasonConfig
    {
        public int Season;
        public List<MinigameConfig> MinigameList;
        public Sprite Thumbnail;
        public bool IsForceLocked;

        [HideInInspector] public string MinigameListString;
    }
}
