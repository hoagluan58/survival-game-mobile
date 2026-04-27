using NFramework;
using SquidGame.LandScape.Config;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Core
{
    public class GameConfig : SingletonMono<GameConfig>
    {
        [SerializeField] private MinigameConfigSO _minigameConfigSO;
        [SerializeField] private SeasonConfigSO _seasonConfigSO;
        [SerializeField] private HairConfigSO _hairConfigSO;

        public SeasonConfigSO SeasonConfigSO => _seasonConfigSO;
        public Dictionary<int, MinigameConfig> MinigameConfigs => _minigameConfigSO.Configs;
        public Dictionary<int, HairConfig> HairConfigs => _hairConfigSO.Configs;

        public void Init()
        {
            _minigameConfigSO.Init();
            _seasonConfigSO.Init();
            _hairConfigSO.Init();
        }
    }
}
