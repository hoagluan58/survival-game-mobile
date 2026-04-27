using Animancer;
using SquidGame.LandScape.Game;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    public abstract class NpcStateMachine
    {
        protected NpcBase NpcBase;
        protected CharacterAnimator Animator;
        protected Transform Transform => NpcBase.transform;
        public virtual NpcStateMachine Init(NpcBase npc)
        {
            NpcBase = npc;
            Animator = npc.Animator;
            return this;
        }
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void OnUpdate() { }
        public virtual void OnLateUpdate() { }
        protected virtual void Jump()
        {
            Animator.PlayAnimation(EAnimStyle.Jump, 0.2f, FadeMode.NormalizedFromStart);
        }
        protected virtual void Dance()
        {
            Animator.PlayAnimation(EAnimStyle.Victory_1, 0.2f);
        }
        protected virtual void Die()
        {
            Animator.PlayAnimation(EAnimStyle.Die);
        }
        protected virtual void Move()
        {
            Animator.PlayAnimation(EAnimStyle.Running, 0.2f);
        }
        protected virtual void StandStill()
        {
            Animator.PlayAnimation(EAnimStyle.Idle, 0.2f);
        }
        public virtual bool IsCompleted() => false;
    }
}
