using Animancer;
using NFramework;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Game;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.MinigameMarblesVer2
{
    public class Guard : MonoBehaviour
    {
        public event UnityAction OnShootCompletedAction;

        public ClipTransition IdleClip;
        public ClipTransition ShootClip;
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
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
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

        public void PlayIdleAnimation()
        {
            Animancer.Play(IdleClip);
        }

        internal void ResetToDefault()
        {
            transform.localEulerAngles  = new Vector3(0, 128.6f, 0); 
        }
    }
}
