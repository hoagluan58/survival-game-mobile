using DG.Tweening;
using NFramework;
using Redcode.Extensions;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SquidGame.LandScape.MinigameMarblesVer2
{
    public class MinigameMarblesTwoMenuUI : BaseUIView
    {
        

        [Header("Menu")]
        [SerializeField] private Button _settingButton;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Button _startButton;
        [SerializeField] private ScorePanel _playerPanel;
        [SerializeField] private ScorePanel _opponentPanel;
        [SerializeField] private TextMeshProUGUI _announcerTMP;
        [SerializeField] private NoticePanel _noticePanel;

        public ScorePanel PlayerPanel => _playerPanel;
        public ScorePanel OpponentPanel => _opponentPanel;

        private Tweener _scoreTweener;
        private Tween _delayTweener;
        private void OnEnable()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
            _settingButton.onClick.AddListener(OnSettingButtonClicked);
        }


        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
            _settingButton.onClick.RemoveListener(OnSettingButtonClicked);
        }

        public void PlayShowScoreAnimation(float score, UnityAction onCompleted)
        {

            GameSound.I.PlaySFX(Define.SoundPath.SFX_ROCKPAPERSCISSOR_RESULT);
            _announcerTMP.gameObject.SetActive(true);
            _announcerTMP.rectTransform.SetAnchoredPositionY(_announcerTMP.rectTransform.anchoredPosition.y - 40f);
            _announcerTMP.rectTransform.DOAnchorPosY(_announcerTMP.rectTransform.anchoredPosition.y + 40f, 0.5f);
            _scoreTweener = DOVirtual.Float(0, score, 1f, value =>
            {
                _announcerTMP.text = $"SCORE: {value.ToString("00")}";
            }).OnComplete(() => {
                _delayTweener = DOVirtual.DelayedCall(1f, () => {
                    _announcerTMP.text = "";
                    _announcerTMP.gameObject.SetActive(false);
                    onCompleted?.Invoke();
                });
            });
        }


        private void OnStartButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            GameManager.I.StartMinigame();            

            _startButton.gameObject.SetActive(false);
            _playerPanel.gameObject.SetActive(true);
            _opponentPanel.gameObject.SetActive(true);

        }

        private void OnSettingButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
        }



        public void ShowNotification(string content, float delayHide = -1)
        {
            _noticePanel.SetText(content);
            _noticePanel.gameObject.DOScaleShow();
            _noticePanel.SetActive(true,0.1f);
            if (delayHide <= 0) return;
            this.InvokeDelay(delayHide, () => {
                Debug.LogError("Hide noti");
                _noticePanel.SetActive(false,1);
                //_noticePanel.gameObject.SetActive(false);
            });

        }


        public void HideNotification()
        {
            _noticePanel.SetActive(false, 1);
           //_noticePanel.gameObject.SetActive(false);
        }

        public void SetActiveStartButton(bool active)
        {
            _startButton.gameObject.SetActive(active);
        }


        public void SetActiveScorePanel(bool value)
        {
            _playerPanel.gameObject.SetActive(value);
            _opponentPanel.gameObject.SetActive(value);
        }

        public void Startgame(int level)
        {
            _levelText.SetText($"Level {level +1 }");
            SetActiveStartButton(false);
            SetActiveScorePanel(false);
            _noticePanel.SetActive(false, 1);
            _playerPanel.OnInitialized();
            _opponentPanel.OnInitialized();
        }


        public override void OnOpen()
        {
            base.OnOpen();
        }


        public override void OnClose()
        {
            base.OnClose();
            _scoreTweener.Kill();
            _delayTweener.Kill();
            _announcerTMP.gameObject.SetActive(false);
        }

        public void ClearPanel()
        {
            _playerPanel.ClearData();
            _opponentPanel.ClearData();
        }

    }
}
