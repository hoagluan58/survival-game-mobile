using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.BehaviourTree
{
    public abstract class LogicNode : Node
    {


        public abstract T GetNode<T>() where T : Node;

    }

}

