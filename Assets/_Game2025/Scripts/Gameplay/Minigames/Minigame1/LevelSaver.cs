using SquidGame.LandScape.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape
{


    public class LevelSaver : MonoBehaviour
    {

        public string KEY_LEVEL_SEASON = "LEVEL_MINIGAME1_SEASON";
        public string KEY_LEVEL_MINIGAME = "LEVEL_MINIGAME1_MINIGAME";

        public int GetLevel(EGameMode gameMode) => PlayerPrefs.GetInt(gameMode == EGameMode.Challenge ? KEY_LEVEL_SEASON : KEY_LEVEL_MINIGAME, 0);

        public void SetLevel(EGameMode gameMode, int value) => PlayerPrefs.SetInt(gameMode == EGameMode.Challenge ? KEY_LEVEL_SEASON : KEY_LEVEL_MINIGAME, value);

        public void LevelUp(EGameMode gameMode)
        {
            SetLevel(gameMode, PlayerPrefs.GetInt(gameMode == EGameMode.Challenge ? KEY_LEVEL_SEASON : KEY_LEVEL_MINIGAME) + 1);
        }
    }
}
