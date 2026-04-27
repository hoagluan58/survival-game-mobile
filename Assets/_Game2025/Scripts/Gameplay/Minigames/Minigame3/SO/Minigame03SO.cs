using NFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class Minigame03SO : GoogleSheetConfigSO<Minigame03Config>
    {
        private Dictionary<int, Minigame03Config> _config;

        public Dictionary<int, Minigame03Config> Config => _config;

        public void Init()
        {
            _config = new Dictionary<int, Minigame03Config>();
            foreach (var data in _datas)
            {
                _config.Add(data.Level, data);
            }
        }

        public Minigame03Config GetConfig(int level)
        {
            if (!_config.TryGetValue(level, out Minigame03Config minigame03Config))
            {
                minigame03Config = _config.Values.Last();
            }
            return minigame03Config;
        }

    }

    [Serializable]
    public class Minigame03Config
    {
        public int Level;
        public float GreenAreaPercentage;
        public float MinValue;
        public float MaxValue;
        public float Speed;
        public float PercentageSpeedEachBreak;
    }
}
