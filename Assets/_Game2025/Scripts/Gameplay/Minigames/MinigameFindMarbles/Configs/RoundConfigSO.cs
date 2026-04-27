using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.MinigameFindMarbles
{
    public class RoundConfigSO : ScriptableObject
    {
        public List<RoundConfig> RoundConfigs;

        public RoundConfig GetConfig(int roundNumber) => RoundConfigs.Find(r => r.RoundNumber == roundNumber);
    }

    [System.Serializable]
    public class RoundConfig
    {
        public int RoundNumber;
        public int BowlCount;
        public Vector3 FirstBowlPos;
        public Vector3 BowlSpacing;
        public float ShuffleTime;
    }
}
