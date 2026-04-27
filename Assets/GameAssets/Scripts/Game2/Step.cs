using System.Linq;
using Redcode.Extensions;
using UnityEngine;

namespace Game2
{
    public class Step : MonoBehaviour
    {
        [SerializeField] private bool _isLastStep;
        [SerializeField] private GlassPiece[] _stepObjects;

        private int _index;
        private bool _isCanJump;
        private bool _isExposed;

        public bool IsCanJump => _isCanJump;
        public int NumberLane => _stepObjects.Length;
        public GlassPiece[] GlassPieces => _stepObjects;
        public bool IsExposed => _isExposed;
        public bool IsLastStep => _isLastStep;

        private void Start()
        {
            _index = transform.GetSiblingIndex();
        }

        private void OnEnable()
        {
            _stepObjects.ForEach(s => s.OnStepped += OnGlassPieceStepped);
        }

        private void OnDisable()
        {
            _stepObjects.ForEach(s => s.OnStepped -= OnGlassPieceStepped);
        }
        
        public void BreakAll()
        {
            for (var i = 0; i < _stepObjects.Length; i++)
            {
                _stepObjects[i].Break(false, true);
            }
        }

        public void ShowTrueMove()
        {
            var rd = Random.Range(0, _stepObjects.Length);

            for (var i = 0; i < _stepObjects.Length; i++)
            {
                _stepObjects[i].Init(rd == i, _index);
            }
        }

        public void HideTrueMove()
        {
            for (int i = 0; i < _stepObjects.Length; i++)
            {
                _stepObjects[i].HideTrueMove();
            }
        }

        public void EnableJump(bool isSetActive = false)
        {
            _isCanJump = true;
            for (int i = 0; i < _stepObjects.Length; i++)
            {
                _stepObjects[i].EnableJump(isSetActive);
            }
        }

        public void DisableJump()
        {
            _isCanJump = false;
            for (int i = 0; i < _stepObjects.Length; i++)
            {
                _stepObjects[i].DisableJump();
            }
        }

        public void OnGlassPieceStepped(GlassPiece glassStepped)
        {
            // If the true move was stepped
            if (glassStepped.IsTrueMove)
            {
                _isExposed = true;
            }
            else
            {
                // If just only one left not broken
                if (_stepObjects.Count(gp => !gp.IsBroken) == 1)
                {
                    _isExposed = true;
                }
            }
        }

        public GlassPiece GetCorrectGlassPiece()
        {
            return _stepObjects.FirstOrDefault(gp => !gp.IsBroken && gp.IsTrueMove);
        }
    }
}