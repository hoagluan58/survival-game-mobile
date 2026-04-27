using Cinemachine;
using Cysharp.Threading.Tasks;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.Utils;
using System;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6.CardGame
{
    public class CardGameController : CheckpointController
    {
        [Header("REF")]
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private Timer _timer;
        [SerializeField] private GameObject _parentGO;

        [Header("CONTROLLER")]
        [SerializeField] private BaseGuard _guard;
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CardManager _cardManager;

        private AudioSource _audioSource;
        private EGameState _state;

        public override void OnEnter() => StartGame().Forget();

        public override void OnExit()
        {
            _audioSource?.Stop();
            _playerController.OnExit();
            _ui.ShowSucceedTMP(false);
        }

        public override void OnRevive()
        {
            base.OnRevive();
            OnEnter();
        }

        public async UniTaskVoid Win()
        {
            if (_state != EGameState.Playing) return;

            _state = EGameState.Win;
            _timer.StopTimer();
            _camera.gameObject.SetActive(false);
            _ui.HideTimeText();
            _ui.ShowSucceedTMP(true);
            _audioSource?.Stop();

            this.InvokeDelay(2f, () =>
            {
                _ui.ShowSucceedTMP(false);
                _ui.TutorialPNL.UpdateText($"Move to next minigame");
            });

            await UniTask.WaitUntil(() => !_cinemachineBrain.IsBlending);

            _parentGO.SetActive(false);
            _ui.SetActiveJumpButton(true);
        }

        public async UniTaskVoid Lose()
        {
            if (_state != EGameState.Playing) return;

            _state = EGameState.Lose;
            _audioSource?.Stop();

            _camera.gameObject.SetActive(false);
            await UniTask.WaitUntil(() => !_cinemachineBrain.IsBlending);
            _cardManager.FlipAllCard(false);

            await KillAllCharacters(_guard);
        }

        private async UniTaskVoid StartGame()
        {
            _camera.gameObject.SetActive(true);
            _parentGO.SetActive(true);
            await UniTask.WaitUntil(() => !_cinemachineBrain.IsBlending);

            // Delay a bit
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            _cardManager.OnEnter(this);
            _playerController.OnEnter(_cardManager);
            _levelGenerator.GenLevel(_cardManager);
            _timer.Init(_levelGenerator.LevelConfig.Time, OnTimeChanged, OnTimerEnd);
            _ui.ShowTimeText(_levelGenerator.LevelConfig.Time);

            await ShowCard();

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            _playerController.SetActive(true);
            _timer.StartTimer();
            _state = EGameState.Playing;

            async UniTask ShowCard()
            {
                var showTime = 2.5f;
                _cardManager.FlipAllCard(true);
                await UniTask.Delay(TimeSpan.FromSeconds(showTime));
                _cardManager.FlipAllCard(false);
            }

            void OnTimeChanged(float timeLeft)
            {
                _ui.UpdateTimeText(timeLeft);
                if (timeLeft == 5)
                {
                    _audioSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_COUNTDOWN_TIME);
                }
            }

            void OnTimerEnd() => Lose().Forget();
        }

        public enum EGameState
        {
            Playing,
            Lose,
            Win
        }
    }
}
