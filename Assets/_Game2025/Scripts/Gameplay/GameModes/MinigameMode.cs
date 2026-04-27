using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Minigame2;

namespace SquidGame.LandScape.Game
{
    public class MinigameMode : IGameModeHandler
    {
        public EGameMode GameMode => EGameMode.Minigame;

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }

        public void OnHandleResult()
        {
            var _saveData = new LevelSaveData(GameManager.I.CurGameModeHandler.GameMode);
            if (GameManager.I.CurGameState == EGameState.Win)
            {
                UIManager.I.Open(Define.UIName.WIN_MINIGAME_POPUP);
            }
            else if (GameManager.I.CurGameState == EGameState.Lose)
            {
                UIManager.I.Open(Define.UIName.LOSE_MINIGAME_POPUP);
            }
        }
    }
}
