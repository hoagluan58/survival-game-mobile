using SquidGame.LandScape.BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class WeaponDamagedNode : Node
    {
        private FindWeaponRootNode _rootNode => parent as FindWeaponRootNode;
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
