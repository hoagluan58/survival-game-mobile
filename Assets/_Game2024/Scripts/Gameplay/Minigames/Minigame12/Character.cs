using Animancer;
using UnityEngine;

namespace SquidGame.Minigame12
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private Transform _handParent;
        [SerializeField] private bool _isPlayer;

        [Header("ANIMATION")]
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private AnimationClip _idleClip;
        [SerializeField] private AnimationClip _throwClip;

        public void Init(bool isCharacterTurn)
        {
            _animancer.Play(_idleClip);
        }

        public void PutInHand(Transform target)
        {
            target.SetParent(_handParent);
            target.localPosition = Vector3.zero;
            target.localEulerAngles = Vector3.zero;
        }

        public void Throw()
        {
            _animancer.Play(_throwClip);
        }
    }
}
