using System;
using SquidGame.Core;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.Minigame4
{
    public class EnemyBall : MonoBehaviour
    {

        [SerializeField] private GameObject _dustFx;
        private UnityAction<bool> _onCompleted;

        private Rigidbody _rigidbody;
        private Collider _collider;
        private bool _isInsideHole;

        public void Initialize()
        {
            _rigidbody.isKinematic = true;
            _collider.enabled = true;
        }


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _dustFx.SetActive(true);
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


        public EnemyBall AddThrowForce(Vector3 valueForce , float lifetime = 3)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(valueForce, ForceMode.Impulse);
            Invoke(nameof(StopBall), lifetime);
            return this;
        }


        public EnemyBall OnCompleted(UnityAction<bool> onCompleted)
        {
            _onCompleted = onCompleted;
            return this;
        }


        private void StopBall()
        {
            if (_isInsideHole)
            {
                // GameSound.I.PlaySFX(Define.SoundPath.SFX_LOSING);
            }
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
            _onCompleted?.Invoke(_isInsideHole);
        }
    }
}