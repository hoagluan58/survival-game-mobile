using Cinemachine;
using Cysharp.Threading.Tasks;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.Utils;
using System;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6.Ddakji
{
    public class DdakjiGameController : CheckpointController
    {
        public enum EGameState
        {
            Playing,
            Lose,
            Win,
        }

        [SerializeField] private Timer _timer;

        [Header("REF")]
        [SerializeField] private BaseGuard _guard;
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private DdakjiHandler _handler;
        [SerializeField] private DdakjiGameConfigSO _config;
        [SerializeField] private GameObject _parentGO;

        private AudioSource _audioSource;
        private EGameState _curState;
        private DdakjiGameUI _ddakjiGameUI;

        public EGameState CurState => _curState;

        public override void OnEnter()
        {
            _ddakjiGameUI = _ui.DdakjiGameUI;
            _ddakjiGameUI.Init(_handler);
            _ui.SetActiveJumpButton(false);
            _handler.OnEnter(this, _ddakjiGameUI);

            StartGame().Forget();
        }

        public override void OnExit()
        {
            _ui.SetActiveDdakjiUI(false);
            _audioSource?.Stop();
            _ui.ShowSucceedTMP(false);
        }

        public override void OnRevive()
        {
            base.OnRevive();
            OnEnter();
        }

        public async UniTaskVoid Win()
        {
            if (_curState != EGameState.Playing) return;

            SetGameState(EGameState.Win);

            _ui.TutorialPNL.Hide();
            _ui.HideTimeText();
            _ui.ShowSucceedTMP(true);
            _audioSource?.Stop();

            this.InvokeDelay(2f, () =>
            {
                _ui.ShowSucceedTMP(false);
                _ui.TutorialPNL.UpdateText($"Move to the finish line");
            });

            _handler.Disable();
            _timer.StopTimer();

            var delay = 1f;
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            _virtualCamera.gameObject.SetActive(false);
            await UniTask.WaitUntil(() => !_cinemachineBrain.IsBlending);

            _parentGO.SetActive(false);
            _ui.SetActiveJumpButton(true);
        }

        public async UniTaskVoid Lose()
        {
            if (_curState != EGameState.Playing) return;
            SetGameState(EGameState.Lose);

            _ui.HideTimeText();
            _ui.TutorialPNL.Hide();
            _handler.Disable();
            _audioSource?.Stop();
            _ui.SetActiveDdakjiUI(false);

            _virtualCamera.gameObject.SetActive(false);
            await UniTask.WaitUntil(() => !_cinemachineBrain.IsBlending);

            await KillAllCharacters(_guard);
        }

        public async UniTaskVoid StartGame()
        {
            // Intro
            _ui.SetActiveDdakjiUI(true);
            _virtualCamera.gameObject.SetActive(true);
            await UniTask.WaitUntil(() => !_cinemachineBrain.IsBlending);

            // Init data

            _timer.Init(_config.PlayTime, OnTimeChanged, OnTimeEnd);
            _ui.ShowTimeText(_config.PlayTime);
            _ui.TutorialPNL.Show("Tap To Play");

            // Delay and start game
            var delayTime = 1f;
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));

            _timer.StartTimer();
            _handler.StartMovingTarget();

            void OnTimeChanged(float time)
            {
                _ui.UpdateTimeText(time);
                if (time == 5)
                {
                    _audioSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_COUNTDOWN_TIME);
                }
            }

            void OnTimeEnd() => Lose().Forget();
        }

        public void SetGameState(EGameState state) => _curState = state;
    }
}
