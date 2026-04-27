namespace SquidGame.Gameplay
{
    public interface IGameModeHandler
    {
        public EGameMode GameMode { get; }

        public void OnStart();

        public void OnRetry();

        public void OnExit();

        public void OnHandleResult();
    }
}
