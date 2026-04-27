using NFramework;
using Sirenix.OdinInspector;
using SquidGame.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SquidGame.SaveData
{
    public class DailyRewardSaveData : SingletonMono<DailyRewardSaveData>, ISaveable
    {
        [SerializeField] private SaveData _saveData;

        public bool IsClaimAll() => _saveData.LastClaimDay >= ConfigManager.I.DailyRewardConfig.Count;

        public enum EDailyState
        {
            Ready,
            Claimed,
            Future,
        }

        public EDailyState GetRewardState(int day)
        {
            if (day <= _saveData.LastClaimDay)
            {
                return EDailyState.Claimed;
            }
            else if (day == _saveData.LastClaimDay + 1)
            {
                return EDailyState.Ready;
            }
            else
            {
                return EDailyState.Future;
            }
        }

        public bool CanClaimDaily()
        {
            if (string.IsNullOrEmpty(_saveData.LastClaimTime))
            {
                return true;
            }

            var now = DateTime.Now;
            var lastClaimTime = DateTime.Parse(_saveData.LastClaimTime);
            var timeDifference = now - lastClaimTime;

            if (timeDifference.TotalHours >= 24)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [Button]
        public List<RewardData> Claim(int multiply = 1)
        {
            _saveData.LastClaimTime = DateTime.Now.ToString();
            _saveData.LastClaimDay++;
            var config = ConfigManager.I.DailyRewardConfig[_saveData.LastClaimDay];
            var rewards = new List<RewardData>(config.Rewards);
            foreach (var reward in rewards)
            {
                reward.Amount *= multiply;
            }
            Utilities.HandleRewards(rewards);
            DataChanged = true;
            return rewards;
        }

        #region ISaveable

        [Serializable]
        public class SaveData
        {
            public int LastClaimDay;
            public string LastClaimTime;
        }

        public string SaveKey => "DailyRewardSaveData";

        public bool DataChanged { get; set; }

        public object GetData() => _saveData;

        public void SetData(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                _saveData = new SaveData();
                DataChanged = true;
            }
            else
            {
                _saveData = JsonUtility.FromJson<SaveData>(data);
            }
        }

        public void OnAllDataLoaded()
        {
        }

        #endregion
    }
}
