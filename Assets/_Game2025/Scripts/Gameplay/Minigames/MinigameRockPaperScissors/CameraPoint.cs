using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.MinigameRockPaperScissors
{
    public class CameraPoint : MonoBehaviour
    {
        [SerializeField] private Transform _point;
        [SerializeField] private float _fieldOfView;

        public float FieldOfView => _fieldOfView;
        private Camera _cam;

        public void Init(Camera cam)
        {
            _cam = cam;
        }

        public void Play(float duration = 2, Ease ease = Ease.Linear, UnityAction onCompleted = null)
        {
            _cam.transform.DOKill();
            _cam.DOFieldOfView(_fieldOfView, duration);
            _cam.transform.DORotate(_point.eulerAngles, duration).SetEase(ease);
            _cam.transform.DOMove(_point.position, duration).SetEase(ease).OnComplete(() => onCompleted?.Invoke());
        }

    }
}
