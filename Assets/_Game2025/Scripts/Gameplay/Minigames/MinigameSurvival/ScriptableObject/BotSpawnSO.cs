using NFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SquidGame.LandScape.Survival
{
    public class BotSpawnSO : GoogleSheetConfigSO<BotSpawnConfig>
    {
        private Dictionary<int, BotSpawnConfig> _config;

        public Dictionary<int, BotSpawnConfig> Config => _config;

        public void Init()
        {
            _config = new Dictionary<int, BotSpawnConfig>();
            foreach (var data in _datas)
            {
                _config.Add(data.Level, data);
            }
        }

        public BotSpawnConfig GetConfig(int level)
        {
            if (!_config.TryGetValue(level, out BotSpawnConfig config))
            {
                config = _config.Values.Last();
            }
            return config;
        }
    }

    [Serializable]
    public class BotSpawnConfig
    {
        public int Level;
        public int Amount;
    }
}
