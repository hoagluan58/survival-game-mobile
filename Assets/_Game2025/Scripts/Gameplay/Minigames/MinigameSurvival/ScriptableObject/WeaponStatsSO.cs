using NFramework;
using System;
using System.Collections.Generic;

namespace SquidGame.LandScape.Survival
{
    public class WeaponStatsSO : GoogleSheetConfigSO<WeaponStatsConfig>
    {
        private Dictionary<WeaponType, WeaponStatsConfig> _config;

        public Dictionary<WeaponType, WeaponStatsConfig> Config => _config;

        public void Init()
        {
            _config = new Dictionary<WeaponType, WeaponStatsConfig>();
            foreach (var data in _datas)
            {
                _config.Add(data.WeaponType, data);
            }
        }

        public WeaponStatsConfig GetConfig(WeaponType weaponType)
        {
            return _config[weaponType];
        }
    }

    [Serializable]
    public class WeaponStatsConfig
    {
        public WeaponType WeaponType;
        public int Damage;
        public float Range;
        public float DelayTime;
        public float ExistTime;
    }
}
