using NFramework;
using SquidGame.Core;
using SquidGame.SaveData;

namespace SquidGame.Gameplay
{
    public class ChallengeMode : IGameModeHandler
    {
        public EGameMode GameMode => EGameMode.Challenge;

        public void OnStart()
        {
        }

        public void OnRetry() => GameManager.I.ReloadMinigame();

        public void OnExit()
        {
            UserData.I.Day = 1;
        }

        public void OnHandleResult()
        {
            var state = GameManager.I.CurGameState;
            if (state == EGameState.Win)
            {
                UIManager.I.Open(Define.UIName.WIN_CHALLENGE_MODE_POPUP);
            }
            else if (state == EGameState.Lose)
            {
                UIManager.I.Open(Define.UIName.LOSE_CHALLENGE_MODE_POPUP);
            }
        }
    }
}
