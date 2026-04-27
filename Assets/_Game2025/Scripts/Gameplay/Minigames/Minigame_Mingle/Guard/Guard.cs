using Animancer;
using NFramework;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.MinigameMingle
{
    public class Guard : MonoBehaviour
    {
        public event UnityAction OnShootCompletedAction;

        public ClipTransition IdleClip;
        public ClipTransition ShootClip;
        public ClipTransition WalkClip;
        [SerializeField] LaserLine _line;
        [SerializeField] Transform _akOutput;

        public AnimancerComponent Animancer;

        public Transform _target;

        [Button]
        public void ShootEditor()
        {
            LookAt(_target).PlayShootAnim().ShowLine(0.25f, _target).ClearLine(0.45f);
        }

        public Guard LookAt(Transform target)
        {
            transform.LookAt(target);
            return this;
        }


        public Guard PlayShootAnim()
        {
            var state = Animancer.Play(ShootClip);
            state.Events.OnEnd += OnEndAnimtion;
            return this;
        }


        public Guard OnShootCompleted(UnityAction unityAction)
        {
            OnShootCompletedAction += unityAction;
            return this;
        }


        private void OnEndAnimtion()
        {
            Animancer.Play(IdleClip);
            OnShootCompletedAction?.Invoke();
            OnShootCompletedAction = null;
        }


        public Guard ShowLine(float delay, Transform point, UnityAction onStartShow = null)
        {
            this.InvokeDelay(delay, () => {
                onStartShow?.Invoke();
                _line.DrawLine(new Transform[] { _akOutput, point });
            });
            return this;
        }

        public Guard ClearLine(float delay)
        {
            this.InvokeDelay(delay, () => { _line.Clearline(); });
            return this;
        }

        public void PlayWalkAnimation()
        {
            Animancer.Play(WalkClip);
        }

        public void PlayIdleAnimation()
        {
            Animancer.Play(IdleClip);
        }

    }
}
