using NFramework;
using SquidGame.LandScape.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.LandScape.Data
{
    public class UserData : ISaveable
    {
        public static event Action<int> OnCoinChanged;
        public static event Action OnHairChanged;

        [SerializeField] private SaveData _saveData;

        public int UserHair => _saveData.CharacterData.CurHairId;

        public int Coin
        {
            get => _saveData.Coin;
            set
            {
                _saveData.Coin = value;
                OnCoinChanged?.Invoke(value);
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

        public List<int> PlayedMinigames => _saveData.PlayedMinigames;

        public void UpdateSeasonProgress(int seasonId)
        {
            var data = GetSeasonData(seasonId);
            data.Progress++;
            DataChanged = true;
        }

        public SeasonProgressData GetSeasonData(int seasonId)
        {
            var data = _saveData.SeasonProgress.Find(x => x.SeasonId == seasonId);
            if (data == null)
            {
                data = new SeasonProgressData(seasonId, 0, 0);
                _saveData.SeasonProgress.Add(data);
                DataChanged = true;
            }
            return data;
        }

        public void CompleteSeason(int seasonId)
        {
            var data = GetSeasonData(seasonId);
            data.Progress = 0;
            data.CompletedTime++;
            Coin += Define.WIN_SEASON_COIN;
            DataChanged = true;
        }

        public bool IsSeasonUnlock(int seasonId)
        {
            var data = GetSeasonData(seasonId);
            var index = _saveData.SeasonProgress.IndexOf(data);
            var config = GameConfig.I.SeasonConfigSO.GetSeasonConfig(seasonId);

            
            if (config.IsForceLocked) return false;
            if (index == 0 || index == 1 || index == 2) return true;
            if (index < 0 || index >= _saveData.SeasonProgress.Count) return false;

            return _saveData.SeasonProgress[index - 1].CompletedTime > 0;
        }

        public void TryAddPlayedMinigame(int minigameId)
        {
            if (IsMinigamePlayed(minigameId)) return;

            _saveData.PlayedMinigames.Add(minigameId);
            DataChanged = true;
        }

        public bool IsMinigamePlayed(int minigameId) => _saveData.PlayedMinigames.Contains(minigameId);

        public void UnlockHair(int hairId)
        {
            _saveData.CharacterData.UnlockHair(hairId);
            DataChanged = true;
        }

        public void ChangeHair(int hairId)
        {
            _saveData.CharacterData.CurHairId = hairId;
            OnHairChanged?.Invoke();
            DataChanged = true;
        }

        public bool IsHairUnlocked(int hairId) => _saveData.CharacterData.UnlockedHairs.Contains(hairId);

        public bool IsCurrentHair(int hairId) => _saveData.CharacterData.CurHairId == hairId;

        #region ISaveable

        [Serializable]
        public class SaveData
        {
            public int Coin;
            public bool IsFirstTimePlayed;
            public bool IsShowRateUsPopup;
            public List<SeasonProgressData> SeasonProgress = new();
            public CharacterData CharacterData;
            public List<int> PlayedMinigames = new();
        }

        public string SaveKey => Define.SaveKey.USER_DATA;

        public bool DataChanged { get; set; }

        public object GetData() => _saveData;

        public void SetData(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                _saveData = new SaveData();
                var defaultHair = GameConfig.I.HairConfigs.FirstOrDefault(x => x.Value.IsDefault).Value;
                _saveData.CharacterData = new CharacterData(defaultHair.Id);
                _saveData.IsFirstTimePlayed = true;
                DataChanged = true;
            }
            else
            {
                _saveData = JsonUtility.FromJson<SaveData>(data);
            }
        }

        public void OnAllDataLoaded() { }

        #endregion
    }

    [Serializable]
    public class SeasonProgressData
    {
        public int SeasonId;
        public int Progress; // Minigame Index In Config
        public int CompletedTime;

        public SeasonProgressData(int seasonId, int progress, int completedTime)
        {
            SeasonId = seasonId;
            Progress = progress;
            CompletedTime = completedTime;
        }

        public bool IsSeasonMaxProgress
        {
            get
            {
                var seasonConfig = GameConfig.I.SeasonConfigSO.GetSeasonConfig(SeasonId);
                return Progress >= seasonConfig.MinigameList.Count;
            }
        }
    }

    [Serializable]
    public class CharacterData
    {
        public int CurHairId;
        public List<int> UnlockedHairs = new List<int>();

        public CharacterData(int defaultHairId)
        {
            CurHairId = defaultHairId;
            UnlockHair(defaultHairId);
        }

        public void UnlockHair(int hairId)
        {
            if (UnlockedHairs.Contains(hairId)) return;

            UnlockedHairs.Add(hairId);
        }
    }
}
