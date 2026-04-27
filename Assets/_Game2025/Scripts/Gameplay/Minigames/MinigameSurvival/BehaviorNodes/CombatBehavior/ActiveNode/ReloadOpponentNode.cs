using SquidGame.LandScape.BehaviourTree;
using SquidGame.LandScape.Minigame3;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class ReloadOpponentNode : CombatAbstractNode
    {
        private Timer _reloadOpponentTimer = new Timer();

        protected override NodeState OnUpdate()
        {
            if (_reloadOpponentTimer.CheckTimer())
            {
                _reloadOpponentTimer.SetCooldownTime(bot.TimeToReloadOpponent);
                ActiveNode.SetOpponent(null);
                return NodeState.SUCCESS;
            }

            return NodeState.FAILURE;
        }
    }
}
