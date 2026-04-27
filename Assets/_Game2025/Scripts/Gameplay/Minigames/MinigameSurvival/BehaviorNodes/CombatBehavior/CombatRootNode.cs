using SquidGame.LandScape.BehaviourTree;
using SquidGame.LandScape.Minigame3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class CombatRootNode : Selector
    {
        private Bot _bot;
        public Bot Bot => _bot;        

        public void SetBot(Bot bot)
        {
            _bot = bot;
        }

       
    }
}
