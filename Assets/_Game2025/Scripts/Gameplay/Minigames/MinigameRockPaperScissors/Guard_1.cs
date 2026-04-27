using Animancer;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class Guard_1 : MonoBehaviour
    {

        [SerializeField] private AnimancerComponent _animator;
        [SerializeField] private ClipTransition _idleClip;
        [SerializeField] private ClipTransition _shootClip;


        [Button]
        public void PlayIdleAnimation()
        {
            _animator.Play(_idleClip);
        }

        [Button]
        public void PlayShootAnimation()
        {
            _animator.Play(_shootClip);
        }

    }
}
