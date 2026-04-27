using Dreamteck.Splines;
using System.Collections;
using UnityEngine;

namespace SquidGame.Minigame03
{
    public class NeedleController : MonoBehaviour
    {
        [SerializeField] private Transform _needle;
        [SerializeField] private Vector3 _offsetPosInit;
        [SerializeField] private Animator _animator;
        [SerializeField] private NeedleLineRenderer _lineRenderer;

        private Dalgona _dalgona;
        private bool _isActive;
        private bool _isWaitToActive;
        private MinigameController _controller;
        private CameraController _cameraController;

        public void Init(MinigameController controller, CameraController cameraController)
        {
            _controller = controller;
            _cameraController = cameraController;
        }

        public void Active(Dalgona dalgona)
        {
            _dalgona = dalgona;
            StartCoroutine(CRActive());

            IEnumerator CRActive()
            {
                var splinePoints = _dalgona.Spline.GetPoints();
                var positions = new Vector3[splinePoints.Length];

                for (int i = 0; i < splinePoints.Length; i++)
                {
                    SplinePoint point = splinePoints[i];
                    positions[i] = point.position;
                }

                _lineRenderer.Init(positions, _dalgona.NeedleSpeed);
                _needle.position = _lineRenderer.CurPosition;

                yield return _cameraController.CRSwitchCamera(CameraController.ECameraType.Follow);

                _needle.gameObject.SetActive(true);
                _isWaitToActive = true;
                _controller.InvokeShowCountdownPanel(true);
                _controller.InvokeShowHoldToMoveTutorial(true);
            }
        }

        public void ToggleLine(bool value) => _lineRenderer.gameObject.SetActive(value);

        public void Deactivate()
        {
            _isActive = false;
            _animator.Play("Idle");
            _needle.gameObject.SetActive(false);
        }

        private void StartCountdown()
        {
            _isWaitToActive = false;
            _isActive = true;
            _controller.StartCountDown();
        }

        private void Update()
        {
            if (_isWaitToActive && Input.GetMouseButtonDown(0))
                StartCountdown();

            if (_isActive == false) return;



            if (Input.GetMouseButton(0))
            {
                _animator.Play("Move");
                _animator.speed = 1f;
                _needle.position = _lineRenderer.CurPosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _animator.speed = 0f;
                if (_lineRenderer.IsDone)
                {
                    Done();
                }
            }
        }

        private void Done()
        {
            _isActive = false;
            _animator.speed = 0f;
            _controller.MoveStepBreakDalgona();
        }
    }
}