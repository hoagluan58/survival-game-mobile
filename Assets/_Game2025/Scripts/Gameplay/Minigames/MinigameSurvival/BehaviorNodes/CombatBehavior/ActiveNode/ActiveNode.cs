using SquidGame.LandScape.BehaviourTree;

namespace SquidGame.LandScape.Survival
{
    public class ActiveNode : Selector
    {

        private IInfomation _currentOpponent;
        public IInfomation CurrentOpponent => _currentOpponent;

        private CombatRootNode _rootNode => parent as CombatRootNode;
        private Bot _bot => _rootNode.Bot;

        public override void Init(LogicNode parent)
        {
            base.Init(parent);
            foreach (CombatAbstractNode node in nodes)
            {
                node.SetBot(_bot);
            }
        }

        public void SetOpponent(IInfomation opponent)
        {
            _currentOpponent = opponent;
        }
    }
}
