using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6.Ddakji
{
    public class DdakjiGameConfigSO : ScriptableObject
    {
        [Header("GAME")]
        public float PlayTime;
        public float MinX;
        public float FlipForce;

        [Header("UI")]
        public float MinTargetPos;
        public float MaxTargetPos;
    }
}
