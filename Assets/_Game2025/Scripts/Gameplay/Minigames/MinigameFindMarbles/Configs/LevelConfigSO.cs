using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.MinigameFindMarbles
{
    public class LevelConfigSO : ScriptableObject
    {
        public List<LevelConfig> LevelConfig;

        public LevelConfig GetConfig(int level)
            => LevelConfig.Find(l => l.Level == level) ?? LevelConfig[LevelConfig.Count - 1];
    }

    [System.Serializable]
    public class LevelConfig
    {
        public int Level;
        public float ShuffleInterval;
    }
}
