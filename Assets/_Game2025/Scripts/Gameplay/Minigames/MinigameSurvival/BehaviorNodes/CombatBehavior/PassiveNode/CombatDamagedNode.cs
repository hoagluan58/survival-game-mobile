using SquidGame.LandScape.BehaviourTree;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class CombatDamagedNode : Node
    {
        private CombatRootNode _rootNode => parent as CombatRootNode;
        private Bot _bot => _rootNode.Bot;        

        protected override NodeState OnUpdate()
        {
            if (_bot.IsInState(_bot.BotDamagedState))
            {                
                _bot.Agent.SetDestination(transform.position);
                return NodeState.SUCCESS;
            }
            return NodeState.FAILURE;
        }
    }
}
