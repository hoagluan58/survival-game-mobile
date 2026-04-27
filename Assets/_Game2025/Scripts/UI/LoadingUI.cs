using DG.Tweening;
using NFramework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.UI
{
    public class LoadingUI : BaseUIView
    {
        [SerializeField] private TextMeshProUGUI _loadingTMP;
        [SerializeField] private Image _fillIMG;

        public void SetData(float loadingTime, Action onLoadingCompleted = null)
        {
            _fillIMG.fillAmount = 0f;
            _fillIMG.DOFillAmount(1f, loadingTime).SetEase(Ease.Linear).OnUpdate(() =>
            {
                var percent = _fillIMG.fillAmount * 100f;
                percent = percent >= 99 ? 100 : percent;
                UpdateLoadingText($"Loading {percent:F0}%");
            }).OnComplete(() =>
            {
                UpdateLoadingText($"Loading {100}%");
                CloseSelf();
                onLoadingCompleted?.Invoke();
            });

            void UpdateLoadingText(string text) => _loadingTMP.text = text;
        }
    }
}
