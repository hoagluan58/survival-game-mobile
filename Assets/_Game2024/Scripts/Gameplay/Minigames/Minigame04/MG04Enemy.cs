using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.Minigame04
{
    public class MG04Enemy : MonoBehaviour
    {
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private AnimationClip _gunIdleClip;

        private void OnEnable()
        {
            _animancer.Play(_gunIdleClip);
        }
    }
}
