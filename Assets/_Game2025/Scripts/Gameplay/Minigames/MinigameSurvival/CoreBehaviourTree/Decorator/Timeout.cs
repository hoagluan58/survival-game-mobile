using SquidGame.LandScape.Minigame3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.BehaviourTree
{
    public class Timeout : DecoratorNode
    {
        [SerializeField] protected float duration = 1.0f;
        protected Timer timer = new Timer();

        protected override void OnStart()
        {        
            timer.SetCooldownTime(duration);
        }
        protected override NodeState OnUpdate()
        {
            if (timer.CheckTimer())
            {
                return NodeState.FAILURE;
            }

            return child.UpdateNode();
        }
    
    }

}

