using SquidGame.Core;
using SquidGame.LandScape;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.Minigame03
{
    public class DalgonaPartOut : MonoBehaviour
    {
        [SerializeField] private int _dirrr = 1;

        private bool _isBreak;
        private SphereCollider _sphereCollider;

        public bool IsBreak => _isBreak;

        private void Awake()
        {
            _sphereCollider = GetComponent<SphereCollider>();
            _isBreak = false;
        }

        public void Break()
        {
            _isBreak = true;
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG03_BREAK_PART_OUT);
            // Vector3 dir = -transform.localPosition;
            Vector3 dir = _sphereCollider.center;
            dir.Normalize();

            transform.position = transform.position + dir * 0.125f * _dirrr;
        }
    }
}
