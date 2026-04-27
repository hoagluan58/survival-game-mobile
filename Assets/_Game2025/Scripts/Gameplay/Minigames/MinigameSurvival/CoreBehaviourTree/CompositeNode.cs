using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.BehaviourTree
{
    public abstract class CompositeNode : LogicNode
    {
        [SerializeField] protected List<Node> nodes = new List<Node>();
        public List<Node> Nodes => nodes;

        protected override void GetAllChildNode()
        {
            nodes.Clear();
            foreach (Transform child in transform)
            {
                nodes.Add(child.GetComponent<Node>());
            }
            Debug.Log("Reset " + nameof(nodes) + " in " + GetType().Name);
        }

        public override void Init(LogicNode parent)
        {
            base.Init(parent);
            foreach (Node node in nodes)
            {
                node.Init(this);
            }
        }
        public override T GetNode<T>()
        {
            foreach (Node node in nodes)
            {
                T neededNode = node as T;
                if (neededNode != null)
                    return neededNode;
            }
            return null;
        }
    }

}

