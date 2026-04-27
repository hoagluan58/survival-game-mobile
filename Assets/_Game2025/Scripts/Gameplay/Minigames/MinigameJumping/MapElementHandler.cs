using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Jumping
{
    public class MapElementHandler : MonoBehaviour
    {
        [SerializeField] private ObstacleRotate _obstacleRotate;
        [SerializeField] private List<BotController> _botControllers;

        public void Init(float intervalSwitchDirectionTime)
        {
            _obstacleRotate.Init(intervalSwitchDirectionTime);
        }

        public void OnStart()
        {
            _obstacleRotate.SetActive(true);
        }

        public void OnPlayerDead()
        {
            _obstacleRotate.SetActive(false);
            _botControllers.ForEach(bot => bot.OnPlayerDead());
        }

        public void OnPlayerRevive()
        {
            _obstacleRotate.SetActive(true, 2f);
            _botControllers.ForEach(bot => bot.OnPlayerRevive());
        }

        public void OnWin()
        {
            _obstacleRotate.OnWin();
        }
    }
}
