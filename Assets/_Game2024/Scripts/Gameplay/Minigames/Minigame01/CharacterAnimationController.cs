using Animancer;
using NFramework;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace SquidGame.Gameplay
{
    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private SerializableDictionaryBase<EAnimStyle, ClipTransition> _animationClips;

        private BaseCharacter _baseCharacter;

        public AnimancerComponent Animancer => _animancer;

        public void Init(BaseCharacter baseCharacter) => _baseCharacter = baseCharacter;

        public void PlayAnimation(params EAnimStyle[] anims)
        {
            if (anims.Length == 0) return;
            _animancer.Play(_animationClips[anims.RandomItem()]);
        }

        public void PlayAnimation(params ClipTransition[] anims)
        {
            if (anims.Length == 0) return;
            _animancer.Play(anims.RandomItem());
        }

        public void PlayAnimation(EAnimStyle anim)
        {
            _animancer.Play(_animationClips[anim]);
            if (anim == EAnimStyle.Die)
            {
                _baseCharacter.ToggleGreyScale(true);
            }
        }

        public void StopAnimation()
        {
            _animancer.Stop();
            _animancer.Animator.playableGraph.Stop();
        }
    }

    public enum EAnimStyle
    {
        Idle,
        Run,
        Victory_1,
        Victory_2,
        Victory_3,
        Die,
        Jump,
        Throw,
        StandStill_1,
        StandStill_2,
        StandStill_3,
        Fall,
        IdleGun,
        RunGun,
    }
}