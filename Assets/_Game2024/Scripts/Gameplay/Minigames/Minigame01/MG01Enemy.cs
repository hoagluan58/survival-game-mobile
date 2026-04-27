using Animancer;
using System;
using UnityEngine;

namespace SquidGame.Minigame01
{
    public class MG01Enemy : MonoBehaviour
    {
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private ClipTransition _gunIdleClip;
        [SerializeField] private ClipTransition _firingClip;
        [SerializeField] private LaserLine _laserLine;
        [SerializeField] private Transform _gunOutput;
        private AnimancerState _animState;

        public bool IsShooting { get; internal set; }

        private void Awake()
        {
            _firingClip.Events.OnEnd = OnEnable;
        }
        public void OnEnable()
        {
            _animState = _animancer.Play(_gunIdleClip);
        }

        public void PlayShootAnim()
        {
            IsShooting = true;
            _animState = _animancer.Play(_firingClip, 0.25f, FadeMode.FromStart);
        }

        public void ShootBot(Transform headPos)
        {
            _laserLine.DrawLine(new Transform[2] { _gunOutput, headPos });
        }

        public void ShootCompleted()
        {
            IsShooting = false;
            _laserLine.Clearline();
        }
    }
}
