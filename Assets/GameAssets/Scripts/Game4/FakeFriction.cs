using UnityEngine;

namespace Game4
{
    public class FakeFriction : MonoBehaviour
    {
        [SerializeField] private float _friction = 1f;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (_rigidbody != null && _rigidbody.velocity.magnitude <= 1f)
            {
                if (!_rigidbody.isKinematic)
                {
                    _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, Vector3.zero, Time.deltaTime * _friction);
                    _rigidbody.angularVelocity = Vector3.Lerp(_rigidbody.angularVelocity, Vector3.zero, Time.deltaTime * _friction);
                }
            }
        }
    }
}
