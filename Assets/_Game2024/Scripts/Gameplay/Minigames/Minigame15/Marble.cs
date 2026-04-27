using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using SquidGame.Core;
using SquidGame.LandScape;
using UnityEngine;

namespace SquidGame.Minigame15
{
    public class Marble : MonoBehaviour
    {
        private Side _side;
        private Rigidbody _rigidbody;
        private Tween _autoStopTween;

        public Side Side => _side;

        private void OnDisable()
        {
            _autoStopTween.Kill();
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Throw(Side side, Vector3 spawnPosition, Vector3 direction, float force)
        {
            gameObject.SetActive(true);
            _side = side;
            transform.position = spawnPosition;
            _rigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
            _autoStopTween = DOVirtual.DelayedCall(5f, () =>
            {
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.velocity = Vector3.zero;
            });
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG15_MARBLES_HIT_WALL);
            }
        }
    }
}