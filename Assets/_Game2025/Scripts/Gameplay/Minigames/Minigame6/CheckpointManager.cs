using NFramework;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.Minigame6.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace SquidGame.LandScape.Minigame6
{
    public class CheckpointManager : MonoBehaviour
    {
        [Serializable]
        public class CheckpointData
        {
            public SplineContainer SplineContainer;
            public CheckpointController Controller;
        }

        [SerializeField] private SplineAnimate _splineAnimate;
        [SerializeField] private List<CheckpointData> _checkpoints;

        private MinigameController _controller;
        private CheckpointData _curCheckpoint;
        private Action _onCompleted;

        private void OnEnable() => _splineAnimate.Completed += OnSplineAnimatedCompleted;

        private void OnDisable() => _splineAnimate.Completed -= OnSplineAnimatedCompleted;

        public void Init(MinigameController controller, List<Character> charGroup, MinigameUI ui)
        {
            _controller = controller;
            _checkpoints.ForEach(c => c.Controller.Init(_controller, charGroup, ui));
        }

        public void MoveToCheckPoint(int index, Action onCompleted = null)
        {
            if (_checkpoints.IsIndexOutOfList(index))
            {
                Debug.LogError($"{this}: Can't move to checkpoint");
                return;
            }

            _onCompleted = onCompleted;
            _curCheckpoint?.Controller?.OnExit();
            _curCheckpoint = _checkpoints[index];
            _splineAnimate.Container = _curCheckpoint.SplineContainer;
            _splineAnimate.Restart(true);
        }

        private void OnSplineAnimatedCompleted()
        {
            _onCompleted?.Invoke();
            _curCheckpoint.Controller?.OnEnter();
        }

        public void Revive() => _curCheckpoint.Controller?.OnRevive();
    }
}
