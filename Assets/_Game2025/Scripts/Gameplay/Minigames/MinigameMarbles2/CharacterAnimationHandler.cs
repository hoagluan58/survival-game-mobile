using NFramework;
using SquidGame.LandScape.Game;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.MinigameMarblesVer2
{
    public class CharacterAnimationHandler : MonoBehaviour
    {
        [Header("ANIMATOR")]
        [SerializeField] private BaseCharacter _model;
        [SerializeField] private CharacterAnimator _animator;

        public void Throw(UnityAction onStart)
        {
            _animator.PlayAnimation(EAnimStyle.Throw).Events.OnEnd ??= () => _animator.PlayAnimation(EAnimStyle.Idle);
            this.InvokeDelay(0.7f,() => onStart?.Invoke());
        }


        public void Died()
        {
            _animator.PlayAnimation(EAnimStyle.Die);
            _model.ToggleGreyScale(true);
        }

        public void Dance()
        {
            _animator.PlayAnimation(EAnimStyle.Victory_1,EAnimStyle.Victory_2,EAnimStyle.Victory_3);
        }


        public void Stand()
        {
            _animator.PlayAnimation(EAnimStyle.Idle);
            _model.ToggleGreyScale(false);
        }
    }
}
