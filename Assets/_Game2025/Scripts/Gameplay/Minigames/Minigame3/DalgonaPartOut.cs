using Sirenix.OdinInspector;
using SquidGame.LandScape.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class DalgonaPartOut : MonoBehaviour
    {
        [SerializeField] private float _dirrr = 1;

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

            transform.position = transform.position + dir * 0.25f * _dirrr;
            //transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }

    }
}
