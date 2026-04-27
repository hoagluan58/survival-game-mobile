using DG.Tweening;
using System;
using UnityEngine;

namespace SquidGame.LandScape.UI
{
    public class MultiplierBarUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _arrowRt;
        [SerializeField] private MultiplierItemUI[] _items;

        private int _curMultiply;
        private Tween _tweenArrow;
        private RectTransform _barRt;
        private Vector2 _baseArrowPos;
        private Action _onArrowMoving;

        public int CurMultiply => _curMultiply;

        private void Awake()
        {
            _barRt = GetComponent<RectTransform>();
            _baseArrowPos = _arrowRt.anchoredPosition;
        }

        public void SetData(Action onArrowMoving = null)
        {
            _onArrowMoving = onArrowMoving;
            _arrowRt.anchoredPosition = _baseArrowPos;
            StopArrow();
        }

        public void StartArrow()
        {
            _tweenArrow = _arrowRt.DOAnchorPosX(_barRt.sizeDelta.x, 1f).SetEase(Ease.Linear).OnUpdate(() =>
            {
                foreach (var item in _items)
                {
                    if (item.IsArrowInPos(_arrowRt.anchoredPosition.x))
                    {
                        _curMultiply = item.MultiplyAmount;
                        break;
                    }
                }
                _onArrowMoving?.Invoke();
            }).SetLoops(-1, LoopType.Yoyo);
        }

        public void StopArrow()
        {
            _tweenArrow?.Kill();
        }
    }
}
