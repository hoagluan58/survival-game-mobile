using DG.Tweening;
using SquidGame.LandScape.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.MinigameMarblesVer2
{
    public class Marble : MonoBehaviour
    {
        [SerializeField] private float _delayStop = 5f;
        [SerializeField] private ParticleSystem _dustFx;
        private Rigidbody _rigidbody;
        private Tween _autoStopTween;

        private void OnDisable()
        {
            _autoStopTween.Kill();
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Throw(Vector3 spawnPosition, Vector3 direction, float force, UnityAction onCompleted)
        {
            gameObject.SetActive(true);
            transform.position = spawnPosition;
            _rigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
            _autoStopTween = DOVirtual.DelayedCall(_delayStop, () =>
            {
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.velocity = Vector3.zero;
                onCompleted?.Invoke();
            });
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG_MARBLES_HIT_WALL);
            }
            if (other.gameObject.CompareTag("Ground"))
            {
                _dustFx.Play();
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG04_MARBLE_DROP);
            }
        }
    }
}
