using System;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame16
{
    [CreateAssetMenu(fileName = "LightConfig" , menuName = "Game/Minigame16/LightConfig")]
    public class LightConfig : ScriptableObject
    {
        public List<LightContent> Contents;

        public LightContent GetContentByLevel(int level)
        {
            return Contents[level >= Contents.Count ? ^1 : level];
        }
    }

    [System.Serializable]
    public class LightContent
    {
        public List<int> MaxLights; 
    }

}
