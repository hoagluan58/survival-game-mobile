using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape
{
    [RequireComponent(typeof(RectTransform))]
    public class BaseRectAnimation : MonoBehaviour
    {
        protected RectTransform _rectTransform;
        [SerializeField] protected bool _showOnEnable;
        [SerializeField] protected float _hideValue;
        [SerializeField] protected float _showValue;


        protected virtual void OnEnable()
        {
            _rectTransform = GetComponent<RectTransform>();
            if (_showOnEnable) Show(0.5f);
        }

        public virtual void Show(float duration,Ease ease = Ease.OutBack,UnityAction onCompleted = null) { }

        public virtual void Hide(float duration) { }
    }
}
