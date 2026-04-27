namespace SquidGame.LandScape.Game
{
    public interface INPCState
    {
        public EState StateName { get; }

        public void OnInit();

        public void OnEnter();

        public void OnUpdate();

        public void OnExit();

        public enum EState
        {
            None,
            Idle,
            Wander,
            MoveToDestination,
        }
    }
}