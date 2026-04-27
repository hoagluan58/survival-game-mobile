using SquidGame.LandScape.Game;
using UnityEngine;

namespace SquidGame.LandScape.Minigame2
{
    public class LevelSaveData
    {
        private const int DEFAULT_LEVEL = 1;
        private const string CHALLENGE_MODE_LEVEL = "Minigame_2_Challenge_Level";
        private const string MINIGAME_MODE_LEVEL = "Minigame_2_Minigame_Level";

        private string _curSaveKey;
        private int _level;

        public int Level => _level;

        public LevelSaveData(EGameMode gameMode)
        {
            _curSaveKey = gameMode == EGameMode.Challenge ? CHALLENGE_MODE_LEVEL : MINIGAME_MODE_LEVEL;
            Load();
        }

        private void Load() => _level = PlayerPrefs.GetInt(_curSaveKey, DEFAULT_LEVEL);

        public void Save() => PlayerPrefs.SetInt(_curSaveKey, _level + 1);
    }
}
