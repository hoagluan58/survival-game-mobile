using SquidGame.LandScape.BehaviourTree;
using SquidGame.LandScape.Survival;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class FindOnlyPlayerNode : CombatAbstractNode
    {
        protected override NodeState OnUpdate()
        {
            if (ActiveNode.CurrentOpponent != null && !((IDamageable)ActiveNode.CurrentOpponent).IsDead()) return NodeState.SUCCESS;
            List<IInfomation> userList = GetListUserInfor();
            foreach (var target in userList)
            {
                if (target.IsPlayer())
                {
                    ActiveNode.SetOpponent(target);
                    break;
                }
            }
            return NodeState.SUCCESS;
        }
    }
}
