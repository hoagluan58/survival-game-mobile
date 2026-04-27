using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.BehaviourTree
{
    public abstract class DecoratorNode : LogicNode
    {
        [SerializeField] protected Node child;
        public Node Child => child;

        protected override void GetAllChildNode()
        {
            child = transform.GetChild(0).GetComponent<Node>();
            Debug.Log("Reset " + nameof(child) + " in " + GetType().Name);
        }

        public override void Init(LogicNode parent)
        {
            base.Init(parent);
            child.Init(this);
        }
        public override T GetNode<T>()
        {
            T neededNode = child as T;
            if (neededNode != null)
                return neededNode;
            return null;
        }
    }

}

