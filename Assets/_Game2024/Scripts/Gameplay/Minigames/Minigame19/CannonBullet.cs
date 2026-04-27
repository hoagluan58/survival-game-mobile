using UnityEngine;
using UnityEngine.Pool;

namespace SquidGame.Minigame19
{
    public class CannonBullet : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private ObjectPool<CannonBullet> _pool;
        private float _aliveTime;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        private void FixedUpdate()
        {
            _aliveTime += Time.fixedDeltaTime;
            if (_aliveTime >= 3f)
            {
                _pool.Release(this);
            }
        }


        public void OnSpawn(ObjectPool<CannonBullet> pool, Vector3 force)
        {
            _pool = pool;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(force, ForceMode.Impulse);
            _aliveTime = 0;
        }
    }
}