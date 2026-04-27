using NFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Old
{
    public class DataManager : SingletonMono<DataManager>
    {
        private const string KEY_DATA = "DataGame_";

        [SerializeField]
        private GameData _gameData = new GameData();

        public GameData GameData
        {
            get { return _gameData; }
            set { _gameData = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            InitData();
        }

        public void InitData()
        {
            _gameData = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString(KEY_DATA));
            if (_gameData == null)
            {
                _gameData = new GameData();

                /*
                //init skin player
                for (int i = 0; i < PlayerSkinConfig.I.PlayerSkinDefines.Length; i++)
                {
                    if (PlayerSkinConfig.I.PlayerSkinDefines[i].IsDefault)
                    {
                        _gameData.IDSkinPlayerSelected = PlayerSkinConfig.I.PlayerSkinDefines[i].ID;
                        _gameData.PlayerSkinUnlocks.Add(new SerializableDictionary<int, bool>.Pair(PlayerSkinConfig.I.PlayerSkinDefines[i].ID, true));
                    }
                    else _gameData.PlayerSkinUnlocks.Add(new SerializableDictionary<int, bool>.Pair(PlayerSkinConfig.I.PlayerSkinDefines[i].ID, false));
                }*/

                SaveData();
            }
        }

        [ContextMenu("SaveData")]
        public void SaveData()
        {
            PlayerPrefs.SetString(KEY_DATA, JsonUtility.ToJson(_gameData));
        }



        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

    }

    [System.Serializable]
    public class GameData
    {
        [Header("Main Data")]
        public int Level;
        public int Round;
        public long Coin;
        public string NamePlayer;
        public bool IsHapic;
        public bool IsSound;
        public bool IsMusic;
        public bool IsRemoveAds;

        public int IDSkinPlayerSelected;
        public SerializableDictionary<int, bool> PlayerSkinUnlocks;

        public GameData()
        {
            Level = 0;
            Round = 0;
            Coin = 0;
            NamePlayer = "";
            IsHapic = true;
            IsSound = true;
            IsMusic = true;
            IsRemoveAds = false;

            IDSkinPlayerSelected = 0;
            PlayerSkinUnlocks = new SerializableDictionary<int, bool>();
        }
    }
}

