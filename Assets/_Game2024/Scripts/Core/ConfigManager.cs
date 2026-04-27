using NFramework;
using SquidGame.Config;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.Core
{
    public class ConfigManager : SingletonMono<ConfigManager>
    {
        [SerializeField] private MinigameConfigSO _minigameConfig;
        [SerializeField] private HatConfigSO _playerSkinConfig;
        [SerializeField] private DailyRewardConfigSO _dailyRewardConfig;

        public Dictionary<int, MinigameConfig> MinigameConfig => _minigameConfig.Config;
        public HatConfigSO PlayerSkinConfig => _playerSkinConfig;
        public Dictionary<int, DailyRewardConfig> DailyRewardConfig => _dailyRewardConfig.Config;

        public void Init()
        {
            _minigameConfig.Init();
            _playerSkinConfig.Init();
            _dailyRewardConfig.Init();
        }
    }
}
