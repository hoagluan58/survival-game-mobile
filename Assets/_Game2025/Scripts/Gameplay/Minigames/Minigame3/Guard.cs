using SquidGame.LandScape.Game;
using System;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class Guard : MonoBehaviour
    {
        [SerializeField] private BaseGuard _baseGuard;

        public BaseGuard BaseGuard => _baseGuard;

        public Guard PlayFireAnimation(Transform target, Action onStart, Action onComplete)
        {
            _baseGuard.PlayShootAnim().ShowLine(0.1f, target,
                () =>
                {
                    onStart?.Invoke();
                }).OnShootCompleted(() => onComplete?.Invoke());        
            return this;
        }

        public Guard LookAtTarget(Transform target)
        {
            transform.LookAt(target);
            return this;
        }
    }
}
