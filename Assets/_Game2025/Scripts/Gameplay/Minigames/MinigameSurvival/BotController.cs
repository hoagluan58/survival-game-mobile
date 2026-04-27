using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class BotController : MonoBehaviour
    {
        [SerializeField] private Bot _botPrefab;
        [SerializeField] private Bot _botHunterPrefab;
        [SerializeField] private LayerMask _obstacleLayer;
        [SerializeField] private float _hunterRatio = 0.3f;
        private List<SavePosition> _spawnedPositionList = new List<SavePosition>();
        private List<Bot> _botList = new List<Bot>();

        private SurvivalManager _survivalManager;
        public SurvivalManager SurvivalManager => _survivalManager;

        public int GetBotCount() => _botList.Count;


        public void Init(SurvivalManager survivalManager)
        {
            _survivalManager = survivalManager;
        }

        public int SpawnAllBot(int level = 1)
        {
            int maxBotAmount = ConfigController.I.BotSpawnSO.GetConfig(level).Amount;
            int hunterAmount = GetHunterAmount(maxBotAmount);
            for (int i = 0; i < maxBotAmount; i++)
            {
                SpawnBot(ref hunterAmount);
            }

            return maxBotAmount;
        }

        private int GetHunterAmount(int botAmount)
        {
            return (int)(botAmount * _hunterRatio);
        }

        public void SpawnBot(ref int hunterAmount)
        {
            Vector3 position = PositionController.I.GetRandomPosition(_obstacleLayer, _spawnedPositionList);
            Bot botPrefab = hunterAmount > 0 ? _botHunterPrefab : _botPrefab;
            Bot bot = Instantiate(botPrefab, position, Quaternion.identity);
            bot.Init(this);
            _botList.Add(bot);
            hunterAmount--;
        }


        public void OnBotDie(Bot bot)
        {
            _botList.Remove(bot);
            _survivalManager.UpdateAmountText();

            if (_botList.Count == 0)
            {
                _survivalManager.WinLevel();
            }
        }

        public List<IInfomation> GetAllInformation()
        {
            List<IInfomation> list = new List<IInfomation>();
            foreach (Bot bot in _botList)
            {
                list.Add(bot);
            }
            return list;
        }
    }
}
