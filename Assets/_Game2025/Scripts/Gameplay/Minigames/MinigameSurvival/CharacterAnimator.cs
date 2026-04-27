using Animancer;
using Redcode.Extensions;
using SquidGame.LandScape.Game;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using static SquidGame.LandScape.Game.CharacterBodySkin;

namespace SquidGame.LandScape.Survival
{
    public class CharacterAnimator : CharacterComponent
    {
        [SerializeField] AnimancerComponent _animancer;
        [SerializeField] private CharacterBodySkin _skin;
        [SerializeField] SerializedDictionary<WeaponType, SerializedDictionary<ANIMATION, AnimationInfor>> _animationClips;

        // [SerializeField] SerializedDictionary<Weapon, AnimationClip> _moveAnimations;
        // [SerializeField] SerializedDictionary<Weapon, AnimationClip> _jumpAnimations;
        // [SerializeField] SerializedDictionary<Weapon, AnimationClip> _hitAnimations;

        public AnimancerState PlayAnimation(WeaponType weapon, ANIMATION animation, FadeMode fadeMode = FadeMode.FixedSpeed, float fadeDuration = 0.2f)
        {
            if (_animationClips.ContainsKey(weapon))
            {
                if (_animationClips[weapon].ContainsKey(animation))
                {
                    _skin.ChangeFace(_animationClips[weapon][animation].Face);
                    return PlayAnimation(_animationClips[weapon][animation].AnimationClip, fadeMode, fadeDuration);
                }
            }
            return null;
        }

        public ANIMATION PlayDanceAnimation()
        {
            ANIMATION[] animList = new ANIMATION[3]
            {
                ANIMATION.Win_1,
                ANIMATION.Win_2,
                ANIMATION.Win_3,
            };
            ANIMATION winAnimation = animList.GetRandomElement();
            PlayAnimation(WeaponType.Default, winAnimation);
            return winAnimation;
        }

        AnimancerState PlayAnimation(AnimationClip animationClip, FadeMode fadeMode = FadeMode.FixedSpeed, float fadeDuration = 0.2f)
        {
            AnimancerState state = _animancer.Play(animationClip, fadeDuration, fadeMode);
            return state;
        }
    }

    [Serializable]
    public class AnimationInfor
    {
        public AnimationClip AnimationClip;
        public EFaceName Face;
    }

    public enum ANIMATION
    {
        Idle,
        Move,
        Jump,
        Damaged,
        Attack,
        Dead,
        Win_1,
        Win_2,
        Win_3,
    }
}
