using SquidGame.LandScape.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame4
{
    public class WinPopup : BasePopup
    {
        public event Action OnMinigameNextLevelClickAction;
        public event Action OnMinigameHomeClickAction;
        public event Action OnSeasonHomeClickAction;
        public event Action OnSeasonNextClickAction;

        [Header("References")]
        [SerializeField] private Transform _content;
        [SerializeField] private GameObject _seasonGo;
        [SerializeField] private GameObject _minigameGo;

        [Header("Season")]
        [SerializeField] private Button _seasonHomeButton;
        [SerializeField] private Button _seasonNextButton;

        [Header("Minigames")]
        [SerializeField] private RewardMover _rewardMover;
        [SerializeField] private Button _adButton;
        [SerializeField] private TextMeshProUGUI _adValueText;
        [SerializeField] private Button _minigameHomeButton;
        [SerializeField] private Button _minigameNextButton;

        private const int _defaultRewardValue = 100;
        private int _currentRewardValue;
        private bool _isFeatureOn; 
        
        public override void OnInitialize()
        {
            base.OnInitialize();
            _seasonHomeButton.onClick.AddListener(OnSeasonHomeButtonClicked);
            _seasonNextButton.onClick.AddListener(OnSeasonNextButtonClicked);

            _adButton.onClick.AddListener(OnAdButtonClicked);
            _rewardMover.OnItemChanged.AddListener(OnRewardValueChanged);
            _minigameHomeButton.onClick.AddListener(OnMinigameHomeButtonClicked);
            _minigameNextButton.onClick.AddListener(OnMinigameNextButtonClicked);

            _content.gameObject.SetActive(false);
        }


        public override BasePopup SetGameMode(EGameMode gameMode)
        {
            _gameMode = gameMode;
            _seasonGo.SetActive(_gameMode == EGameMode.Challenge);
            _minigameGo.SetActive(_gameMode != EGameMode.Challenge);
            return this;
        }


        public override void OnShow()
        {
            base.OnShow();
            _content.gameObject.SetActive(true);
            EnableAllFeatures(true);
        }


        public override void OnHide()
        {
            base.OnHide();
            _content.gameObject.SetActive(false);
        }


        public void EnableAllFeatures(bool value)
        {
            _isFeatureOn = value;
        }


        public void IncreaseMoney(int value , UnityAction onCompleted)
        {
            Debug.LogError("IncreaseMoney");
            onCompleted?.Invoke();
        }


        #region UI EVENTS

        private void OnMinigameNextButtonClicked()
        {
            if (!_isFeatureOn) return;
            EnableAllFeatures(false);
            IncreaseMoney(_defaultRewardValue, () => OnMinigameNextLevelClickAction?.Invoke());
        }


        private void OnMinigameHomeButtonClicked()
        {
            if (!_isFeatureOn) return;
            EnableAllFeatures(false);
            IncreaseMoney(_defaultRewardValue, () => OnMinigameHomeClickAction?.Invoke());
        }


        private void OnRewardValueChanged(MultipleReward rewardValue)
        {
            _currentRewardValue = _defaultRewardValue * rewardValue.MultipleTime;
            _adValueText.text = $"{_currentRewardValue}";
        }

        private void OnAdButtonClicked()
        {
            if (!_isFeatureOn) return;
            EnableAllFeatures(false);
            _rewardMover.Pause();
            IncreaseMoney(_currentRewardValue, () => OnMinigameNextLevelClickAction?.Invoke());
        }

        private void OnSeasonNextButtonClicked()
        {
            if (!_isFeatureOn) return;
            EnableAllFeatures(false);
            Debug.LogError("OnSeasonNextButtonClicked");
            OnSeasonNextClickAction?.Invoke();
        }

        private void OnSeasonHomeButtonClicked()
        {
            if (!_isFeatureOn) return;
            Debug.LogError("OnSeasonHomeButtonClicked");
            EnableAllFeatures(false);
            OnSeasonHomeClickAction?.Invoke();
        }

        #endregion
    }
}
