using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.BehaviourTree
{
    public class BehaviorTree
    {
        public static void Traverse(Node node, System.Action<Node> visiter)
        {
            if (node)
            {
                visiter.Invoke(node);
                var children = GetAllChildren(node);
                children.ForEach((n) => Traverse(n, visiter));
            }
        }

        public static List<Node> GetAllChildren(Node parent)
        {
            List<Node> children = new List<Node>();

            if (parent is DecoratorNode decorator && decorator.Child != null)
            {
                children.Add(decorator.Child);
            }


            if (parent is CompositeNode composite)
            {
                return composite.Nodes;
            }

            return children;
        }
    }

}

