using Redcode.Extensions;
using SquidGame.LandScape.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    public class GuardManager : MonoBehaviour
    {
        [SerializeField] private Guard _guardPrefab;
        [SerializeField] private List<Guard> _guards;
        

        public void Init()
        {
        }

        public void KillPlayer(Transform head)
        {
            var guard = _guards.GetRandomElement();
            guard.LookAt(head).PlayShootAnim().ShowLine(0.25f, head).ClearLine(0.45f);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
        }

        public Guard GetGuard()
        {
            return Instantiate(_guardPrefab);
        }
    }
}
