using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game1
{
    public class EnviromentHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _blockStartGo;

        public void Init()
        {
            _blockStartGo.SetActive(true);
        }

        public void StartGame()
        {
            _blockStartGo.SetActive(false);
        }
    }
}
