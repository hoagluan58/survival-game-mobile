using System;
using UnityEngine;

namespace SquidGame.Minigame20
{
    public class BotController : UnitBase
    {
        public void StartBot()
        {
            
        }


        public void OnStartMinigame()
        {
           
        }
    }

    public enum EBotState
    {
        None,
        Idle,
        Run,
        Hit,
    }
}