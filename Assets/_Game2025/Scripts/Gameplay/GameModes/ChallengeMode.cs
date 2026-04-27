using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Data;
using SquidGame.LandScape.Minigame2;
using SquidGame.LandScape.UI;

namespace SquidGame.LandScape.Game
{
    public class ChallengeMode : IGameModeHandler
    {
        public EGameMode GameMode => EGameMode.Challenge;

        public void OnEnter()
        {
            CanRevive = true;
        }

        public void OnExit()
        {

        }

        public void OnHandleResult()
        {
            var _saveData = new LevelSaveData(GameManager.I.CurGameModeHandler.GameMode);
            if (GameManager.I.CurGameState == EGameState.Win)
            {
                UIManager.I.Open(Define.UIName.WIN_CHALLENGE_POPUP);
                var userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
                userData.UpdateSeasonProgress(GameManager.I.CurSeasonId);
            }
            else if (GameManager.I.CurGameState == EGameState.Lose)
            {
                UIManager.I.Open<LoseChallengePopupUI>(Define.UIName.LOSE_CHALLENGE_POPUP).ShowRevivePopup(CanRevive);
            }
        }

        public bool CanRevive { get; set; }
    }
}
