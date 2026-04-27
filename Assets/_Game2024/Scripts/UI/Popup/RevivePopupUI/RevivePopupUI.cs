using DG.Tweening;
using NFramework;
using SquidGame.Core;
using SquidGame.LandScape;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class RevivePopupUI : BaseUIView
    {
        [SerializeField] private Image _fillAmountTimeIMG;
        [SerializeField] private TextMeshProUGUI _timesTMP;

        [SerializeField] private Button _reviveBTN;

        private Action _onRevive, _onTimeout;
        private Coroutine _countdownCoroutine;

        public override void OnOpen()
        {
            _reviveBTN.onClick.AddListener(OnReviveButtonClicked);
            StartCountDown();
        }

        public override void OnClose()
        {
            _reviveBTN.onClick.RemoveListener(OnReviveButtonClicked);
            _reviveBTN.transform.DOKill();
        }

        public void SetData(Action onRevive = null, Action onNoThanks = null)
        {
            _onRevive = onRevive;
            _onTimeout = onNoThanks;
            _reviveBTN.gameObject.DOScaleLoop(Vector3.one, Vector3.one * 1.1f, 0.5f);
        }

        private void OnReviveButtonClicked()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_REVIVE);
            _onRevive?.Invoke();
            CloseSelf();
        }

        private void StartCountDown()
        {
            StopCountDown();
            _countdownCoroutine = StartCoroutine(CRCountdown());
        }

        private void StopCountDown()
        {
            if (_countdownCoroutine != null)
            {
                StopCoroutine(_countdownCoroutine);
            }
        }

        private IEnumerator CRCountdown()
        {
            float t = 5f;

            _fillAmountTimeIMG.fillAmount = 1f;
            _fillAmountTimeIMG.DOFillAmount(0f, t);

            while (t > 0f)
            {
                t--;
                _timesTMP.text = t.ToString();
                GameSound.I.PlaySFX(Define.SoundPath.SFX_TING);
                yield return new WaitForSeconds(1f);
            }

            _onTimeout?.Invoke();
            CloseSelf();
        }
    }
}
