namespace SquidGame.LandScape.BehaviourTree
{
    public class Repeat : DecoratorNode
    {
        public bool restartOnSuccess = true;
        public bool restartOnFailure = false;



        protected override NodeState OnUpdate()
        {
            switch (child.UpdateNode())
            {
                case NodeState.RUNNING:
                    break;
                case NodeState.FAILURE:
                    if (restartOnFailure)
                    {
                        return NodeState.RUNNING;
                    }
                    else
                    {
                        return NodeState.FAILURE;
                    }
                case NodeState.SUCCESS:
                    if (restartOnSuccess)
                    {
                        return NodeState.RUNNING;
                    }
                    else
                    {
                        return NodeState.SUCCESS;
                    }
            }
            return NodeState.RUNNING;
        }
    }

}

