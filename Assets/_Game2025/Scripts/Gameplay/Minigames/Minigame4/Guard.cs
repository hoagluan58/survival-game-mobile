using SquidGame.LandScape.Game;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.Minigame4
{
    public class Guard : MonoBehaviour
    {
        [SerializeField] private bool _isShooting;
        [SerializeField] private BaseGuard _baseGuard;
        public bool IsShooting => _isShooting;


        public Guard PlayAnimationFire(Transform position, UnityAction onStartShoot = null)
        {
            _isShooting = true;
            _baseGuard.PlayShootAnim().ShowLine(0.75f, position, onStartShoot).OnShootCompleted(() => _isShooting = false);
            return this;
        }


        public Guard LookAtTarget(Transform target)
        {
            transform.LookAt(target);
            return this;
        }


        public Guard OnShootCompleted(UnityAction unityAction)
        {
            _baseGuard.OnShootCompleted(unityAction);
            return this;
        }

        public BaseGuard GetBaseGuard() => _baseGuard;



    }
}
