using SquidGame.LandScape.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame4
{
    public class LosePopup : BasePopup
    {
        public event Action OnSeasonHomeClickAction;
        public event Action OnSeasonReviveClickAction;
        public event Action OnSeasonLoseClickAction;

        public event Action OnMinigameReplayClickAction;
        public event Action OnMinigameHomeClickAction;


        [Header("References")]
        [SerializeField] private Transform _content;
        [SerializeField] private GameObject _seasonGo;
        [SerializeField] private GameObject _minigameGo;

        [Header("Season")]
        [SerializeField] private Button _seasonReviveButton;
        [SerializeField] private Button _seasonLoseButton;
        [SerializeField] private Button _seasonHomeButton;

        [Header("Minigame")]
        [SerializeField] private Button _minigameReplayButton;
        [SerializeField] private Button _minigameHomeButton;


        public override void OnInitialize()
        {
            base.OnInitialize();
            _seasonReviveButton.onClick.AddListener(OnSeasonReviveButtonClicked);
            _seasonLoseButton.onClick.AddListener(OnSeasonLoseButtonClicked);
            _seasonHomeButton.onClick.AddListener(OnSeasonHomeButtonClicked);

            _minigameReplayButton.onClick.AddListener(OnMinigameReplayButtonClicked);
            _minigameHomeButton.onClick.AddListener(OnMinigameHomeButtonClicked);

            _content.gameObject.SetActive(false);
        }

        public override BasePopup SetGameMode(EGameMode gameMode)
        {
            _gameMode = gameMode;
            _seasonGo.SetActive(gameMode == EGameMode.Challenge);
            _minigameGo.SetActive(gameMode == EGameMode.Minigame);
            return this;
        }

        public override void OnShow()
        {
            base.OnShow();
            _content.gameObject.SetActive(true);
        }

        public override void OnHide()
        {
            base.OnHide();
            _content.gameObject.SetActive(false);
        }


        private void OnSeasonHomeButtonClicked()
        {
            OnSeasonHomeClickAction?.Invoke();
        }

        private void OnSeasonLoseButtonClicked()
        {
            OnSeasonLoseClickAction?.Invoke();
        }

        private void OnSeasonReviveButtonClicked()
        {
            OnSeasonReviveClickAction?.Invoke();
        }



        private void OnMinigameHomeButtonClicked()
        {
            OnMinigameHomeClickAction?.Invoke();
        }


        private void OnMinigameReplayButtonClicked()
        {
            OnMinigameReplayClickAction?.Invoke();
        }


    }
}
