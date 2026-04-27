using Animancer;
using UnityEngine;

namespace SquidGame.Minigame03
{
    public class MG03Bot01 : MonoBehaviour
    {
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private AnimationClip _idleClip;

        private void OnEnable() => _animancer.Play(_idleClip);
    }
}
