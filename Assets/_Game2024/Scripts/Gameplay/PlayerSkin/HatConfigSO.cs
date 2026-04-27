using NFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.Config
{
    public class HatConfigSO : GoogleSheetConfigSO<HatConfig>
    {
        private Dictionary<int, HatConfig> _config;

        public Dictionary<int, HatConfig> Config => _config;

        public void Init()
        {
            _config = new Dictionary<int, HatConfig>();
            foreach (var data in _datas)
            {
                _config.Add(data.Id, data);
            }
        }

#if UNITY_EDITOR
        protected override void OnSynced(List<HatConfig> googleSheetData)
        {
            base.OnSynced(googleSheetData);
            foreach (var data in _datas)
            {
                data.Icon = FileUtils.LoadFirstAssetWithName<Sprite>($"hat_{data.Id}");
            }
        }
#endif
    }

    [Serializable]
    public class HatConfig
    {
        public int Id;
        public bool IsDefault;
        public bool CanNotBuy;

        [Header("UI")]
        public Sprite Icon;
    }
}