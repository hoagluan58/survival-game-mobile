using SquidGame.LandScape.BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class FindWeaponRootNode : Selector
    {
        private Bot _bot;
        private SavePosition _savePosition;

        public Bot Bot => _bot;
        public SavePosition SavePosition => _savePosition;

        public void SetBot(Bot bot)
        {
            _bot = bot;
        }

        public void SetSavePosition(SavePosition savePosition)
        {
            _savePosition = savePosition;
        }

    }
}
