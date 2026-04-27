using DG.Tweening;
using NFramework;
using SquidGame.Core;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class LoadingUI : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingUI;
        [SerializeField] private float _loadingTime;
        [SerializeField] private TextMeshProUGUI _loadingTMP;
        [SerializeField] private Image _loadingFillIMG;

        private void Start() => StartCoroutine(CRLoadGame());

        private IEnumerator CRLoadGame()
        {
            RunLoadingBar(_loadingTime);
            yield return new WaitForSeconds(_loadingTime);
            yield return SceneUtils.CRLoadSceneAsync(Define.SceneName.CORE, true);
            _loadingUI.SetActive(false);
            yield return SceneUtils.CRUnloadSceneAsync(Define.SceneName.LOADING);
        }

        private void RunLoadingBar(float duration)
        {
            _loadingFillIMG.fillAmount = 0f;
            _loadingFillIMG.DOFillAmount(1f, duration).SetEase(Ease.Linear).OnUpdate(() =>
            {
                var percent = _loadingFillIMG.fillAmount * 100f;
                UpdateLoadingText($"LOADING {percent:F0}%");
            });

            void UpdateLoadingText(string text) => _loadingTMP.text = text;
        }
    }
}