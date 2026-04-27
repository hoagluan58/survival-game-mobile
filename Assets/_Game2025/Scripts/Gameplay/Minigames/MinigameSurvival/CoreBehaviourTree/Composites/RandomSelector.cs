using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.BehaviourTree
{
    public class RandomSelector : CompositeNode
    {
        protected int current;

        protected override void OnStart()
        {
            current = Random.Range(0, nodes.Count);
        }


        protected override NodeState OnUpdate()
        {
            var child = nodes[current];
            return child.UpdateNode();
        }
    }

}

