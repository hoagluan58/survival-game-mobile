using DG.Tweening;
using NFramework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class TransitionUI : BaseUIView
    {
        public static float DELAY_TIME = 0.75f;
        private readonly int _circleSizeId = Shader.PropertyToID("_Circle_Size");

        [SerializeField] private Image _loadingIMG;

        private Tween _tween;

        public IEnumerator CRLoadingAnim(bool value)
        {
            if (_tween != null && _tween.IsPlaying())
            {
                yield return new WaitUntil(() => _tween.IsPlaying() == false);
            }
            _tween?.Kill();
            UIManager.I.DisableInteract();
            if (value)
            {
                _loadingIMG.gameObject.SetActive(true);
                SetCircleSize(1f);
                _tween = DOVirtual.Float(1f, 0f, DELAY_TIME, x => SetCircleSize(x));
                yield return _tween.WaitForCompletion();
            }
            else
            {
                SetCircleSize(0f);
                _tween = DOVirtual.Float(0f, 1f, DELAY_TIME, x => SetCircleSize(x)).OnComplete(() =>
                {
                    _loadingIMG.gameObject.SetActive(false);
                    UIManager.I.EnableInteract();
                });
                yield return _tween.WaitForCompletion();
            }
        }

        private void SetCircleSize(float value)
        {
            _loadingIMG.material.SetFloat(_circleSizeId, value);
        }
    }
}
