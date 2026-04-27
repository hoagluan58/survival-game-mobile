using DG.Tweening;
using NFramework;
using Redcode.Extensions;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class WinTrainingModePopupUI : BaseUIView
    {
        [SerializeField] private Button _backBTN;
        [SerializeField] private Button _continueBTN;
        [SerializeField] private GameObject _normalPNL;

        [Header("ANIMATION")]
        [SerializeField] private RectTransform _winPanel;
        [SerializeField] private RectTransform _cashPanel;

        private AudioSource _winSFX;

        public override void OnOpen()
        {
            base.OnOpen();
            _backBTN.onClick.AddListener(OnBackButtonClicked);
            _continueBTN.onClick.AddListener(OnContinueButtonClicked);
            SetData();
        }

        public override void OnClose()
        {
            base.OnClose();
            _backBTN.onClick.RemoveListener(OnBackButtonClicked);
            _continueBTN.onClick.RemoveListener(OnContinueButtonClicked);
            _winSFX.Stop();
            _continueBTN.transform.DOKill();
        }

        private void SetData()
        {
            _winSFX = GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_SCREEN);
            StartCoroutine(CRPlayAnim());
        }

        private void OnContinueButtonClicked()
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
            _backBTN.gameObject.SetActive(false);
            _continueBTN.gameObject.SetActive(false);
            _winPanel.transform.SetLocalScaleX(0f);
            _cashPanel.SetAnchoredPositionY(-663f);
            _winPanel.transform.DOScaleX(1f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            _cashPanel.DOAnchorPosY(0, 0.5f);
            yield return new WaitForSeconds(0.5f);
            _backBTN.gameObject.DOScaleShow();
            _continueBTN.gameObject.DOScaleShow(() =>
            {
                _continueBTN.gameObject.DOScaleLoop(Vector3.one, Vector3.one * 1.1f, 0.5f);
            });
        }
    }
}
