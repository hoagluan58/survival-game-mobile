using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.BehaviourTree
{
    public class Selector : CompositeNode
    {
        [SerializeField] protected int current;
        protected override void OnStart()
        {
            base.OnStart();
            current = 0;
        }


        protected override NodeState OnUpdate()
        {
            for (int i = current; i < nodes.Count; ++i)
            {
                current = i;
                var child = nodes[current];

                switch (child.UpdateNode())
                {
                    case NodeState.SUCCESS:
                        nodeState = NodeState.SUCCESS;
                        return nodeState;
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.RUNNING:
                        nodeState = NodeState.RUNNING;
                        return nodeState;
                }
            }

            nodeState = NodeState.FAILURE;
            return nodeState;
        }
    }

}
