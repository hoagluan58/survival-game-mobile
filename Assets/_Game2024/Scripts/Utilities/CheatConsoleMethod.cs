using IngameDebugConsole;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using UnityEngine;

namespace SquidGame.Utils
{
    public static class CheatConsoleMethod
    {
        [ConsoleMethod("/go", "Go to minigame")]
        public static void GoToMinigame(int id)
        {
            UIManager.I.CloseAllInLayer(EUILayer.Menu);
            if (!ConfigManager.I.MinigameConfig.ContainsKey(id))
            {
                Debug.Log($"No minigame with {id} in config");
                return;
            }
            GameManager.I.LoadMinigame(id);
        }

        [ConsoleMethod("/win", "Win current game")]
        public static void WinMinigame()
        {
            GameManager.I.Win();
        }

        [ConsoleMethod("/lose", "Lose current game")]
        public static void LoseMinigame()
        {
            GameManager.I.Lose();
            GameManager.I.HandleResult();
        }

        [ConsoleMethod("/bigbang", "Big bang?")]
        public static void Quit()
        {
            Application.Quit();
        }

        [ConsoleMethod("/forceui", "Force show ui")]
        public static void ForceOpenView(string identifier)
        {
            UIManager.I.Open(identifier);
        }
    }
}
