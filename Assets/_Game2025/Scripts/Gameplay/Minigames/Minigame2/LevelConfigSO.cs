using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.LandScape.Minigame2
{
    public class LevelConfigSO : ScriptableObject
    {
        public List<LevelData> LevelDatas;

        public LevelData GetLevelConfig(int levelId)
        {
            var levelData = LevelDatas.Find(_ => _.Id == levelId);
            return levelData == null ? LevelDatas.Last() : levelData;
        }
    }

    [Serializable]
    public class LevelData
    {
        public int Id;
        public int Rows;
        public int Columns;
        public int PlayTime;
        public int BotNumber;

        private Vector3 _firstGlassPos;
        public Vector3 FirstGlassPos => _firstGlassPos;

        public void GetFirstGlassPosition(Vector3 data, Vector3 distanceBetweenRow) => _firstGlassPos = new(data.x, data.y, data.z - (distanceBetweenRow.z * (Rows - 1)));
    }
}
