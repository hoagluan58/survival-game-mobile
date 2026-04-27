using SquidGame.LandScape.Game;
using System;

namespace SquidGame.LandScape.MinigameRockPaperScissors
{
    public class Opponent : Character
    {
        public override void Init()
        {
            base.Init();
        }

        public override void Dead()
        {
            base.Dead();
        }

        public override void OnRevive()
        {
            base.OnRevive();
        }


        public GameResult GetGameResult(GameResult playerResult)
        {
            var isPlayerWin = UnityEngine.Random.Range(0f, 1f) > 0.6f;
            if (isPlayerWin) return GetLose(playerResult);
            return GetWin(playerResult);
        }


        private GameResult GetLose(GameResult result)
        {
            return result == GameResult.Rock ? GameResult.Scissor : result == GameResult.Paper ? GameResult.Rock : GameResult.Paper;
        }

        private GameResult GetWin(GameResult result)
        {
            return result == GameResult.Rock ? GameResult.Paper : result == GameResult.Paper ? GameResult.Scissor : GameResult.Rock;
        }

    }
}
