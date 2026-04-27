namespace SquidGame.LandScape.Game
{
    public interface IGameModeHandler
    {
        public EGameMode GameMode { get; }

        public void OnEnter();

        public void OnExit();

        public void OnHandleResult();
    }
}
