using Animancer;
using UnityEngine;

namespace SquidGame.Gameplay
{
    public class BaseGuard : MonoBehaviour
    {
        public ClipTransition IdleClip;
        public ClipTransition ShootClip;
        
        [HideInInspector] public AnimancerComponent Animancer;

        public void Start()
        {
            Animancer = GetComponentInChildren<AnimancerComponent>();
            Animancer.Play(IdleClip);
        }

        public void PlayShootAnim()
        {
            var state = Animancer.Play(ShootClip);
            state.Events.OnEnd += () => Animancer.Play(IdleClip);
        }
    }
}