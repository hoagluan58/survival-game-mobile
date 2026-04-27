using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Game
{
    public class NPC : MonoBehaviour
    {
        [SerializeField] private BaseCharacter _model;

        [Header("DEBUG")]
        [SerializeField] private INPCState.EState _curStateName;

        private bool _isUpdate;
        private List<INPCState> _states = new List<INPCState>();
        private INPCState _curState;
        public BaseCharacter Model => _model;
        public List<INPCState> States => _states;

        public void SetState(List<INPCState> states)
        {
            _states = states;
            foreach (var state in _states)
            {
                state.OnInit();
            }
            _curState = null;
            _curStateName = INPCState.EState.None;
        }

        public void TransitionTo(INPCState state)
        {
            _curState?.OnExit();
            _curState = state;
            _curStateName = _curState.StateName;
            _curState?.OnEnter();
            ToggleUpdate(true);
        }

        public void TransitionTo(INPCState.EState stateName)
        {
            var state = _states.Find(_ => _.StateName == stateName);
            TransitionTo(state);
        }

        private void Update()
        {
            if (!_isUpdate) return;

            _curState?.OnUpdate();
        }

        public void ToggleUpdate(bool value) => _isUpdate = value;
    }
}
