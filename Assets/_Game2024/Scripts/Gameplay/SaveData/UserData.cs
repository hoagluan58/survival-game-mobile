using NFramework;
using Redcode.Extensions;
using Sirenix.OdinInspector;
using SquidGame.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.SaveData
{
    public class UserData : SingletonMono<UserData>, ISaveable
    {
        public static event Action OnCoinChanged;
        public static event Action OnSkinChanged;

        [SerializeField] private SaveData _saveData;

        public int MaxChallengeDay => _saveData.MaxChallengeDay;

        public int Coin
        {
            get => _saveData.Coin;
            set
            {
                _saveData.Coin = value;
                OnCoinChanged?.Invoke();
                DataChanged = true;
            }
        }

        public int Day
        {
            get => _saveData.Day;
            set
            {
                var isMaxDay = value > _saveData.MaxChallengeDay;

                if (isMaxDay)
                {
                    _saveData.ChallengeCompletedCount++;
                }

                NextMaxDay();
                _saveData.Day = isMaxDay ? 1 : value;
                GetMinigameId();
                DataChanged = true;
            }
        }

        public bool IsFirstTimePlayed
        {
            get => _saveData.IsFirstTimePlayed;
            set
            {
                _saveData.IsFirstTimePlayed = value;
                DataChanged = true;
            }
        }

        public bool IsShowRateUsPopup
        {
            get => _saveData.IsShowRateUsPopup;
            set
            {
                _saveData.IsShowRateUsPopup = value;
                DataChanged = true;
            }
        }

        public List<int> TrainingMinigameUnlockedByAds => _saveData.TrainingMinigameUnlockedByAds;

        public void UnlockNewTrainingMinigame(int id)
        {
            _saveData.TrainingMinigameUnlockedByAds.Add(id);
            DataChanged = true;
        }

        public int CurMinigameId
        {
            get
            {
                return _saveData.CurMinigameId;
            }
        }

        public void GetMinigameId()
        {
            var config = ConfigManager.I.MinigameConfig.Keys;
            var rndId = -1;

            if (IsFirstTimeChallenge())
            {
                rndId = config.First();
            }
            else if (IsPlayedAllMinigame())
            {
                _saveData.PlayedMinigameList = new List<int>();
                rndId = config.GetRandomElement();
            }
            else
            {
                var pool = new List<int>();
                foreach (var minigameId in config)
                {
                    if (!_saveData.PlayedMinigameList.Contains(minigameId))
                    {
                        pool.Add(minigameId);
                    }
                }
                rndId = pool.RandomItem();
            }

            _saveData.CurMinigameId = rndId;
            if (!_saveData.PlayedMinigameList.Contains(rndId))
            {
                _saveData.PlayedMinigameList.Add(rndId);
            }
            DataChanged = true;

            bool IsPlayedAllMinigame() => _saveData.PlayedMinigameList.Count == config.Count;

            bool IsFirstTimeChallenge() => _saveData.ChallengeCompletedCount == 0 && _saveData.Day == 1;
        }

        public bool IsSkinUnlocked(int id) => _saveData.HatsUnlocked.Contains(id);

        public int CurrentHatId => _saveData.CurrentHatId;

        public void ChangeHat(int hatId)
        {
            _saveData.CurrentHatId = hatId;
            OnSkinChanged?.Invoke();
            DataChanged = true;
        }

        public void UnlockSkin(int hatId)
        {
            if (_saveData.HatsUnlocked.Contains(hatId)) return;
            _saveData.HatsUnlocked.Add(hatId);
            DataChanged = true;
        }

        public bool IsMaxDay => _saveData.Day == _saveData.MaxChallengeDay;

        private void NextMaxDay()
        {
            var config = Define.CHALLENGE_MAX_DAY_CONFIG;

            if (!config.IsIndexOutOfList(_saveData.ChallengeCompletedCount))
            {
                _saveData.MaxChallengeDay = config[_saveData.ChallengeCompletedCount];
            }
            else
            {
                _saveData.MaxChallengeDay = config[^1];
            }
        }

        #region ISaveable

        [Serializable]
        public class SaveData
        {
            public int Coin;

            public int ChallengeCompletedCount;
            public int MaxChallengeDay;
            public int Day;

            public int CurMinigameId;
            public List<int> PlayedMinigameList;
            public List<int> TrainingMinigameUnlockedByAds;

            public List<int> HatsUnlocked;
            public int CurrentHatId;

            public bool IsFirstTimePlayed;
            public bool IsShowRateUsPopup;
        }

        public string SaveKey => "UserData";

        public bool DataChanged { get; set; }

        public object GetData() => _saveData;

        public void SetData(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                // Init default values
                _saveData = new SaveData()
                {
                    Day = 1,
                    IsFirstTimePlayed = true,
                    PlayedMinigameList = new(),
                    TrainingMinigameUnlockedByAds = new List<int>(),

                    // Unlock default skin
                    HatsUnlocked = new() { 0 },
                    CurrentHatId = 0,
                };

                var defaultMinigameId = ConfigManager.I.MinigameConfig.First().Key;
                _saveData.CurMinigameId = defaultMinigameId;
                _saveData.PlayedMinigameList.Add(defaultMinigameId);
                NextMaxDay();
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