using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.LandScape.BehaviourTree
{
    public class Parallel : CompositeNode
    {
        [SerializeField] protected List<NodeState> childrenLeftToExecute = new List<NodeState>();

        protected override void OnStart()
        {
            childrenLeftToExecute.Clear();
            nodes.ForEach(a => {
                childrenLeftToExecute.Add(NodeState.RUNNING);
            });
        }

        protected override void OnStop()
        {
        }

        protected override NodeState OnUpdate()
        {
            bool stillRunning = false;
            for (int i = 0; i < childrenLeftToExecute.Count(); ++i)
            {
                if (childrenLeftToExecute[i] == NodeState.RUNNING)
                {
                    var status = nodes[i].UpdateNode();
                    if (status == NodeState.FAILURE)
                    {
                        AbortRunningChildren();
                        return NodeState.FAILURE;
                    }

                    if (status == NodeState.RUNNING)
                    {
                        stillRunning = true;
                    }

                    childrenLeftToExecute[i] = status;
                }
            }

            return stillRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        }

        void AbortRunningChildren()
        {
            for (int i = 0; i < childrenLeftToExecute.Count(); ++i)
            {
                if (childrenLeftToExecute[i] == NodeState.RUNNING)
                {
                    nodes[i].Abort();
                }
            }
        }
    }

}

