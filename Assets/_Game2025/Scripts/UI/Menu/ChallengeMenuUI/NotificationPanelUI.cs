using DG.Tweening;
using Redcode.Extensions;
using System;
using UnityEngine;

namespace SquidGame.LandScape.UI
{
    public class NotificationPanelUI : MonoBehaviour
    {
        public static Action OnShowNotification;

        [SerializeField] private RectTransform _rect;
        [SerializeField] private float _scaleDuration = 0.3f;
        [SerializeField] private float _showDuration = 1.5f;

        private Sequence _sequence;

        private void OnEnable()
        {
            OnShowNotification += SetData;
            _rect.SetLocalScale(0f);
        }

        private void OnDisable() => OnShowNotification -= SetData;

        public void SetData()
        {
            _rect.DOKill();
            _rect.SetLocalScale(0f);

            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _sequence.Append(_rect.DOScale(1f, _scaleDuration).SetEase(Ease.OutBack))
                     .AppendInterval(_showDuration)
                     .Append(_rect.DOScale(0f, _scaleDuration).SetEase(Ease.InBack))
                     .OnComplete(() => gameObject.SetActive(false));
        }
    }
}
