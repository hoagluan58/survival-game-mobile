using NFramework;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.Config
{
    public class DailyRewardConfigSO : GoogleSheetConfigSO<DailyRewardConfig>
    {
        private Dictionary<int, DailyRewardConfig> _config;

        public Dictionary<int, DailyRewardConfig> Config => _config;

        public void Init()
        {
            _config = new Dictionary<int, DailyRewardConfig>();
            foreach (var data in _datas)
            {
                _config.Add(data.Day, data);
            }
        }

#if UNITY_EDITOR
        protected override void OnSynced(List<DailyRewardConfig> googleSheetData)
        {
            base.OnSynced(googleSheetData);
            foreach (var data in _datas)
            {
                data.Rewards = Utilities.ParseStringToRewardDatas(data.RewardString);
            }
        }
#endif
    }

    [System.Serializable]
    public class DailyRewardConfig
    {
        public int Day;
        public List<RewardData> Rewards;

        [HideInInspector] public string RewardString;
    }
}
