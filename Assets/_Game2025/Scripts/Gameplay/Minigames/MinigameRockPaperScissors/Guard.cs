using Animancer;
using NFramework;
using SquidGame.LandScape.Core;
using System;
using UnityEngine;

namespace SquidGame.LandScape.MinigameRockPaperScissors
{
    public class Guard : MonoBehaviour
    {
        [SerializeField] private GameObject _gunInTableGo;
        [SerializeField] private GameObject _gunInHandGo;
        [SerializeField] private AnimancerComponent _animator;
        [SerializeField] private ClipTransition _idleClip;
        [SerializeField] private ClipTransition _shootClip;
        [SerializeField] private ParticleSystem _smokeFx;

        private Vector3 _defaultAngle;

        private void Awake()
        {
            _defaultAngle = transform.localEulerAngles;
        }

        private void Start()
        {
            PlayIdleAnimation();
        }


        public void PlayIdleAnimation()
        {
            _animator.Play(_idleClip,0.1f);
        }

        public void PlayShootAnimation()
        {
            _animator.Play(_shootClip, 0.1f);
        }


        public void ShowGun(bool value)
        {
            _gunInTableGo.SetActive(!value);
            _gunInHandGo.SetActive(value);
        }


        public void Shoot(Transform target)
        {
            ShowGun(true);
            transform.LookAt(target.position);
            PlayShootAnimation();
            this.InvokeDelay(0.1f,() => _smokeFx.Play());

            this.InvokeDelay(_shootClip.Length, () =>
            {
                transform.localEulerAngles = _defaultAngle;
                PlayIdleAnimation();
            });
        }


    }
}
