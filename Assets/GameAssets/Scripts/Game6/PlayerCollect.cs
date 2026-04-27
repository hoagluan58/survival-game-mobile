using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game6
{
    public class PlayerCollect : MonoBehaviour
    {
        [SerializeField] private TypeItemCollect _typeItem;
        public TypeItemCollect TypeItem => _typeItem;
    }


    public enum TypeItemCollect
    {
        Candy,
        Thorns,
        StopBar
    }
}