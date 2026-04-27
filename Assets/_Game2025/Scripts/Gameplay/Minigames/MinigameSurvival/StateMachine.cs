using Animancer;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Survival;
using System;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class StateMachine
    {
        public State CurrentState;

        bool canChangeState = true;

        public void Initialize(State startState)
        {
            canChangeState = true;
            CurrentState = startState;
            startState?.Enter();
        }


        public void ChangState(State nextState)
        {
            if (!canChangeState) return;

            CurrentState?.Exit();
            CurrentState = nextState;
            nextState.Enter();
        }

        public bool IsInState(State thisState) => CurrentState == thisState;
        public void SetActive(bool isActive) => canChangeState = isActive;
    }

    public abstract class State
    {
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void LogicUpdate() { }
        public virtual void PhysicUpdate() { }
    }

    public abstract class PlayerState : State
    {
        public PlayerController Character;
        protected PlayerState(PlayerController playerController)
        {
            Character = playerController;
        }
    }

    public class IdleState : PlayerState
    {
        public IdleState(PlayerController playerController) : base(playerController) { }

        public override void Enter()
        {
            Character.ClearVelocity();
            Character.PlayAnimation(ANIMATION.Idle);
        }
        public override void PhysicUpdate()
        {
            Character.HandleGravity();
        }

        public override void LogicUpdate()
        {
            if (Character.GetJoystickDirection().magnitude >= 0.1f)
                Character.ChangeState(Character.MoveState);
        }
    }

    public class MoveState : PlayerState
    {
        AudioSource _footStepSound;
        public MoveState(PlayerController playerController) : base(playerController) { }

        public override void Enter()
        {
            // Character.ParticleDust.Play();
            Character.CancelHitbox();
            Character.PlayAnimation(ANIMATION.Move);
            _footStepSound = GameSound.I.PlaySFX(Define.SoundPath.SFX_FOOT_STEP, true);
        }

        public override void Exit()
        {
            _footStepSound?.Stop();

        }
        public override void LogicUpdate()
        {
            if (Character.GetJoystickDirection().magnitude < 0.1f)
                Character.ChangeState(Character.IdleState);
        }

        public override void PhysicUpdate()
        {
            Character.HandleGravity();
        }
    }

    public class JumpState : PlayerState
    {
        public JumpState(PlayerController playerController) : base(playerController) { }

        override public void Enter()
        {
            Character.PlayAnimation(ANIMATION.Jump, FadeMode.FromStart);
        }

        public override void Exit()
        {
            Character.ClearVelocity();
        }

        public override void PhysicUpdate()
        {
            Character.HandleGravity();
            if (Character.IsGrounded())
            {
                Character.DecideStateBasedOnCurrentJoystickInput();
            }
        }
    }

    public class AttackState : PlayerState
    {
        AnimancerState attackAnimation;
        public AttackState(PlayerController playerController) : base(playerController) { }

        public override void Enter()
        {
            Character.ClearVelocity();
            Character.ActiveHitbox(Character.CurrentWeapon.Stats.DelayTime, Character.CurrentWeapon.Stats.Duration);
            Character.SetActiveJoystick(false);
            attackAnimation = Character.PlayAnimation(ANIMATION.Attack, FadeMode.FromStart, 0f);
            attackAnimation.Events.OnEnd = () =>
            {
                attackAnimation.Events.Clear();
                Character.DecideStateBasedOnCurrentJoystickInput();
            };
        }

        public override void Exit()
        {
            Character.SetActiveJoystick(true);
        }
    }

    public class DamagedState : PlayerState
    {
        AnimancerState attackAnimation;
        public DamagedState(PlayerController playerController) : base(playerController) { }

        public override void Enter()
        {
            Character.CombatHandler?.CancleSlashEffect();
            Character.CancelHitbox();
            Character.ClearVelocity();
            Character.SetActiveJoystick(false);
            attackAnimation = Character.PlayAnimation(ANIMATION.Damaged, FadeMode.FixedDuration, 0f);
            attackAnimation.Events.OnEnd = () =>
            {
                attackAnimation.Events.Clear();
                Character.DecideStateBasedOnCurrentJoystickInput();
            };
        }

        public override void Exit()
        {
            Character.SetActiveJoystick(true);
        }
    }

    public class DeadState : PlayerState
    {
        public DeadState(PlayerController playerController) : base(playerController) { }

        public override void Enter()
        {
            Character.PlayDeadAnimation();
        }

        public override void PhysicUpdate()
        {
            Character.HandleGravity();
        }
    }

    public class WinState : PlayerState
    {
        public WinState(PlayerController playerController) : base(playerController) { }

        public override void Enter()
        {
        }
    }
}
