using UnityEngine;

namespace SquidGame.Minigame03
{
    public class NeedleLineRenderer : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        private bool _isDone;
        private float _speed;
        private Vector3[] _positions;
        private Vector3 _curPosition;
        private bool _isDrawing, _isInit;
        private int _currentIndex = 1;
        private float _t = 0f;

        public Vector3 CurPosition => _curPosition;
        public bool IsDone => _isDone;

        public void Init(Vector3[] positions, float speed)
        {
            _speed = speed;
            _positions = positions;
            _lineRenderer.positionCount = _positions.Length;
            _lineRenderer.SetPosition(0, _positions[0]);
            _curPosition = _positions[0];
            _currentIndex = 1;
            _isInit = true;
        }

        private void Update() => HandleMouseInput();

        private void HandleMouseInput()
        {
            if (!_isInit || _isDone) return;

            if (Input.GetMouseButtonDown(0)) _isDrawing = true;

            if (Input.GetMouseButtonUp(0)) _isDrawing = false;

            if (_isDrawing && _currentIndex < _positions.Length)
            {
                Vector3 orig = _positions[_currentIndex - 1];
                Vector3 target = _positions[_currentIndex];

                _t += Time.deltaTime;
                Vector3 newPos = Vector3.Lerp(orig, target, _t / _speed);
                _lineRenderer.SetPosition(_currentIndex, newPos);
                _curPosition = newPos;

                if (_t >= _speed)
                {
                    _lineRenderer.SetPosition(_currentIndex, target);
                    _curPosition = target;
                    _currentIndex++;
                    _t = 0f;
                }
            }

            if (_currentIndex >= _positions.Length) _isDone = true;
        }
    }
}
