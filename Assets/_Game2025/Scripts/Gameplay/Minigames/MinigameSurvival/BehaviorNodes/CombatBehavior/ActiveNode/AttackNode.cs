using NFramework;
using SquidGame.LandScape.BehaviourTree;
using SquidGame.LandScape.Minigame3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class AttackNode : CombatAbstractNode
    {
        [SerializeField] private float _minTime = 0.25f;
        [SerializeField] private float _maxTime = 0.35f;

        private float _delayTime;
        private Timer _delayFromAttackToIdleTimer = new Timer();

        public override void Init(LogicNode parent)
        {
            base.Init(parent);
        }        

        protected override NodeState OnUpdate()
        {
            if (ActiveNode.CurrentOpponent == null) return NodeState.FAILURE;
            if (bot.IsInState(bot.AttackState)) return NodeState.SUCCESS;
            if (!IsInRange()) return NodeState.FAILURE;
            if (!_delayFromAttackToIdleTimer.CheckTimer()) return NodeState.SUCCESS;            
            bot.AttackState.SetOnCompleteAttack(() =>
            {
                RandomDelayTime();
                _delayFromAttackToIdleTimer.SetCooldownTime(_delayTime);
            });
            bot.BotDamagedState.SetOnDone(() =>
            {                
                _delayFromAttackToIdleTimer.CancelTimer();
            });
            bot.CombatHandler.SetCanPlaySound(ActiveNode.CurrentOpponent.IsPlayer());
            bot.Agent.SetDestination(transform.position);
            bot.ChangeState(bot.AttackState);
            bot.Agent.transform.LookAt(ActiveNode.CurrentOpponent.GetTransform());
            return NodeState.SUCCESS;
        }

        private void RandomDelayTime()
        {
            _delayTime = Random.Range(_minTime, _maxTime);
        }
    }
}
