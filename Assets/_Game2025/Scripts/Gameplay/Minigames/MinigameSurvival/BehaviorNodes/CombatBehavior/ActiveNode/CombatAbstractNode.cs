using SquidGame.LandScape.BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public abstract class CombatAbstractNode : Node
    {
        public ActiveNode ActiveNode => parent as ActiveNode;
        protected Bot bot;        

        public void SetBot(Bot bot)
        {
            this.bot = bot;
        }

        protected bool IsInRange()
        {
            return Vector3.Distance(bot.transform.position, ActiveNode.CurrentOpponent.GetTransform().position) < bot.AttackRadius;
        }

        protected List<IInfomation> GetListUserInfor()
        {
            List<IInfomation> userList = bot.BotController.SurvivalManager.Userlist;
            userList.Remove(bot);
            return userList;
        }
    }
}
