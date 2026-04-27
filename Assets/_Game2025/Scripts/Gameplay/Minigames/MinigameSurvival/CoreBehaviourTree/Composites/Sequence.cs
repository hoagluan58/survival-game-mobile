using UnityEngine;

namespace SquidGame.LandScape.BehaviourTree
{
    public class Sequence : CompositeNode
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
                        continue;
                    case NodeState.FAILURE:
                        nodeState = NodeState.FAILURE;
                        return nodeState;
                    case NodeState.RUNNING:
                        nodeState = NodeState.RUNNING;
                        return nodeState;
                }
            }
            nodeState = NodeState.SUCCESS;
            return nodeState;        
        }

    }

}
