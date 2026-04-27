using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using UnityEngine;

namespace SquidGame.LandScape.Jumping
{
    public class JumpingGameManager : BaseMinigameController
    {
        #region SAVE
        public const string MINIGAME_LEVEL_KEY = "MINIGAME_JUMPING";
        public const string CHALLENGE_LEVEL_KEY = "CHALLENGE_JUMPING";

        private int GetCurrentLevel()
        {
            bool isMinigame = GameManager.I.CurGameModeHandler.GameMode == EGameMode.Minigame;
            return PlayerPrefs.GetInt(isMinigame ? MINIGAME_LEVEL_KEY : CHALLENGE_LEVEL_KEY, 1);
        }

        private void CompleteLevel()
        {
            bool isMinigame = GameManager.I.CurGameModeHandler.GameMode == EGameMode.Minigame;
            int currentLevel = GetCurrentLevel();
            PlayerPrefs.SetInt(isMinigame ? MINIGAME_LEVEL_KEY : CHALLENGE_LEVEL_KEY, currentLevel + 1);
        }
        #endregion

        #region CONFIG
        [Space(8)]
        [Header("--- CONFIG ---")]
        [SerializeField] private List<int> levelPlayTimeConfig;
        // [SerializeField] private List<int> levelRotateSpeedConfig;
        [SerializeField] private List<int> levelDirectionSwitchConfig;

        // public int GetRotateSpeed()
        // {
        //     if (GetCurrentLevel() <= levelRotateSpeedConfig.Count)
        //         return levelRotateSpeedConfig[GetCurrentLevel() - 1];
        //     else
        //         return levelRotateSpeedConfig[^1];
        // }

        public int GetIntervelDirectionSwitchTime()
        {
            if (GetCurrentLevel() <= levelDirectionSwitchConfig.Count)
                return levelDirectionSwitchConfig[GetCurrentLevel() - 1];
            else
                return levelDirectionSwitchConfig[^1];
        }

        public int GetLevelPlayTime()
        {
            if (GetCurrentLevel() <= levelPlayTimeConfig.Count)
                return levelPlayTimeConfig[GetCurrentLevel() - 1];
            else
                return levelPlayTimeConfig[^1];
        }
        #endregion

        #region MANAGER
        [Space(8)]
        [Header("--- REFERENCES ---")]
        [SerializeField] private CinemachineFreeLookInput _cinemachineFreeLookInput;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private MapElementHandler _mapElementHandler;

        private MinigameJumpingMenuUI _minigameUI;
        private AudioSource _losingAudioSource;

        private void OnDestroy()
        {
            PlayerController.OnPlayerDead -= OnPlayerDead;
            _losingAudioSource?.Stop();
            _minigameUI?.CloseSelf();
        }

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            _minigameUI = UIManager.I.Open<MinigameJumpingMenuUI>(Define.UIName.MINIGAME_JUMPING_MENU);
            _minigameUI.Init(_playerController, _cinemachineFreeLookInput, GetCurrentLevel());
            _mapElementHandler.Init(GetIntervelDirectionSwitchTime());

            PlayerController.OnPlayerDead += OnPlayerDead;
            GameManager.I.StartMinigame();
        }

        public override void OnStart()
        {
            base.OnStart();
            StartCoroutine(StartPlayingMinigame());
        }

        private void OnPlayerDead()
        {
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Failure);
            _minigameUI?.CloseSelf();
            _mapElementHandler.OnPlayerDead();

            if (timerCoroutine != null)
                StopCoroutine(timerCoroutine);

            DOVirtual.DelayedCall(2f, () =>
            {
                _losingAudioSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_LOSING);
                GameManager.I.Lose();
            });
        }

        private void Win()
        {
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);
            CompleteLevel();
            _minigameUI?.CloseSelf();
            _mapElementHandler.OnWin();
            _playerController.Win();

            // DOVirtual.DelayedCall(2f, () =>
            // {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
            GameManager.I.Win();
            // });
        }

        public override void OnRevive()
        {
            base.OnRevive();
            _minigameUI = UIManager.I.Open<MinigameJumpingMenuUI>(Define.UIName.MINIGAME_JUMPING_MENU);
            _playerController.Revive(maximumHealh: false);
            _playerController.SetActiveHealthBar(false);
            _mapElementHandler.OnPlayerRevive();
            _losingAudioSource?.Stop();
            timerCoroutine = StartCoroutine(StartCountingPlayTime());
        }
        #endregion

        #region TIMER
        TimeSpan timer;
        TimeSpan oneSecond = new(0, 0, 1);
        Coroutine timerCoroutine;

        IEnumerator StartPlayingMinigame()
        {
            int _startTime = 5;
            while (_startTime > 0)
            {
                _minigameUI.UpdateCountText($"Game start in {_startTime} seconds");
                _startTime--;
                yield return new WaitForSeconds(1f);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_TICK);
            }

            _minigameUI.UpdateCountText($"START!");
            yield return new WaitForSeconds(1f);
            _minigameUI.StartMinigame();
            _mapElementHandler.OnStart();

            timer = new(0, 0, GetLevelPlayTime());
            timerCoroutine = StartCoroutine(StartCountingPlayTime());
        }

        IEnumerator StartCountingPlayTime()
        {
            _minigameUI.ActiveNotiTimeCount(true);

            while (timer.TotalSeconds >= 0)
            {
                _minigameUI.UpdateTimerText($"{timer.Minutes:D2}:{timer.Seconds:D2}");
                timer = timer.Subtract(oneSecond);
                yield return new WaitForSeconds(1f);
            }

            Win();
        }

        // private void CheckRevive()
        // {
        //     if (GameManager.I.CurGameModeHandler.GameMode != EGameMode.Challenge) return;
        //     (GameManager.I.CurGameModeHandler as ChallengeMode).CanRevive = false;
        // }
        #endregion
    }
}
