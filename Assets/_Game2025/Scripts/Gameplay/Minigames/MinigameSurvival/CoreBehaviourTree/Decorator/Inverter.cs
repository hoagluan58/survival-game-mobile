using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.BehaviourTree
{
    public class Inverter : DecoratorNode
    {
        protected override NodeState OnUpdate()
        {
            switch (child.UpdateNode())
            {
                case NodeState.RUNNING:
                    return NodeState.RUNNING;
                case NodeState.FAILURE:
                    return NodeState.SUCCESS;
                case NodeState.SUCCESS:
                    return NodeState.FAILURE;
            }
            return NodeState.FAILURE;
        }
    }

}

