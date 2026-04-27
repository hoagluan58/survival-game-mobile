using Cinemachine;
using Cysharp.Threading.Tasks;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.Utils;
using System;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6.ThrowStoneGame
{
    public class ThrowStoneGameController : CheckpointController
    {
        private const float PLAY_TIME = 25f;

        [Header("REF")]
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        [SerializeField] private GameObject _parentGO;
        [SerializeField] private BaseGuard _guard;

        [Header("CONTROLLER")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Target _target;
        [SerializeField] private Timer _timer;

        private AudioSource _audioSource;
        private ThrowStoneUI _throwStoneUI;

        private EGameState _curState;
        public EGameState CurState => _curState;

        public override void OnEnter()
        {
            _throwStoneUI = _ui.ThrowStoneUI;
            _ui.SetActiveJumpButton(false);
            _ui.SetActiveThrowStoneUI(false);

            StartGame().Forget();
        }

        public override void OnExit()
        {
            _audioSource?.Stop();
            _ui.SetActiveThrowStoneUI(false);
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
            _curState = EGameState.Win;
            _timer.StopTimer();
            _audioSource?.Stop();

            _ui.TutorialPNL.Hide();
            _ui.SetActiveThrowStoneUI(false);
            _ui.HideTimeText();
            _ui.ShowSucceedTMP(true);

            this.InvokeDelay(2f, () =>
            {
                _ui.ShowSucceedTMP(false);
                _ui.TutorialPNL.UpdateText($"Move to next minigame");
            });

            var delay = 1f;
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            _camera.gameObject.SetActive(false);
            await UniTask.WaitUntil(() => !_cinemachineBrain.IsBlending);

            _parentGO.SetActive(false);
            _ui.SetActiveJumpButton(true);
        }

        public async UniTaskVoid Lose()
        {
            if (_curState != EGameState.Playing) return;

            _curState = EGameState.Lose;

            _ui.TutorialPNL.Hide();
            _ui.SetActiveThrowStoneUI(false);
            _ui.HideTimeText();
            _audioSource?.Stop();

            _camera.gameObject.SetActive(false);
            await UniTask.WaitUntil(() => !_cinemachineBrain.IsBlending);

            await KillAllCharacters(_guard);
        }

        private async UniTaskVoid StartGame()
        {
            _camera.gameObject.SetActive(true);

            await UniTask.WaitUntil(() => !_cinemachineBrain.IsBlending);

            _ui.TutorialPNL.Show("Throw the stone to hit the target");
            _playerController.OnEnter(this, _throwStoneUI);
            _target.OnEnter(this);
            _timer.Init(PLAY_TIME, OnTimeChanged, OnTimeEnd);
            _ui.ShowTimeText(PLAY_TIME);

            var delayTime = 2f;
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));

            _ui.SetActiveThrowStoneUI(true);
            _curState = EGameState.Playing;
            _timer.StartTimer();
            _playerController.StartGame();

            void OnTimeChanged(float timeLeft)
            {
                _ui.UpdateTimeText(timeLeft);
                if (timeLeft == 5)
                {
                    _audioSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_COUNTDOWN_TIME);
                }
            }

            void OnTimeEnd() => Lose().Forget();
        }

        public enum EGameState
        {
            Win,
            Lose,
            Playing,
        }
    }
}