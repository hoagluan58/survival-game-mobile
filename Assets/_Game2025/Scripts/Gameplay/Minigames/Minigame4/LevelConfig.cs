using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame4
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Minigame4/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public List<LevelContent> Levels;

        public LevelContent LoadConfigLevel(int id)
        {
            var content = Levels.Find(x=> x.IdLevel == id);
            if (content == null)
                return Levels[^1];
            return content;
        }
    }


    [System.Serializable]
    public class LevelContent
    {
        public int IdLevel;
        [Range(0, 1)] public float SuccessRate;
    }
}
