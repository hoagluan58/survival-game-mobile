using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Lobby
{
    public class Coin : MonoBehaviour
    {
        MeshCollider _meshCollider;


        private void Awake()
        {
            _meshCollider = GetComponent<MeshCollider>();
        }

        private void Update()
        {
            if (transform.localPosition.y <= -6.5f)
            {
                _meshCollider.enabled = true;
                enabled = false;
            }
        }
    }
}
