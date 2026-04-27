using System;
using SquidGame.Core;
using SquidGame.LandScape;
using UnityEngine;

namespace Game4
{
    public class EnemyBall : MonoBehaviour
    {
        private Game4Control _controller;
        private Rigidbody _rigidbody;
        private Collider _collider;
        private bool _isInsideHole;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        public void Init(Game4Control controller) => _controller = controller;

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
                _controller.EnemyBallInGoal++;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(Tag.Hole))
            {
                _isInsideHole = false;
                _controller.EnemyBallInGoal--;
            }
        }

        public void AddThrowForce(Vector3 valueForce)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(valueForce, ForceMode.Impulse);
            Invoke(nameof(StopBall), 3f);
        }

        private void StopBall()
        {
            if (_isInsideHole)
            {
                // GameSound.I.PlaySFX(Define.SoundPath.SFX_LOSING);
            }
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
        }
    }
}