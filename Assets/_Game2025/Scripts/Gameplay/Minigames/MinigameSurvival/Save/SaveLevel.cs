using SquidGame.LandScape.Game;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class SaveLevel : MonoBehaviour
    {
        public const string CHALLENGE_LEVEL_KEY = "Survival_Challenge";
        public const string MINIGAME_LEVEL_KEY = "Survival_Minigame";        

        public int GetCurrentLevel()
        {
            CheckFirstTime();
            return PlayerPrefs.GetInt(GetKeySaveLevel());            
        }

        private void CheckFirstTime()
        {
            if (!PlayerPrefs.HasKey(CHALLENGE_LEVEL_KEY))
            {
                PlayerPrefs.SetInt(CHALLENGE_LEVEL_KEY, 1);
            }
            if (!PlayerPrefs.HasKey(MINIGAME_LEVEL_KEY))
            {
                PlayerPrefs.SetInt(MINIGAME_LEVEL_KEY, 1);
            }
        }

        public void InscreaseLevel()
        {
            PlayerPrefs.SetInt(GetKeySaveLevel(), PlayerPrefs.GetInt(GetKeySaveLevel()) + 1);
        }

        private string GetKeySaveLevel()
        {
            string key = GameManager.I.CurGameModeHandler.GameMode == EGameMode.Challenge ? CHALLENGE_LEVEL_KEY : MINIGAME_LEVEL_KEY;
            return key;
        }
    }
}
