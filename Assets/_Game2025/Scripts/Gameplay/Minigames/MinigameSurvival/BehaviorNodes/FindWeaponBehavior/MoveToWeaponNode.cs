using SquidGame.LandScape.BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class MoveToWeaponNode : Node
    {
        [SerializeField] private float _stopDistance = 0.3f;

        private FindWeaponRootNode _rootNode => parent as FindWeaponRootNode;
        private Bot _bot => _rootNode.Bot;

        protected override NodeState OnUpdate()
        {
            if (_rootNode.SavePosition == null) return NodeState.FAILURE;
            if (IsInRange()) return NodeState.SUCCESS;
            if(_rootNode.SavePosition.IsRemoved) return NodeState.FAILURE;
            if(!_bot.IsInState(_bot.MoveState))
                _bot.ChangeState(_bot.MoveState);
            _bot.Agent.SetDestination(_rootNode.SavePosition.Position);
            return NodeState.SUCCESS;
        }

        protected bool IsInRange()
        {
            return Vector3.Distance(_bot.transform.position, _rootNode.SavePosition.Position) < _stopDistance;
        }
    }
}
