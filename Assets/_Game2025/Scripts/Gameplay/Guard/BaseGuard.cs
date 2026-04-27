using Animancer;
using NFramework;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.Game
{
    public class BaseGuard : MonoBehaviour
    {
        public event UnityAction OnShootCompletedAction;

        public ClipTransition IdleClip;
        public ClipTransition ShootClip;
        [SerializeField] LaserLine _line;
        [SerializeField] Transform _akOutput;

        [HideInInspector] public AnimancerComponent Animancer;

        public void Start()
        {
            Animancer = GetComponentInChildren<AnimancerComponent>();
            Animancer.Play(IdleClip);
        }


        public BaseGuard PlayShootAnim()
        {
            var state = Animancer.Play(ShootClip);
            state.Events.OnEnd += OnEndAnimtion; 
            return this;
        }


        public BaseGuard OnShootCompleted(UnityAction unityAction)
        {
            OnShootCompletedAction += unityAction;
            return this;
        }


        private void OnEndAnimtion()
        {
            Animancer.Play(IdleClip);
            _line.Clearline();
            OnShootCompletedAction?.Invoke();
            OnShootCompletedAction = null;
        }


        public BaseGuard ShowLine(float delay, Transform point, UnityAction onStartShow = null)
        {
            this.InvokeDelay(delay, () => {
                onStartShow?.Invoke();
                _line.DrawLine(new Transform[] { _akOutput, point });
            });
            return this;
        }

        public BaseGuard ClearLine(float delay)
        {
            this.InvokeDelay(delay, () => { _line.Clearline(); });
            return this;
        }
    }
}