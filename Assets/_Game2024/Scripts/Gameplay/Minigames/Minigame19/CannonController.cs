using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace SquidGame.Minigame19
{
    public class CannonController : MonoBehaviour
    {
        [SerializeField] private Transform _firePoint;

        // Refs
        private ObjectPool<CannonBullet> _bulletPool;

        // Model
        private const float SHOOT_COOLDOWN = 1f;
        private const float HIDE_COOLDOWN = 2.5f;
        private float _shootCooldownTimer;
        private bool _isShoot;

        public void Init(ObjectPool<CannonBullet> bulletPool)
        {
            _bulletPool = bulletPool;
        }

        private void OnEnable()
        {
            _isShoot = false;
            _shootCooldownTimer = 0;
            transform.DOPunchScale(Vector3.one * 0.1f, 0.5f);
        }

        private void FixedUpdate()
        {
            _shootCooldownTimer += Time.fixedDeltaTime;
            if (_shootCooldownTimer >= SHOOT_COOLDOWN && !_isShoot)
            {
                _isShoot = true;
                Shoot();
            }

            if (_shootCooldownTimer >= HIDE_COOLDOWN && _isShoot)
            {
                gameObject.SetActive(false);
            }
        }

        private void Shoot()
        {
            var bullet = _bulletPool.Get();
            var force = -_firePoint.right;
            force *= 5f;

            bullet.transform.position = _firePoint.position;
            bullet.OnSpawn(_bulletPool, force);
        }
    }
}