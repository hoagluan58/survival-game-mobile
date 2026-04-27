using DG.Tweening;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.SaveData;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class LoseChallengeModePopupUI : BaseUIView
    {
        [SerializeField] private TextMeshProUGUI _dayTMP;
        [SerializeField] private Button _retryBTN;
        [SerializeField] private Button _nextBTN;

        private AudioSource _loseAudioSource;

        public override void OnOpen()
        {
            base.OnOpen();
            _retryBTN.onClick.AddListener(OnRetryButtonClicked);
            _nextBTN.onClick.AddListener(OnNextButtonClicked);
            SetData();
        }

        public override void OnClose()
        {
            base.OnClose();
            _retryBTN.onClick.RemoveListener(OnRetryButtonClicked);
            _nextBTN.onClick.RemoveListener(OnNextButtonClicked);
            _loseAudioSource?.Stop();
            _nextBTN.transform.DOKill();
        }

        private void SetData()
        {
            var day = UserData.I.Day;
            _loseAudioSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_LOSING);
            _dayTMP.SetText(GameLocalization.I.GetStringFromTable("STRING_DAY", day));
            StartCoroutine(CRPlayAnim());
        }

        private void OnRetryButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            GameManager.I.Retry();
            this.InvokeDelay(TransitionUI.DELAY_TIME, () => CloseSelf());
        }

        private void OnNextButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            if (UserData.I.IsMaxDay)
            {
                CloseSelf();
                UIManager.I.Open(Define.UIName.WIN_CHALLENGE_MODE_POPUP);
            }
            else
            {
                GameManager.I.GoNextDay();
                this.InvokeDelay(TransitionUI.DELAY_TIME, () => CloseSelf());
            }
        }

        private IEnumerator CRPlayAnim()
        {
            // Reset state
            _retryBTN.gameObject.SetActive(false);
            _nextBTN.gameObject.SetActive(false);
            _nextBTN.transform.localScale = Vector3.one;
            _nextBTN.gameObject.DOScaleShow(() =>
            {
                _nextBTN.gameObject.DOScaleLoop(Vector3.one, Vector3.one * 1.1f, 0.5f);
            });
            yield return new WaitForSeconds(0.5f);
            _retryBTN.gameObject.DOScaleShow();
        }
    }
}
