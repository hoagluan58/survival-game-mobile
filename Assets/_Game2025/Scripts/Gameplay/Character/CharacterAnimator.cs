using Animancer;
using NFramework;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace SquidGame.LandScape.Game
{
    public class CharacterAnimator : CharacterComponent
    {
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private CharacterAnimationConfigSO _configSO;

        private CharacterBodySkin _skin;

        public override void Init(BaseCharacter character)
        {
            base.Init(character);
            _skin = character.GetCom<CharacterBodySkin>();
        }

        public AnimancerState PlayAnimation(EAnimStyle style/* , float fadeDuration = 0.2f, FadeMode fadeMode = FadeMode.FixedSpeed */)
        {
            var cfg = _configSO.GetConfig(style);
            if (cfg == null) return null;

            // _skin.ChangeFace(cfg.Face);
            return _animancer.Play(cfg.Clip);
        }

        public AnimancerState PlayAnimation(EAnimStyle style, float fadeDuration = 0.2f, FadeMode fadeMode = FadeMode.FixedSpeed)
        {
            var cfg = _configSO.GetConfig(style);
            if (cfg == null) return null;

            // _skin.ChangeFace(cfg.Face);
            return _animancer.Play(cfg.Clip, fadeDuration, fadeMode);
        }

        public AnimancerState PlayAnimation(AnimationClip animationClip, float fadeDuration = 0.2f, FadeMode fadeMode = FadeMode.FixedSpeed)
        {
            return _animancer.Play(animationClip, fadeDuration, fadeMode);
        }

        public AnimancerState PlayAnimation(float fadeDuration, params EAnimStyle[] styles) => PlayAnimation(styles.RandomItem(), fadeDuration);

        public AnimancerState PlayAnimation(params EAnimStyle[] styles) => PlayAnimation(styles.RandomItem());

        public float GetLength(EAnimStyle style)
        {
            var config = _configSO.GetConfig(style);
            if (config == null) return 0;

            return config.Clip.length;
        }
    }
}
