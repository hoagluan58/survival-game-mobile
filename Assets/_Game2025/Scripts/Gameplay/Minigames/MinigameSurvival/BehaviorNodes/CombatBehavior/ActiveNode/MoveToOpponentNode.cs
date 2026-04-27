using SquidGame.LandScape.BehaviourTree;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class MoveToOpponentNode : CombatAbstractNode
    {

        protected override NodeState OnUpdate()
        {
            if (ActiveNode.CurrentOpponent == null) return NodeState.FAILURE;
            if (!bot.IsInState(bot.MoveState))
                bot.ChangeState(bot.MoveState);
            if (bot.Agent.isStopped)
                bot.Agent.isStopped = false;
            bot.Agent.SetDestination(ActiveNode.CurrentOpponent.GetTransform().position);
            bot.Agent.transform.LookAt(ActiveNode.CurrentOpponent.GetTransform());

            return NodeState.SUCCESS;
        }

    }
}
