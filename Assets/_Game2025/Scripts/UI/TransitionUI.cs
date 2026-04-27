using DG.Tweening;
using NFramework;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.UI
{
    public class TransitionUI : BaseUIView
    {
        public static float DELAY_TIME = 1f;
        private readonly int _circleSizeId = Shader.PropertyToID("_Circle_Size");

        [SerializeField] private Image _loadingIMG;

        private Tween _tween;

        public IEnumerator CRLoadingAnim(bool value)
        {
            _tween?.Kill();
            UIManager.I.DisableInteract();
            if (value)
            {
                _loadingIMG.gameObject.SetActive(true);
                FadeScreen(0f);
                _tween = _loadingIMG.DOFade(1f, DELAY_TIME); //DOVirtual.Float(0f, 1f, DELAY_TIME, x => FadeScreen(x));
                yield return _tween.WaitForCompletion();
            }
            else
            {
                FadeScreen(1f);
                _tween = _loadingIMG.DOFade(0f, DELAY_TIME).OnComplete(() =>
                {
                    _loadingIMG.gameObject.SetActive(false);
                    UIManager.I.EnableInteract();
                });
                yield return _tween.WaitForCompletion();
            }
        }

        private void FadeScreen(float value)
        {
            _loadingIMG.color.WithAlpha(value);
        }

        public void FadeScreenDuration(float value, float duration)
        {
            _loadingIMG.color.WithAlpha(value ==  0f ? 1f : 0f);
            _loadingIMG.DOKill();
            _loadingIMG.DOFade(value, duration);
        }

        public TransitionUI SetActiveLoadingIMG(bool value)
        {
            _loadingIMG.gameObject.SetActive(value);
            return this;
        }


        private void SetCircleSize(float value)
        {
            _loadingIMG.material.SetFloat(_circleSizeId, value);
        }
    }
}
