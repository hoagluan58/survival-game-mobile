using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace SquidGame.Minigame15
{
    public class Guard : MonoBehaviour
    {
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private AnimationClip _idleClip;
        [SerializeField] private AnimationClip _shootClip;
        
        public void PlayShootAnimation(Side side)
        {
            
        }
    }
}
