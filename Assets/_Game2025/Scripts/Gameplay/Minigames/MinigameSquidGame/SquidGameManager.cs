using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using UnityEngine;

namespace SquidGame.LandScape.SquidGame
{
    public class SquidGameManager : BaseMinigameController
    {
        #region SAVE
        public const string MINIGAME_LEVEL_KEY = "MINIGAME_SQUID_GAME";
        public const string CHALLENGE_LEVEL_KEY = "CHALLENGE_SQUID_GAME";

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
        [SerializeField] private List<int> levelPlayerHealthConfig;
        [SerializeField] private List<int> levelEnemyHealthConfig;
        [SerializeField] private List<int> levelObsacleAmountConfig;

        public int GetPlayerHealth()
        {
            if (GetCurrentLevel() <= levelPlayerHealthConfig.Count)
                return levelPlayerHealthConfig[GetCurrentLevel() - 1];
            else
                return levelPlayerHealthConfig[^1];
        }

        public int GetEnemyHealth()
        {
            if (GetCurrentLevel() <= levelEnemyHealthConfig.Count)
                return levelEnemyHealthConfig[GetCurrentLevel() - 1];
            else
                return levelEnemyHealthConfig[^1];
        }

        public int GetLevelPlayTime()
        {
            if (GetCurrentLevel() <= levelPlayTimeConfig.Count)
                return levelPlayTimeConfig[GetCurrentLevel() - 1];
            else
                return levelPlayTimeConfig[^1];
        }

        public int GetObsacleAmount()
        {
            if (GetCurrentLevel() <= levelObsacleAmountConfig.Count)
                return levelObsacleAmountConfig[GetCurrentLevel() - 1];
            else
                return levelObsacleAmountConfig[^1];
        }
        #endregion

        #region MANAGER
        [Space(8)]
        [Header("--- REFERENCES ---")]
        [SerializeField] private CinemachineFreeLookInput _cinemachineFreeLookInput;
        [SerializeField] private MapElementHandler _mapElementHandler;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private EnemyUnit _enemyUnit;
        [SerializeField] private GameObject _gateBlockObject, _weaponHolder;
        [SerializeField] private Transform _playerHeadTransform;

        private bool _isPlayerEquipWeapon;
        private MinigameSquidGameUI _minigameUI;
        private AudioSource _losingAudioSource;
        #endregion

        private void OnDestroy()
        {
            PlayerController.OnPlayerDead -= OnPlayerDead;
            PlayerController.OnPlayerEquipWeapon -= OnPlayerEquipWeapon;

            EnemyUnit.OnEnemyDead -= Win;
            _losingAudioSource?.Stop();
            _minigameUI?.CloseSelf(true);
        }

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            _mapElementHandler.OnLoadMinigame(GetObsacleAmount());
            _minigameUI = UIManager.I.Open<MinigameSquidGameUI>(Define.UIName.MINIGAME_SQUID_GAME_MENU);
            _minigameUI.Init(_playerController, _cinemachineFreeLookInput, GetCurrentLevel());

            _playerController.MaximumHealth(GetPlayerHealth());
            _playerController.SetHealth(0);

            _enemyUnit.MaximumHealth(GetEnemyHealth());
            GameManager.I.StartMinigame();

            PlayerController.OnPlayerDead += OnPlayerDead;
            PlayerController.OnPlayerEquipWeapon += OnPlayerEquipWeapon;

            EnemyUnit.OnEnemyDead += Win;
        }

        public override void OnStart()
        {
            base.OnStart();
            StartCoroutine(StartPlayingMinigame());
        }

        private void OnPlayerEquipWeapon()
        {
            _isPlayerEquipWeapon = true;
            _gateBlockObject.SetActive(false);
            _weaponHolder.SetActive(false);
            _minigameUI.OnPlayerEquipWeapon();
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }
        }

        private void OnPlayerDead()
        {
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Failure);
            _minigameUI.CloseSelf();
            _enemyUnit.OnPlayerDead();

            if (timerCoroutine != null)
                StopCoroutine(timerCoroutine);
            else CheckRevive();

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
            _minigameUI.CloseSelf();
            _playerController.SetActiveHealthBar(false);
            DOVirtual.DelayedCall(2f, () =>
            {
                _playerController.Win();
                GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
                GameManager.I.Win();
            });
        }

        public override void OnRevive()
        {
            base.OnRevive();
            _minigameUI = UIManager.I.Open<MinigameSquidGameUI>(Define.UIName.MINIGAME_SQUID_GAME_MENU);
            _enemyUnit.OnPlayerRevive();

            _playerController.Revive(maximumHealh: false);
            _playerController.SetImmortalInSecond(2f);

            if (!_isPlayerEquipWeapon)
                timerCoroutine = StartCoroutine(StartCountingPlayTime());

            _losingAudioSource?.Stop();
        }

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

            _minigameUI.UpdateCountText($"Come to the enemy ahead!");
            yield return new WaitForSeconds(1f);
            _minigameUI.StartMinigame();

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

            CheckRevive();
            _minigameUI.CloseSelf();
            _playerController.Die();
            _mapElementHandler.OnTimeout(_playerHeadTransform);
        }

        private void CheckRevive()
        {
            if (GameManager.I.CurGameModeHandler.GameMode != EGameMode.Challenge) return;
            (GameManager.I.CurGameModeHandler as ChallengeMode).CanRevive = false;
        }
        #endregion
    }
}
