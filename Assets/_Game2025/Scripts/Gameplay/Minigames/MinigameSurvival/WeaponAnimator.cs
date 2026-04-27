using System.Collections;
using System.Collections.Generic;
using Animancer;
using SquidGame.LandScape.Survival;
using UnityEngine;
using UnityEngine.Rendering;

namespace SquidGame.LandScape
{
    public class WeaponAnimator : MonoBehaviour
    {
        AnimancerComponent _animancer;
        [SerializeField] SerializedDictionary<WeaponType, SerializedDictionary<ANIMATION, AnimationClip>> _animationClips;

        // [SerializeField] SerializedDictionary<Weapon, AnimationClip> _moveAnimations;
        // [SerializeField] SerializedDictionary<Weapon, AnimationClip> _jumpAnimations;
        // [SerializeField] SerializedDictionary<Weapon, AnimationClip> _hitAnimations;

        public void PlayAnimation(WeaponType weapon, ANIMATION animation, FadeMode fadeMode = FadeMode.FixedSpeed, float fadeDuration = 0.2f)
        {
            if (_animationClips.ContainsKey(weapon))
            {
                if (_animationClips[weapon].ContainsKey(animation))
                {
                    PlayAnimation(_animationClips[weapon][animation], fadeMode, fadeDuration);
                }
            }
        }

        public void SetAnimancer(AnimancerComponent animancerComponent)
        {
            _animancer = animancerComponent;
        }

        void PlayAnimation(AnimationClip animationClip, FadeMode fadeMode = FadeMode.FixedSpeed, float fadeDuration = 0.2f)
        {
            _animancer.Play(animationClip, fadeDuration, fadeMode);
        }
    }
}
