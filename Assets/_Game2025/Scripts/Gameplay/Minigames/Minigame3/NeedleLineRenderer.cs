
using NFramework;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class NeedleLineRenderer : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private NeedleController _controller;

        private Transform _needle;
        private bool _isDone;
        private bool _isBroken;
        private float _speed;
        private Vector3[] _positions;
        private Vector3 _curPosition;
        private bool _isDrawing, _isInit;
        private int _currentIndex = 1;
        private float _t = 0f;

        public Vector3 CurPosition => _curPosition;
        public bool IsDone => _isDone;

        public void Init(Vector3[] positions, float speed, Transform needle)
        {
            _needle = needle;
            _speed = speed;
            _positions = positions;
            _lineRenderer.positionCount = _positions.Length;
            _lineRenderer.SetPosition(0, _positions[0]);
            for (int i = 1; i < _lineRenderer.positionCount; i++)
            {
                _lineRenderer.SetPosition(i, _positions[0]);
            }
            _curPosition = _positions[0];
            _currentIndex = 1;
            _isInit = true;
        }

        private void Update() => HandleMouseInput();

        private void HandleMouseInput()
        {
            if (!_isInit || _isDone) return;
            if (_isBroken) return;
            if (UIManager.I.IsPointerOverUIObject()) return; 

            if (Input.GetMouseButtonDown(0))
            {
                _isDrawing = true;
                _controller.OnPointerDown(); 
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDrawing = false;
                _controller.OnPointerUp();
            }

            if (_isDrawing && _currentIndex < _positions.Length)
            {
                Vector3 orig = _positions[_currentIndex - 1];
                Vector3 target = _positions[_currentIndex];

                _t += Time.deltaTime;
                Vector3 newPos = Vector3.Lerp(orig, target, _t / _speed);
                _lineRenderer.SetPosition(_currentIndex, newPos);
                UpdatePoint(_currentIndex, _lineRenderer.positionCount, newPos);
                _curPosition = newPos;
                _needle.position = CurPosition;

                if (_t >= _speed)
                {
                    _lineRenderer.SetPosition(_currentIndex, target);
                    _curPosition = target;
                    _currentIndex++;
                    _t = 0f;
                }
            }

            if (_currentIndex >= _positions.Length)
            {
                _isDone = true;
                _controller.OnPointerUp();
            }
        }

        private void UpdatePoint(int from, int to, Vector3 pos)
        {
            for (int i = from; i < to; i++)
            {
                _lineRenderer.SetPosition(i, pos);
            }
        }

        public void CompleteLine()
        {
            _lineRenderer.positionCount = _positions.Length;
            for (int i = 0; i < _lineRenderer.positionCount; i++)
            {
                _lineRenderer.SetPosition(i, _positions[i]);
            }
        }

        public void ForceStopDraw()
        {
            _isDrawing = false;
        }

        public void SetBroken()
        {
            _isBroken = true;
        }

        public void ResetLineRenderer()
        {
            _lineRenderer.positionCount = 0;
            _currentIndex = 1;
            _isDone = false;
            _isInit = false;
            _isDrawing = false;
            _isBroken = false;
        }
    }
}
