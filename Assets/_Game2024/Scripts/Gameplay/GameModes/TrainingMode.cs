using NFramework;
using SquidGame.Core;

namespace SquidGame.Gameplay
{
    public class TrainingMode : IGameModeHandler
    {
        public EGameMode GameMode => EGameMode.Training;

        public void OnExit() => UIManager.I.Open(Define.UIName.TRAINING_MODE_MENU);

        public void OnHandleResult()
        {
            var state = GameManager.I.CurGameState;
            if (state == EGameState.Win)
            {
                UIManager.I.Open(Define.UIName.WIN_TRAINING_MODE_POPUP);
            }
            else if (state == EGameState.Lose)
            {
                UIManager.I.Open(Define.UIName.LOSE_TRAINING_MODE_POPUP);
            }
        }

        public void OnRetry()
        {
            GameManager.I.ReloadMinigame();
        }

        public void OnStart()
        {
        }
    }
}
