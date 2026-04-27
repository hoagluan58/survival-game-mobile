using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.BehaviourTree
{
    public class Succeed : DecoratorNode
    {
        protected override NodeState OnUpdate()
        {
            var state = child.UpdateNode();
            if (state == NodeState.FAILURE)
            {
                return NodeState.SUCCESS;
            }
            return state;
        }
    }

}

