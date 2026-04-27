using DG.Tweening;
using SquidGame.Core;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.Minigame4
{
    public class Ball : MonoBehaviour
    {
        public UnityAction OnEnterHoldAction;

        [SerializeField] private GameObject _trail;
        [SerializeField] private GameObject _dustFx;
        private Rigidbody _rigidbody;
        private Collider _collider;
        private UnityAction<bool> _onCompleted;
        
        private bool _isInsideHole;
        private bool _startCheck;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        private void Update()
        {
            if (_startCheck) {

                if (IsStopped(_rigidbody,0.1f))
                {
                    _startCheck = false;
                    _rigidbody.isKinematic = true;
                    _collider.enabled = false;
                    _onCompleted?.Invoke(_isInsideHole);
                }
            }
        }

        public void Initialized()
        {
            _rigidbody.isKinematic = true;
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one*1.5f,0.2f).SetEase(Ease.OutCubic);
        }


        public Ball ThrowBall(Vector3 valueForce, float lifetime = 5f)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(valueForce);
            DOVirtual.DelayedCall(0.1f, () => _startCheck = true);
            Invoke(nameof(StopBall), lifetime);
            return this;
        }


        public Ball OnCompleted(UnityAction<bool> onCompleted)
        {
            _onCompleted = onCompleted;
            return this; 
        }

        private  bool IsStopped(Rigidbody rb, float threshold = 0.01f)
        {
            return rb.velocity.sqrMagnitude < threshold * threshold && rb.angularVelocity.sqrMagnitude < threshold * threshold;
        }

        private void StopBall()
        {
            if (!_startCheck) return;
            _startCheck = false;
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
            _onCompleted?.Invoke(_isInsideHole);
        }

        private bool _isTriggerSound = false;
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Ground") && !_isTriggerSound)
            {
                _dustFx.SetActive(true);
                _isTriggerSound = true;
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG04_MARBLE_DROP);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tag.Hole))
            {
                OnEnterHoldAction?.Invoke();
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

    }
}