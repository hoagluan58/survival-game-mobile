using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame4
{
    public class Minigame04MenuUI : BaseUIView
    {
        [SerializeField] private Button _settingButton;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _notificationText;
        [SerializeField] private CanvasGroup _notificationGo;
        [SerializeField] private Scoreboard _playerScoreboard;
        [SerializeField] private Scoreboard _enemyScoreboard;
        [SerializeField] private WinPopup _winPopup;
        [SerializeField] private LosePopup _losePopup;
        [SerializeField] private GameObject _tutorialGo;
        public WinPopup WinPopup => _winPopup;
        public LosePopup LosePopup => _losePopup;

        private const string SHOW_TUTORIAL = "SHOW_TUTORIAL_MINIGAME04";

        #region API
        public void Initialized(EGameMode gameMode, LevelContent _levelContent)
        {
            _levelText.text = $"Level {_levelContent.IdLevel + 1}";
            _levelText.gameObject.SetActive(gameMode == EGameMode.Minigame); 
            _playerScoreboard.OnInitialized();
            _enemyScoreboard.OnInitialized();
            _winPopup.OnInitialize();
            _losePopup.OnInitialize();

           
        }


        public void ShowScoreBoard(float delay, bool value,float duration)
        {
            _playerScoreboard.Show(delay,value, duration);
            _enemyScoreboard.Show(delay, value, duration);
        }


        public Minigame04MenuUI SetLevel(int level)
        {
            _levelText.text = $"Level {level}";
            return this;
        }

        public Minigame04MenuUI SetNotification(string notification)
        {
            _notificationText.text = notification;
            return this;
        }

        public Minigame04MenuUI ShowNotification(float delay, bool value , float duration)
        {
            _notificationGo.DOFade(value ? 1 : 0, duration).OnComplete(() => _notificationGo.blocksRaycasts = value).SetDelay(delay);
            return this;
        }

        public Minigame04MenuUI SetPlayerScored(int index, bool value)
        {
            _playerScoreboard.Scored(index, value);
            return this;
        }

        public Minigame04MenuUI SetPlayerScored(bool value)
        {
            _playerScoreboard.Scored(value);
            return this;
        }

        public Minigame04MenuUI SetEnemyScored(int index, bool value)
        {
            _enemyScoreboard.Scored(index, value);
            return this;
        }

        public Minigame04MenuUI SetEnemyScored(bool value)
        {
            _enemyScoreboard.Scored(value);
            return this;
        }
        #endregion

        private void OpenSettingPopup()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
        }

        #region 
        public override void OnOpen()
        {
            base.OnOpen();
            _settingButton.onClick.AddListener(OpenSettingPopup);
        }

        public override void OnClose()
        {
            base.OnClose();
            _settingButton.onClick.RemoveListener(OpenSettingPopup);
            _tutorialGo.SetActive(false);
        }

        public override void CloseSelf(bool destroy = false)
        {
            base.CloseSelf(destroy);
            _tutorialGo.SetActive(false);
        }

        public void CheckShowTutorial()
        {
            var isShowed = PlayerPrefs.GetInt(SHOW_TUTORIAL, 0) == 1;
            if (isShowed) return;
            _tutorialGo.SetActive(true);
        }


        public void HideTutorial()
        {
            _tutorialGo.SetActive(false);
            PlayerPrefs.SetInt(SHOW_TUTORIAL, 1);
        }



        public void ClearScoreboard()
        {
            _playerScoreboard.ClearData();
            _enemyScoreboard.ClearData();
        }


        public void ShowWinPopup(EGameMode gameMode)
        {
            _winPopup.SetGameMode(gameMode).OnShow(); 
        }
        

        public void ShowLosePopup(EGameMode gameMode)
        {
            _losePopup.SetGameMode(gameMode).OnShow();
        }


        #endregion


    }
}
