using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.BehaviourTree
{
    public class InterruptSelector : Selector
    {
        protected override NodeState OnUpdate()
        {
            int previous = current;
            base.OnStart();
            var status = base.OnUpdate();
            if (previous != current)
            {
                if (nodes[previous].nodeState == NodeState.RUNNING)
                {
                    nodes[previous].Abort();
                }
            }

            return status;
        }
    }

}

