using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.MinigameMarblesVer2
{
    public class CameraPoint : MonoBehaviour
    {
        [SerializeField] private Transform _point;
        private Transform _cam;
        public void Init(Transform cam)
        {
            _cam = cam;
        }

        public void Play(float duration = 2, Ease ease = Ease.Linear, UnityAction onCompleted = null)
        {
            _cam.DOKill();
            _cam.DORotate(_point.eulerAngles, duration).SetEase(ease);
            _cam.DOMove(_point.position, duration).SetEase(ease).OnComplete(() => onCompleted?.Invoke());
        }

    }
}
