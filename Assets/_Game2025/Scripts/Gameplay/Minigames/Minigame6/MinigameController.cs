using Cysharp.Threading.Tasks;
using DinoFractureDemo;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.Minigame6.UI;
using SquidGame.LandScape.Utils;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6
{
    public class MinigameController : BaseMinigameController
    {
        private const float DELAY_START_GAME = 5f;

        [Header("REF")]
        [SerializeField] private Timer _timer;

        [Header("CONTROLLER")]
        [SerializeField] private MinigameIntro _intro;
        [SerializeField] private PlayerController _playerController;

        [Header("MANAGER")]
        [SerializeField] private CheckpointManager _checkpointManager;

        private IGameModeHandler _handler;
        private MinigameUI _ui;

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();

            InitGame().Forget();
        }

        public override void OnRevive()
        {
            base.OnRevive();
            _ui = UIManager.I.Open<MinigameUI>(Define.UIName.MINIGAME_06_MENU);
            _playerController.OnRevive();
            _checkpointManager.Revive();
        }

        private void OnDisable()
        {
            GameSound.I.StopBGM();
            _ui.CloseSelf();
        }

        private void StartGame()
        {
            _ui.TutorialPNL.Show();
            _timer.StartTimer();
            GameManager.I.StartMinigame();
        }

        public void Win()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
            GameManager.I.Win();
        }

        public void Lose()
        {
            _ui.CloseSelf();
            CheckRevive();
            GameSound.I.PlaySFX(Define.SoundPath.SFX_LOSING);
            GameManager.I.Lose();
        }

        public async UniTaskVoid InitGame()
        {
            GameSound.I.PlayBGM(Define.SoundPath.BGM_MINIGAME_06);
            _ui = UIManager.I.Open<MinigameUI>(Define.UIName.MINIGAME_06_MENU);
            _handler = GameManager.I.CurGameModeHandler;
            _checkpointManager.Init(this, _playerController.CharGroup, _ui);
            _playerController.Init(_checkpointManager);
            _timer.Init(DELAY_START_GAME, OnTimeChanged, OnTimeEnd);

            _ui.Init();
            _ui.SetJumpButtonCallback(OnJumpButtonClicked);

            await _intro.PlayIntro();

            StartGame();

            void OnTimeChanged(float time)
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_TICK);
                _ui.TutorialPNL.UpdateText($"Game will start in {time} seconds");
            }

            void OnTimeEnd()
            {
                _ui.SetActiveJumpButton(true);
                _ui.TutorialPNL.UpdateText($"Move to next minigame");
            }

            void OnJumpButtonClicked()
            {
                _playerController.MoveToNextCheckpoint();
                _ui.TutorialPNL.Hide();
            }
        }

        private void CheckRevive()
        {
            var isChallengeMode = _handler.GameMode == EGameMode.Challenge;
            if (!isChallengeMode) return;

            var handler = _handler as ChallengeMode;

            handler.CanRevive = true;
        }
    }
}
