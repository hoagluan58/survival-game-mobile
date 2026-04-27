namespace SquidGame.LandScape.MinigameMingle
{
    public class DieState : NpcStateMachine
    {
        public override void Enter()
        {
            NpcBase.NavMeshAgent.enabled = false;
            NpcBase.BaseCharacter.ToggleGreyScale(true);
            Die();
        }

        public override void Exit()
        {
            base.Exit();
            NpcBase.NavMeshAgent.enabled = true;
            NpcBase.BaseCharacter.ToggleGreyScale(false);
        }
    }
}
