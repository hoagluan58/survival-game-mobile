using UnityEngine;

namespace SquidGame.Minigame18
{
    public class Obstacle : MonoBehaviour
    {
        private float _speed;
        private bool _isActive;

        public void Init(float speed)
        {
            _speed = speed;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        private void FixedUpdate()
        {
            if (!_isActive) return;
            transform.Rotate(Vector3.up, _speed * Time.fixedDeltaTime);
        }
    }
}