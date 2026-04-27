using Sirenix.OdinInspector;
using SquidGame.LandScape.Game;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SquidGame.LandScape.MinigameMingle
{
    public enum NpcState
    {
        GoToPodium = 0,
        Dance = 1,
        Die = 2,
        EnterRoom = 3,
        WanderInRoom = 4,
        Wander = 5,
        WanderInPodium = 6,
    }


    public class NpcBase : Character
    {
        [Header("Configs")]
        [SerializeField] private int _id;
        [SerializeField] private NpcState _state;
        [SerializeField][ReadOnly] private int _round;

        [Header("References")]
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private ParticleSystem _dustFx;
        [SerializeField] private BaseCharacter _baseCharacter;

        public BaseCharacter BaseCharacter => _baseCharacter;
        public int Round => _round;
        public bool IsLive => _state != NpcState.Die;
        public RoomBase Room => _room;
        public RingAreaSpawner RingAreaSpawner => _ringAreaSpawner;
        public CharacterAnimator Animator => _animator;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        public GameObject DustFx => _dustFx.gameObject;

        private RingAreaSpawner _ringAreaSpawner;
        private RoomBase _room;
        private Dictionary<NpcState, NpcStateMachine> _stateMachine;
        private NpcStateMachine _currentState;


        public void Init()
        {
            _stateMachine = new Dictionary<NpcState, NpcStateMachine>();
            _stateMachine.Add(NpcState.Wander, new WanderState().Init(this));
            _stateMachine.Add(NpcState.GoToPodium, new GoToPodiumState().Init(this));
            _stateMachine.Add(NpcState.EnterRoom, new EnterRoomState().Init(this));
            _stateMachine.Add(NpcState.Die, new DieState().Init(this));
            _stateMachine.Add(NpcState.WanderInPodium, new WanderInPodium().Init(this));
            _stateMachine.Add(NpcState.WanderInRoom, new WanderInRoomState().Init(this));
            _animator.PlayAnimation(EAnimStyle.Idle);
        }

        public NpcBase SetRoom(RoomBase room)
        {
            _room = room;
            return this;
        }


        public NpcBase SetRingAreaSpawner(RingAreaSpawner ringAreaSpawner)
        {
            _ringAreaSpawner = ringAreaSpawner;
            return this;
        }


        public void SwitchState(NpcState state)
        {
            if (_state == state || _state == NpcState.Die) return;
            _state = state;
            _currentState?.Exit();
            _currentState = _stateMachine[state];
            _currentState.Enter();
        }


        public void SetImmediateState(NpcState state)
        {
            SetEnableOutline(false);
            _state = state;
            _currentState?.Exit();
            _currentState = _stateMachine[state];
            _currentState.Enter();
        }


        private void Update()
        {
            _currentState?.OnUpdate();
        }


        private void LateUpdate()
        {
            _currentState?.OnLateUpdate();
        }

        public bool IsInPodium()
        {
            return _currentState.IsCompleted();
        }

        public void ChangeStateToDieIfWander()
        {
            if (_state == NpcState.Wander)
                SwitchState(NpcState.Die);
        }


        public void SetRound(int round)
        {
            _round = round;
        }


        public void OnRevive()
        {
            SetEnableOutline(false);
            _dustFx.Stop();
            _dustFx.Clear();
        }

        public void EnableDustFx()
        {
            _dustFx.Play();
        }
    }
}
