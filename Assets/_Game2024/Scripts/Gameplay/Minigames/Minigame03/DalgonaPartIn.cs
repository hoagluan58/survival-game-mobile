using SquidGame.Core;
using SquidGame.LandScape;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.Minigame03
{
    public class DalgonaPartIn : MonoBehaviour
    {
        [SerializeField] private int _dirrr = 1;
        private SphereCollider _sphereCollider;

        private void Awake()
        {
            _sphereCollider = GetComponent<SphereCollider>();
        }

        public void Break()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG03_BREAK_PART_IN);
            //Vector3 dir = -transform.localPosition;
            Vector3 dir = _sphereCollider.center;
            dir.Normalize();

            transform.position = transform.position + dir * 0.075f * _dirrr;
        }
    }
}
