using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame1
{
    public class PlayerCollect : MonoBehaviour
    {
        [SerializeField] private TypeItemCollect _typeItem;
        public TypeItemCollect TypeItem => _typeItem;
    }


    public enum TypeItemCollect
    {
        Start,
        Win,
        Obstacle
    }
}
