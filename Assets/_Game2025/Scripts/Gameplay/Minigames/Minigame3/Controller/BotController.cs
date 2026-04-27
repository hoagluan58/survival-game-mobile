using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class BotController : MonoBehaviour
    {
        //[SerializeField] private BotBehavior _prefab;
        [SerializeField] private List<BotBehavior> _botList;
        //[SerializeField] private int _botCount = 19;
        //[SerializeField] private Transform _botHolder;

        public void Init(MinigameController minigameController)
        {
            //SpawnBot();
            foreach (var bot in _botList)
            {
                bot.Init(minigameController);
                bot.StartBot();
            }
        }

        public void SetActiveAllBot(bool isActive)
        {
            foreach (var bot in _botList)
            {
                bot.gameObject.SetActive(isActive);
            }
        }

        [Button]
        public void LoadAllBot()
        {
            _botList = GetComponentsInChildren<BotBehavior>(true).ToList();
        }

        //private void SpawnBot()
        //{
        //    for (int i = 0; i < _botCount; i++)
        //    {
        //        BotBehavior botBehavior = Instantiate(_prefab, _botHolder);
        //        botBehavior.gameObject.SetActive(true);
        //        _botList.Add(botBehavior);
        //    }
        //}
    }
}
