using SquidGame.Core;
using SquidGame.LandScape;
using UnityEngine;

namespace Game4
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private GameObject _trail;

        private Rigidbody _rigidbody;
        private PlayerController _playerControl;
        private Game4Control _controller;
        private Collider _collider;
        private bool _isInsideHole;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        public void Init(Game4Control controller) => _controller = controller;

        public void ThrowBall(Vector3 valueForce, PlayerController playerControl)
        {
            _playerControl = playerControl;
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(valueForce);
            Invoke(nameof(StopBall), 3f);
        }

        private void StopBall()
        {
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
            CheckInGoal();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG04_MARBLE_DROP);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tag.Hole))
            {
                _isInsideHole = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(Tag.Hole))
            {
                _isInsideHole = false;
            }
        }

        private void CheckInGoal()
        {
            _controller.PlayerBallInGoal += _isInsideHole ? 1 : 0;
        }
    }
}