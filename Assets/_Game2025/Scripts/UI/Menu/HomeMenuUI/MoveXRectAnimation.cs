using DG.Tweening;
using Redcode.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape
{
    public class MoveXRectAnimation : BaseRectAnimation
    {
        public override void Show(float duration, Ease ease = Ease.OutBack, UnityAction onCompleted = null)
        {
            base.Show(duration);
            _rectTransform.DOKill();
            _rectTransform.SetAnchoredPositionX(_hideValue);
            _rectTransform.DOAnchorPosX(_showValue, duration).SetEase(ease).OnComplete(() => onCompleted?.Invoke());
        }

        public override void Hide(float duration)
        {
            base.Hide(duration);
            _rectTransform.DOKill();
            _rectTransform.DOAnchorPosX(_hideValue, duration);
        }
    }
}
