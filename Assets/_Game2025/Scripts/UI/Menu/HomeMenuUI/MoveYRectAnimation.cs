using DG.Tweening;
using Redcode.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape
{
    public class MoveYRectAnimation : BaseRectAnimation
    {
        public override void Hide(float duration)
        {
            base.Hide(duration);
            _rectTransform.DOKill();
            _rectTransform.DOAnchorPosY(_hideValue, duration);
        }

        public override void Show(float duration, Ease ease = Ease.OutBack, UnityAction onCompleted = null)
        {
            base.Show(duration);
            _rectTransform.DOKill();
            _rectTransform.SetAnchoredPositionY(_hideValue);
            _rectTransform.DOAnchorPosY(_showValue, duration).SetEase(ease).OnComplete(() => onCompleted?.Invoke());
        }
    }
}
