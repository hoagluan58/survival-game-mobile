using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System;
using System.Collections;
using UnityEngine;

namespace SquidGame.LandScape.BalanceBridge
{
    public class BalanceBridgeManager : BaseMinigameController
    {
        public static event Action OnShowTapLeft;
        public static event Action OnShowTapRight;
        public static event Action<bool> OnTapLeftHighlight;
        public static event Action<bool> OnTapRightHighlight;
        public static event Action<float> OnProgressChanged;

        [Header("--- References ---")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private CinemachineFreeLookInput _cinemachineFreeLookInput;
        [SerializeField] private GameObject _mapColliders, _confettiObject;

        private MinigameBalanceBridgeUI _minigameUI;
        private Coroutine _playTimerCoroutine;

        private void OnDestroy()
        {
            _minigameUI?.CloseSelf();
        }

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            _minigameUI = UIManager.I.Open<MinigameBalanceBridgeUI>(Define.UIName.MINIGAME_BALANCE_BRIDGE_MENU);
            GameManager.I.StartMinigame();
        }

        public void StartCountingToPlay(bool value, Action onDone = null)
        {
            _minigameUI.ActiveNotiTimeCount(value);
            if (value)
                _playTimerCoroutine = StartCoroutine(PlayTimerCounting());
            else
            {
                if (_playTimerCoroutine != null)
                    StopCoroutine(_playTimerCoroutine);
            }

            IEnumerator PlayTimerCounting()
            {
                int _startTime = 3;
                while (_startTime > 0)
                {
                    _minigameUI.UpdateCountText($"Game start in {_startTime} seconds");
                    _startTime--;
                    yield return new WaitForSeconds(1f);
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_TICK);
                }

                _minigameUI.UpdateCountText($"Start!");

                yield return new WaitForSeconds(1f);
                PrepareBeforeStartGame();
                onDone?.Invoke();
                _minigameUI.StartGame();
            }
        }

        void PrepareBeforeStartGame()
        {
            _playerMovement.enabled = false;
            _playerController.enabled = true;
            _playerController.SetActive(true);
            _mapColliders.SetActive(false);
        }

        public override void OnStart()
        {
            base.OnStart();
            _minigameUI.Init(_playerMovement, _cinemachineFreeLookInput);
            _playerController.Init(this);
        }

        public void Win()
        {
            _minigameUI.CloseSelf();
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);
            _confettiObject.SetActive(true);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
            DOVirtual.DelayedCall(3f, () =>
            {
                GameManager.I.Win();
            });
        }

        public void Lose()
        {
            _minigameUI.CloseSelf();
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Failure);

            DOVirtual.DelayedCall(2.5f, () =>
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_LOSING);
                GameManager.I.Lose();
            });
        }

        public override void OnRevive()
        {
            base.OnRevive();
            _minigameUI = UIManager.I.Open<MinigameBalanceBridgeUI>(Define.UIName.MINIGAME_BALANCE_BRIDGE_MENU);
            _minigameUI.OnRevive();
            _playerController.SetActive(true);
        }

        public void InvokeOnShowTapLeft() => OnShowTapLeft?.Invoke();
        public void InvokeOnShowTapRight() => OnShowTapRight?.Invoke();
        public void InvokeOnTapLeftHighlight(bool value) => OnTapLeftHighlight?.Invoke(value);
        public void InvokeOnTapRightHighlight(bool value) => OnTapRightHighlight?.Invoke(value);
        public void InvokeOnProgressChanged(float value) => OnProgressChanged?.Invoke(value);
    }
}