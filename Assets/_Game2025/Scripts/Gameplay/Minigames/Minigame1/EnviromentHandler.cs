using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame1
{
    public class EnviromentHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _blockStartGo;
        [SerializeField] private Transform _boundStartGameMin;
        [SerializeField] private Transform _boundStartGameMax;


        public Vector2 GetRangeXStartgame() => new Vector2(_boundStartGameMin.position.x , _boundStartGameMax.position.x);
        public Vector2 GetRangeZStartgame() => new Vector2(_boundStartGameMin.position.z, _boundStartGameMax.position.z);

        public Vector2 GetRangeXEndgame() => new Vector2(_boundStartGameMin.position.x, _boundStartGameMax.position.x);
        public Vector2 GetRangeZEndgame() => new Vector2(_boundStartGameMin.position.z, _boundStartGameMax.position.z);

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
