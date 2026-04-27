using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame1
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Minigame1/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public List<LevelContent> Levels;

        public LevelContent GetLevelContent(int index)
        {
            return index >= Levels.Count ? Levels[^1] : Levels[index];
        }
    }


    [System.Serializable]
    public class LevelContent
    {
        public bool IsFixedPitch;
        public int ObstacleLimit;
    }
}
