using UnityEngine;

namespace Game2
{
    /// <summary>
    /// Manage glass bridge and steps
    /// </summary>
    public class GameLevel : MonoBehaviour
    {
        [SerializeField] private Transform _posEndArea;
        [SerializeField] private int _totalTime;

        private Step[] _allSteps;

        public Step[] AllSteps => _allSteps;
        public int TotalTime => _totalTime;
        public Transform PosEndArea => _posEndArea;
        public int NumberLane => _allSteps[0].NumberLane;

        private void Awake()
        {
            _allSteps = GetComponentsInChildren<Step>();
        }

        public (bool isLastStep, Step step) GetStepInfo(int index)
        {
            var step = index >= 0 && index < _allSteps.Length ? _allSteps[index] : null;
            return (step != null && index == _allSteps.Length - 1, step);
        }
    }
}