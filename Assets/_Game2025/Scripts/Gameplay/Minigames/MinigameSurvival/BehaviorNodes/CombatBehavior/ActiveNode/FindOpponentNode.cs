using SquidGame.LandScape.BehaviourTree;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class FindOpponentNode : CombatAbstractNode
    {
        protected override NodeState OnUpdate()
        {
            if (ActiveNode.CurrentOpponent != null && !((IDamageable)ActiveNode.CurrentOpponent).IsDead()) return NodeState.SUCCESS;
            List<IInfomation> userList = GetListUserInfor();            
            float minDistance = Mathf.Infinity;
            foreach (var target in userList)
            {
                float currentDistance = Vector3.Distance(target.GetTransform().position, transform.position);
                if (currentDistance >= minDistance) continue;
                minDistance = currentDistance;
                ActiveNode.SetOpponent(target);
            }
            return NodeState.SUCCESS;
        }        
    }
}
