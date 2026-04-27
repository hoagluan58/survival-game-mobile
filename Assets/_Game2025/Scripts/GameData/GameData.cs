using NFramework;
using SquidGame.LandScape.Data;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Core
{
    public class GameData : SingletonMono<GameData>
    {
        private Dictionary<string, ISaveable> _dataDict = new Dictionary<string, ISaveable>();

        public void Init()
        {
            RegisterData(new UserData());
            RegisterData(VibrationManager.I);
            RegisterData(SoundManager.I);
            SaveManager.I.Load();
        }

        public T GetData<T>(string key) where T : class, ISaveable
        {
            if (_dataDict.TryGetValue(key, out ISaveable data))
            {
                return data as T;
            }
            return null;
        }

        private void RegisterData(ISaveable saveable)
        {
            SaveManager.I.RegisterSaveData(saveable);
            _dataDict[saveable.SaveKey] = saveable;
        }
    }
}
