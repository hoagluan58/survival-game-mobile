using UnityEngine;

namespace SquidGame.LandScape.Minigame6.Ddakji
{
    public class Ddakji : MonoBehaviour
    {
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private DdakjiGameConfigSO _configSO;
        [SerializeField] private Rigidbody _rigidbody;

        private bool _isFlip;
        public bool IsFlip => _isFlip;

        public bool IsWithinCollider(Vector3 position)
        {
            var boundsY = _collider.bounds.center.y;
            position.y = boundsY;
            return _collider.bounds.Contains(position);
        }

        public void Flip(bool isFlip)
        {
            var rndDirection = new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), 1f, UnityEngine.Random.Range(-0.2f, 0.2f)).normalized;
            _rigidbody.AddForce(rndDirection * (isFlip ? _configSO.FlipForce : 0.1f), ForceMode.Impulse);

            if (isFlip)
            {
                var rndFlipDirection = UnityEngine.Random.value > 0.5f ? Vector3.left : Vector3.right;
                var rndTorque = UnityEngine.Random.Range(180f, 270f);
                _rigidbody.AddTorque(rndFlipDirection * rndTorque, ForceMode.Impulse);
            }
        }

        public void SetFlipOnCollision(bool value) => _isFlip = value;
    }
}
