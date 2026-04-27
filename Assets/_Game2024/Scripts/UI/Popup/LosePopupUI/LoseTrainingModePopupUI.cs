using DG.Tweening;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class LoseTrainingModePopupUI : BaseUIView
    {
        [SerializeField] private Button _backBTN;
        [SerializeField] private Button _retryBTN;

        private AudioSource _loseAudioSource;

        public override void OnOpen()
        {
            base.OnOpen();
            _backBTN.onClick.AddListener(OnBackButtonClicked);
            _retryBTN.onClick.AddListener(OnRetryButtonClicked);
            SetData();
        }

        public override void OnClose()
        {
            base.OnClose();
            _backBTN.onClick.RemoveListener(OnBackButtonClicked);
            _retryBTN.onClick.RemoveListener(OnRetryButtonClicked);
            _loseAudioSource?.Stop();
            _retryBTN.transform.DOKill();
        }

        private void SetData()
        {
            _loseAudioSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_LOSING);
            StartCoroutine(CRPlayAnim());
        }

        private void OnRetryButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            GameManager.I.Retry();
            this.InvokeDelay(TransitionUI.DELAY_TIME, () => CloseSelf());
        }

        private void OnBackButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            GameManager.I.Exit();
            this.InvokeDelay(TransitionUI.DELAY_TIME, () => CloseSelf());
        }

        private IEnumerator CRPlayAnim()
        {
            // Reset state
            _backBTN.gameObject.SetActive(false);
            _retryBTN.gameObject.SetActive(false);
            _retryBTN.transform.localScale = Vector3.one;

            _backBTN.gameObject.DOScaleShow();
            yield return new WaitForSeconds(0.5f);
            _retryBTN.gameObject.DOScaleShow(() =>
            {
                _retryBTN.gameObject.DOScaleLoop(Vector3.one, Vector3.one * 1.1f, 0.5f);
            });
        }
    }
}
