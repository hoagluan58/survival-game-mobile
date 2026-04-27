using SquidGame.Gameplay;
using UnityEngine;

namespace SquidGame.Minigame19
{
    public class BotController : MonoBehaviour
    {
        [SerializeField] private CharacterAnimationController _animator;

        private bool _isActive;
        private MapLimit _mapLimit;
        private Vector3 _currentTargetPosition;

        private bool _isAlive;

        public void Init(MapLimit mapLimit)
        {
            _mapLimit = mapLimit;
            _currentTargetPosition = transform.position;
            _animator.PlayAnimation(EAnimStyle.Idle);
            _isAlive = true;
        }

        private void Update()
        {
            if (!_isActive) return;

            // Move and look at _currentTargetPosition
            transform.position = Vector3.MoveTowards(transform.position, _currentTargetPosition, 3f * Time.deltaTime);
            transform.LookAt(_currentTargetPosition);

            if (Vector3.Distance(transform.position, _currentTargetPosition) < 0.1f)
            {
                _currentTargetPosition = _mapLimit.MapCenterPosition + new Vector3(
                    Random.Range(-_mapLimit.MaxDistance, _mapLimit.MaxDistance),
                    0,
                    Random.Range(-_mapLimit.MaxDistance, _mapLimit.MaxDistance));
            }
        }

        public void SetActive(bool isActive)
        {
            if (!_isAlive) return;

            _isActive = isActive;
            _animator.PlayAnimation(_isActive ? EAnimStyle.Run : EAnimStyle.Idle);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isAlive) return;
            if (!_isActive) return;
            if (other.gameObject.CompareTag("Dead_Zone")) Die();
        }

        private void Die()
        {
            if (!_isAlive) return;
            SetActive(false);
            _animator.PlayAnimation(EAnimStyle.Die);
            _isAlive = false;
        }
    }
}