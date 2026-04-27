using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class Dalgona : MonoBehaviour
    {
        [SerializeField] private SplineComputer _spline;
        [SerializeField] private DalgonaPartOut[] _dalgonaPartOuts;
        [SerializeField] private DalgonaPartIn[] _dalgonaPartIns;
        [SerializeField] private float _needleSpeed;

        public float NeedleSpeed => _needleSpeed;
        public SplineComputer Spline => _spline;
        public DalgonaPartOut[] DalgonaPartOuts => _dalgonaPartOuts;
        public DalgonaPartIn[] DalgonaPartIns => _dalgonaPartIns;


#if UNITY_EDITOR
        private void OnValidate()
        {
            _spline = GetComponent<SplineComputer>();
            _dalgonaPartOuts = GetComponentsInChildren<DalgonaPartOut>();
            _dalgonaPartIns = GetComponentsInChildren<DalgonaPartIn>();
        }
#endif
    }
}
