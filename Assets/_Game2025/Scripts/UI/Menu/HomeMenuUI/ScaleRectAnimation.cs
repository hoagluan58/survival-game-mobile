using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape
{
    public class ScaleRectAnimation : BaseRectAnimation
    {
        public override void Show(float duration, Ease ease = Ease.OutBack, UnityAction onCompleted = null)
        {
            base.Show(duration, ease, onCompleted);
            _rectTransform.DOKill();
            _rectTransform.localScale = Vector3.one*_hideValue;
            _rectTransform.DOScale(Vector3.one*_showValue, 0.5f).SetEase(ease).OnComplete(() => onCompleted?.Invoke());
        }
    }
}
