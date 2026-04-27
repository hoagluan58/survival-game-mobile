using Cysharp.Threading.Tasks;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System;
using UnityEngine;

namespace SquidGame.LandScape.MinigameFindMarbles
{
    public class MinigameController : BaseMinigameController
    {
        [Header("REF")]
        [SerializeField] private BaseGuard _guard;
        [SerializeField] private Player _player;

        [Header("CONTROLLER")]
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private RoundController _roundController;
        [SerializeField] private BowlManager _bowlManager;

        private MinigameUI _ui;
        private IGameModeHandler _handler;
        private LevelSaveData _saveData;

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            InitGame().Forget();

            async UniTaskVoid InitGame()
            {
                _cameraController.Init(CameraController.ECameraType.Character);
                _handler = GameManager.I.CurGameModeHandler;
                _saveData = new LevelSaveData(_handler.GameMode);
                _roundController.Init(this, _bowlManager, _saveData.Level);

                _ui = UIManager.I.Open<MinigameUI>(Define.UIName.MINIGAME_FIND_MARBLES_MENU);
                _ui.SetData(_saveData.Level);

                _roundController.PlayNextRound();
                _ui.UpdateRoundText(_roundController.CurRound);

                await UniTask.Delay(1000);
                StartGame().Forget();
            }
        }

        public override void OnRevive()
        {
            HandleOnRevive().Forget();

            async UniTaskVoid HandleOnRevive()
            {
                var curRound = _roundController.CurRound;
                _player.OnRevive();
                _roundController.PlayRound(curRound);

                await _cameraController.CRSwitchCamera(CameraController.ECameraType.Game);
                ShuffleBowl();
            }
        }

        private void OnEnable()
        {
            _bowlManager.ClickedCorrectBowl += OnClickedCorrectBowl;
            _bowlManager.ClickedWrongBowl += OnClickedWrongBowl;
        }

        private void OnDisable()
        {
            _bowlManager.ClickedCorrectBowl -= OnClickedCorrectBowl;
            _bowlManager.ClickedWrongBowl -= OnClickedWrongBowl;
        }

        private async UniTaskVoid StartGame()
        {
            await _cameraController.CRSwitchCamera(CameraController.ECameraType.Game);
            ShuffleBowl();
            GameManager.I.StartMinigame();
        }

        private void GoNextRound()
        {
            _roundController.PlayNextRound();
            _ui.UpdateRoundText(_roundController.CurRound);

            this.InvokeDelay(1.5f, ShuffleBowl);
        }

        private void ShuffleBowl() => _roundController.StartRound().Forget();

        private void OnClickedCorrectBowl()
        {
            HandleCorrectBowl().Forget();

            async UniTaskVoid HandleCorrectBowl()
            {
                await UniTask.Delay(1000);
                if (_roundController.IsMaxRound())
                {
                    WinGame().Forget();
                }
                else
                {
                    GoNextRound();
                }
            }
        }

        private void OnClickedWrongBowl()
        {
            HandleWrongBowl().Forget();

            async UniTaskVoid HandleWrongBowl()
            {
                await UniTask.Delay(1000);
                CheckRevive();
                LoseGame().Forget();
            }
        }

        public async UniTaskVoid WinGame()
        {
            _ui.CloseSelf();
            await _cameraController.CRSwitchCamera(CameraController.ECameraType.Character);
            _player.OnWin();
            _saveData.Save();

            await UniTask.Delay(2000);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
            GameManager.I.Win();
        }

        public async UniTaskVoid LoseGame()
        {
            _ui.CloseSelf();
            await _cameraController.CRSwitchCamera(CameraController.ECameraType.Character);
            VibrationManager.I.Haptic(VibrationManager.EHapticType.MediumImpact);

            GuardShoot();
            _player.OnDie();

            await UniTask.Delay(2000);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_LOSING);
            GameManager.I.Lose();

            void GuardShoot()
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
                _guard.transform.LookAt(_player.transform.position);
                _guard.PlayShootAnim();
                _guard.ShowLine(0f, _player.HeadPos);
                _guard.ClearLine(1f);
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
