using Game1;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Minigame1;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SquidGame.LandScape.Minigame1 {

    public class BotManager : MonoBehaviour
    {
        [SerializeField] private List<Bot> _bots;
        [SerializeField] private EnviromentHandler _enviromentHandler;

        public EnviromentHandler Enviroment => _enviromentHandler;

        public List<Bot> Bots => _bots;

        public void Init(MinigameController controller)
        {
            foreach (var bot in _bots)
            {
                bot.Init(controller, this);
            }
        }

        public void Stop()
        {
            for (int i = 0; i < _bots.Count; i++)
            {
                _bots[i].StopGame();
            }
        }

        public List<Bot> PickRandomeAliveBots(int count)
        {
            var aliveBots = _bots.Where(x => !x.IsDie && !x.IsWin).ToList();
            if (aliveBots.Count < count)
            {
                return aliveBots;
            }
            return aliveBots.Take(count).ToList();
        }


        public void StartGame()
        {
            foreach (var item in _bots)
            {
                item.OnStart();
            }
        }

        internal void Revive()
        {
            foreach (var item in _bots)
            {
                item.OnRevive();
            }
        }
    }

}
