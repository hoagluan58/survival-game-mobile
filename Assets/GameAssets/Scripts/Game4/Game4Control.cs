using System;
using System.Collections;
using NFramework;
using SquidGame;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.UI;
using UnityEngine;

namespace Game4
{
    public class Game4Control : BaseMinigameController
    {
        [Header("Configs")]
        [SerializeField] private GameLevel[] _allLevels;

        [Header("Refs")]
        [SerializeField] private BaseGuard _guard;
        [SerializeField] private PlayerController _playerControl;
        [SerializeField] private BotController _botController;
        [SerializeField] private Transform _posHole;
        [SerializeField] private ParticleSystem _scoreFx;

        [Header("Pos Enemy Ball")]
        [SerializeField] private Vector2 _randomPosEnemyX;
        [SerializeField] private Vector2 _randomPosEnemyZ;

        private Minigame04MenuUI _ui;

        private int _playerBallInGoal;
        private int _enemyBallInGoal;

        public int PlayerBallInGoal
        {
            get => _playerBallInGoal;
            set
            {
                if (value > _playerBallInGoal)
                {
                    VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG04_INTO_HOLE);
                    _scoreFx.Play();
                }
                _playerBallInGoal = Mathf.Max(0, value);
                _ui.PlayerBallInfoPanel.UpdateSuccessBallCount(value);
            }
        }
        public int EnemyBallInGoal
        {
            get => _enemyBallInGoal;
            set
            {
                if (value > _enemyBallInGoal)
                {
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG04_INTO_HOLE);
                    _scoreFx.Play();
                }
                _enemyBallInGoal = Mathf.Max(0, value);
                _ui.EnemyBallInfoPanel.UpdateSuccessBallCount(value);
            }
        }

        private void OnEnable()
        {
            _playerControl.OnEndTurn += OnPlayerEndTurn;
            _botController.OnEndTurn += OnBotEndTurn;
        }

        private void OnDisable()
        {
            _playerControl.OnEndTurn -= OnPlayerEndTurn;
            _botController.OnEndTurn -= OnBotEndTurn;
        }

        private void OnPlayerEndTurn()
        {
            if (IsEndGame())
            {
                EndGame();
            }
            else
            {
                _botController.OnStartTurn();
            }
        }

        private void OnBotEndTurn()
        {
            if (IsEndGame())
            {
                EndGame();
            }
            else
            {
                _playerControl.OnStartTurn();
            }
        }

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            UIManager.I.Close(Define.UIName.MINIGAME_04_MENU);
            _ui = UIManager.I.Open(Define.UIName.MINIGAME_04_MENU) as Minigame04MenuUI;
            _playerControl.Init(this, _ui.PlayerBallInfoPanel, _ui.TutorialPNL);
            _botController.Init(this, _ui.EnemyBallInfoPanel);
        }

        public override void OnStart()
        {
            base.OnStart();
            _playerControl.ResetBall();
            _botController.ResetBall();
            PlayerBallInGoal = 0;
            EnemyBallInGoal = 0;
            _playerControl.OnStartTurn();
        }

        public override void OnLose()
        {
            StartCoroutine(IE_GameOver());

            IEnumerator IE_GameOver()
            {
                UIManager.I.Close(Define.UIName.MINIGAME_04_MENU);
                _botController.PerformVictory();
                yield return new WaitForSeconds(2f);
                CameraControl.I.MoveToPosCamWinLose(0.5f);
                yield return new WaitForSeconds(0.5f);
                _playerControl.Lose();
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Failure);
                yield return new WaitForSeconds(1.5f);
                GameManager.I.HandleResult();
            }
        }

        public override void OnRevive() => GameManager.I.ReloadMinigame();

        private bool IsEndGame()
        {
            return _playerControl.IsThrewAllBalls && _botController.IsThrewAllBalls;
        }

        public void EndGame()
        {
            if (PlayerBallInGoal > EnemyBallInGoal) // Win
            {
                StartCoroutine(WinGameCoroutine());
            }
            else if (PlayerBallInGoal == EnemyBallInGoal) // Draw
            {
                _playerControl.ResetBall();
                _botController.ResetBall();
                _playerControl.OnStartTurn();
            }
            else // Lose
            {
                GameManager.I.Lose();
            }


            IEnumerator WinGameCoroutine()
            {
                UIManager.I.Close(Define.UIName.MINIGAME_04_MENU);
                _guard.PlayShootAnim();
                _botController.PerformDie();
                yield return new WaitForSeconds(2f);
                CameraControl.I.MoveToPosCamWinLose(0.5f);
                yield return new WaitForSeconds(0.5f);
                _playerControl.Win();
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);
                CameraControl.I.transform.GetChild(0).gameObject.SetActive(true); // Play win fx
                var winSoundSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
                yield return new WaitForSeconds(4f);
                winSoundSource.Stop();
                GameManager.I.Win();
            }
        }
    }


    [Serializable]
    public class GameLevel
    {
        public int PlayerBall;
        public int EnemyBall;
        public int EnemyBallInGoal;
    }
}