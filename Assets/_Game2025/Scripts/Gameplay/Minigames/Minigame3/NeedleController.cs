using Dreamteck.Splines;
using NFramework;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace SquidGame.LandScape.Minigame3
{
    public class NeedleController : MonoBehaviour
    {
        [SerializeField] private Transform _needle;
        [SerializeField] private Animator _animator;
        [SerializeField] private NeedleLineRenderer _lineRenderer;

        private Dalgona _dalgona;
        private CutDalgonaPanelUI _dalgonaPanelUI;
        private MinigameController _controller;

        public void Init(MinigameController controller, CutDalgonaPanelUI dalgonaPanelUI)
        {
            _controller = controller;
            _dalgonaPanelUI = dalgonaPanelUI;
        }

        private void Awake()
        {
            TimeController.OnTimeOut += OnPointerUp;
        }

        private void OnDestroy()
        {
            TimeController.OnTimeOut -= OnPointerUp;
        }

        public void ResetNeedle()
        {
            _lineRenderer.ResetLineRenderer();
        }

        public void SetBroken()
        {
            _lineRenderer.SetBroken();
        }

        public void CompleteLine()
        {
            _lineRenderer.CompleteLine();
        }

        public void Active(Dalgona dalgona)
        {
            _dalgona = dalgona;
            //_dalgonaPanelUI.SetEvent(OnPointerDown, OnPointerHold, OnPointerUp);
            var splinePoints = _dalgona.Spline.GetPoints();
            var positions = new Vector3[splinePoints.Length];

            for (int i = 0; i < splinePoints.Length; i++)
            {
                SplinePoint point = splinePoints[i];
                positions[i] = point.position;
            }
            _needle.position = positions[0];
            _lineRenderer.Init(positions, _dalgona.NeedleSpeed, _needle);

            _needle.gameObject.SetActive(true);
        }

        public void ToggleLine(bool value) => _lineRenderer.gameObject.SetActive(value);

        public void Deactivate()
        {
            _animator.Play("Idle");
            _needle.gameObject.SetActive(false);
        }

        public void OnPointerDown()
        {
            _dalgonaPanelUI.OnPointerDown();
            _controller.TimeController.StartCountDown(TimerType.HoldNeedle);
            _animator.Play("Move");
            _animator.speed = 1f;
        }

        private void OnPointerHold()
        {            
            //_needle.position = _lineRenderer.CurPosition;            
        }

        public void OnPointerUp()
        {
            _dalgonaPanelUI.OnPointerUp();
            _controller.TimeController.StopCountDown(TimerType.HoldNeedle);
            _lineRenderer.ForceStopDraw();
            _animator.speed = 0f;
            if (_lineRenderer.IsDone)
            {
                Done();
            }
        }

        private void Done()
        {
            _dalgonaPanelUI.RemoveAllEvent();
            _animator.speed = 0f;
            _controller.StartBreakDalgonaStep(1f);
        }
    }
}
