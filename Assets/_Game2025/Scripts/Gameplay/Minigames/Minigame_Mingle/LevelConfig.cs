using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "SquidGame/MinigameMingle/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public List<LevelContent> LevelContents;

        public LevelContent GetLevelContent(int level)
        {
            return LevelContents[level];
        }
    }

    [System.Serializable]
    public class LevelContent
    {
        public int PeopleCountMin;
        public int PeopleCountMax;
        public int RoomCount;
        public int Time;
    }

}
