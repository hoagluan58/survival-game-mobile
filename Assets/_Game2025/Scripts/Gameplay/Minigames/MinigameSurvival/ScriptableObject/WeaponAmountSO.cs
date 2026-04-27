using NFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class WeaponAmountSO : GoogleSheetConfigSO<WeaponAmountConfig>
    {
        private Dictionary<int, WeaponAmountConfig> _config;

        public Dictionary<int, WeaponAmountConfig> Config => _config;

        public void Init()
        {
            _config = new Dictionary<int, WeaponAmountConfig>();
            foreach (var data in _datas)
            {
                _config.Add(data.Level, data);
            }
        }

        public WeaponAmountConfig GetConfig(int level)
        {
            if (!_config.TryGetValue(level, out WeaponAmountConfig config))
            {
                config = _config.Values.Last();
            }
            return config;
        }

#if UNITY_EDITOR
        protected override void OnSynced(List<WeaponAmountConfig> googleSheetData)
        {
            base.OnSynced(googleSheetData);
            foreach (var data in _datas)
            {
                data.WeaponAmountList = ConvertStringToList(data.WeaponAmountString);
            }
        }

        private List<WeaponAmountDetail> ConvertStringToList(string weaponAmountConfig)
        {
            List<WeaponAmountDetail> list = new List<WeaponAmountDetail>();
            string[] eachWeaponConfig = weaponAmountConfig.Split(';');
            foreach (var config in eachWeaponConfig)
            {
                string[] strings = config.Split(",");
                WeaponType weaponType = strings[0].ParseToEnum<WeaponType>();
                int amount = strings[1].ParseToInt();
                list.Add(new WeaponAmountDetail(weaponType, amount));
            }
            return list;
        }
#endif
    }

    [Serializable]
    public class WeaponAmountConfig
    {
        public int Level;
        public List<WeaponAmountDetail> WeaponAmountList = new List<WeaponAmountDetail>();
        [HideInInspector] public string WeaponAmountString;
    }

    [Serializable]
    public class WeaponAmountDetail
    {
        public WeaponType WeaponType;
        public int Amount;

        public WeaponAmountDetail(WeaponType weaponType, int amount)
        {
            WeaponType = weaponType;
            Amount = amount;
        }

        public override bool Equals(object obj)
        {
            if (obj is WeaponAmountDetail other)
            {
                return this.WeaponType == other.WeaponType;
            }
            return false;
        }
        
        public override int GetHashCode()
        {
            return WeaponType.GetHashCode();
        }
    }
}
