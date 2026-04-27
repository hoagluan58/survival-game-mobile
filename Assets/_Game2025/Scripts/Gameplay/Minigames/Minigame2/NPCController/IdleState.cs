using SquidGame.LandScape.Game;

namespace SquidGame.LandScape.Minigame2
{
    public class IdleState : INPCState
    {
        private CharacterAnimator _animator;
        private GlassBridgeNPC _npc;

        public IdleState(GlassBridgeNPC npc)
        {
            _npc = npc;
        }

        public INPCState.EState StateName => INPCState.EState.Idle;

        public void OnInit()
        {
            _animator = _npc.Model.GetCom<CharacterAnimator>();
        }

        public void OnEnter()
        {
            _animator.PlayAnimation(EAnimStyle.Idle);
        }

        public void OnExit()
        {
        }

        public void OnUpdate()
        {
        }
    }
}
