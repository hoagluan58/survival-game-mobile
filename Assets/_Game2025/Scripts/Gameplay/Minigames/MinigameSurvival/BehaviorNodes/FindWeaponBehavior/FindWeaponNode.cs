using SquidGame.LandScape.BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class FindWeaponNode : Node
    {
        private FindWeaponRootNode _rootNode => parent as FindWeaponRootNode;
        private Bot _bot => _rootNode.Bot;

        protected override NodeState OnUpdate()
        {
            if (WeaponController.I.TryGetNeareastSavePosition(transform, out SavePosition savePosition))
            {
                _rootNode.SetSavePosition(savePosition);
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }
    }
}
