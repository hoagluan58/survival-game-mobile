using DinoFractureDemo;
using NFramework;
using SquidGame.Core;
using SquidGame.LandScape.Game;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SquidGame.LandScape.Minigame4
{


    public enum MinigameResult
    {
        Win = 0,
        Lose = 1,
        Draw = 2,
    }

    public class MinigameController : BaseMinigameController
    {

        [Header("CONFIGS")]
        [SerializeField] private EGameMode _gameMode;
        [SerializeField] private LevelSaver _levelSaver;
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private float _introDelay;
        [SerializeField] private float _introMoveDuration;

        [Header("REFS")]
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private EnemyController _enemyController;
        [SerializeField] private Guard _guard;

        private Minigame04MenuUI _ui;
        private LevelContent _levelContent;


        #region OVERRIDE_METHODS

        //init
        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();

            Initialized();
            InitializedUI();
            PlayIntro();
        }

        public override void OnStart()
        {
            base.OnStart();
            _playerController.ResetMarbles();
            _enemyController.ResetMarbles();

            _ui.SetNotification("Your turn").ShowNotification(2f, false, 0.5f);
            _ui.CheckShowTutorial();
            _playerController.StartTurn();
        }


        public override void OnReload()
        {
            base.OnReload();
            _ui.CloseSelf(true);
        }

        public override void OnWin()
        {
            base.OnWin();

        }

        public override void OnLose()
        {
            base.OnLose();
        }

        private void LoseGame()
        {
            _playerController.SetActiveModel(true);
            _enemyController.Dance();
            _cameraController.FocusPlayer(0.25f, 0.5f, () => _guard.LookAtTarget(_playerController.Head).PlayAnimationFire(_playerController.Head, () =>
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
                _playerController.Dead();
            }).OnShootCompleted(() =>
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_LOSING);
                GameManager.I.Lose();
                _ui.CloseSelf();
            }));
        }


        private void WinGame()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
            _levelSaver.LevelUp(_gameMode);
            _playerController.SetActiveModel(true).RotateCharacter(Vector3.up * 180).Dance();
            _ui.SetNotification("You win !").ShowNotification(0, true, 0.35f);
            _cameraController.FocusPlayer(0.25f, 0.5f, () =>
            {
                this.InvokeDelay(2f, OnWinCompleted);
            });
        }


        private void OnWinCompleted()
        {
            _ui.CloseSelf();
            _guard.LookAtTarget(_enemyController.Head).PlayAnimationFire(_enemyController.Head, _enemyController.Dead).OnShootCompleted(() =>
            {
                GameManager.I.Win();
            });

        }


        public override void OnRevive()
        {
            base.OnRevive();
        }

        #endregion


        #region PRIVATE_METHODS

        private void Initialized()
        {
            _gameMode = GameManager.I.GetGameMode();
            _levelContent = _levelConfig.LoadConfigLevel(_levelSaver.GetLevel(_gameMode));

            _playerController.SetActiveModel(true);
            _playerController.OnEndTurn += OnPlayerEndTurn;
            _playerController.OnStartThrowMarble += OnStartThrowMarble;
            _playerController.OnInitialized();

            _enemyController.OnEndTurn += OnEnemyEndTurn;
            _enemyController.OnInitialized(_levelContent);
        }


        private void OnStartThrowMarble()
        {
            _ui.HideTutorial();
        }


        private bool IsCompletedMinigame()
        {
            return _playerController.IsCompleted() && _enemyController.IsCompleted();
        }


        private void InitializedUI()
        {
            _ui = UIManager.I.Open<Minigame04MenuUI>(Define.UIName.MINIGAME_04_MENU);
            _ui.Initialized(_gameMode, _levelContent);
            _ui.ShowScoreBoard(0f, false, 0f);

        }

        private void PlayIntro()
        {
            _ui.SetNotification("Throw the marble into the hole");
            _ui.ShowScoreBoard(_introDelay + _introMoveDuration / 2f, true, 0.5f);
            _cameraController.PlayIntro(_introDelay, _introMoveDuration, OnCompletedIntroCallback);
        }

        #endregion


        #region EVENT_CALLBACKS

        private void OnEnemyEndTurn(bool isScored)
        {
            _ui.SetEnemyScored(isScored);
            if (!IsCompletedMinigame())
            {
                _ui.ShowNotification(0, true, 0.1f).SetNotification("Your turn").ShowNotification(1f, false, 0.5f);
                _playerController.StartTurn();
            }
            else
            {
                CheckScore();
            }
        }


        private void OnPlayerEndTurn(bool isScored)
        {
            _ui.SetPlayerScored(isScored);
            if (!IsCompletedMinigame())
            {
                _ui.ShowNotification(0, true, 0.1f).SetNotification("Enemy turn").ShowNotification(1f, false, 0.5f);
                _enemyController.StartTurn();
            }
            else
            {
                CheckScore();
            }
        }


        private void OnCompletedIntroCallback()
        {
            _playerController.SetActiveModel(false);
            GameManager.I.StartMinigame();
            //OnStart();
        }


        private void CheckScore()
        {
            var result = GetResult();
            switch (result)
            {
                case MinigameResult.Win:
                    WinGame();
                    break;
                case MinigameResult.Lose:
                    LoseGame();
                    break;
                case MinigameResult.Draw:
                    OnDraw();
                    break;
                default:
                    break;
            }
        }


        private void OnDraw()
        {
            _ui.ClearScoreboard();
            _playerController.ResetMarbles();
            _enemyController.ResetMarbles();
            _ui.ShowNotification(0, true, 0.1f).SetNotification("Your turn").ShowNotification(1f, false, 0.5f);
            _playerController.StartTurn();
        }


        private MinigameResult GetResult()
        {
            var playerScore = _playerController.GetScore();
            var enemiesScore = _enemyController.GetScore();

            var result = playerScore == enemiesScore ? MinigameResult.Draw : playerScore > enemiesScore ? MinigameResult.Win : MinigameResult.Lose;
            return result;
        }


        #endregion
    }
}
