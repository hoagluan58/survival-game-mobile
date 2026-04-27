using DinoFractureDemo;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System.Collections;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMarblesVer2
{

    public class Marblesver2Controller : BaseMinigameController
    {
        [SerializeField] private Player _player;
        [SerializeField] private Opponent _opponent;
        [SerializeField] private Guard _guard;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private LevelSaver _levelSaver;

        private MinigameMarblesTwoMenuUI _ui;
        public MinigameMarblesTwoMenuUI UI => _ui;
        public int Level => _levelSaver.GetLevel(GameManager.I.GetGameMode());

        private void OnEnable()
        {
            _player.OnStartTurn += OnPlayerStartTurn;
            _player.OnEndTurn += OnPlayerEndTurn;

            _opponent.OnStartTurn += OnOpponentStartTurn;
            _opponent.OnEndTurn += OnOpponentEndTurn;
        }


        private void OnDisable()
        {
            _player.OnStartTurn -= OnPlayerStartTurn;
            _player.OnEndTurn -= OnPlayerEndTurn;

            _opponent.OnStartTurn -= OnOpponentStartTurn;
            _opponent.OnEndTurn -= OnOpponentEndTurn;
        }


        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            _player.Init(this);
            _opponent.Init(this);
            _cameraController.Init();
            _ui = UIManager.I.Open<MinigameMarblesTwoMenuUI>(Define.UIName.MINIGAME_MARBLESTWO_MENU);
            _ui.Startgame(Level);
            _cameraController.PlayIntro(() =>
            {
                _ui.SetActiveStartButton(true);
            });
        }


        public override void OnRevive()
        {
            base.OnRevive();

            _player.Revive();
            _opponent.Revive();
            _player.SetMarble(3);
            _opponent.SetMarble(3);

            _guard.ResetToDefault();

            _ui = UIManager.I.Open<MinigameMarblesTwoMenuUI>(Define.UIName.MINIGAME_MARBLESTWO_MENU);
            _ui.ClearPanel();
            _ui.ShowNotification("Your turn !", 1f);

            _player.StartTurn();
        }

        public override void OnStart()
        {
            base.OnStart();
            _player.StartTurn();
        }


        #region CALLBACK

        private void OnPlayerEndTurn()
        {
            _ui.PlayShowScoreAnimation(_player.CurrentTurnScore, _opponent.StartTurn);
        }


        private void OnPlayerStartTurn()
        {
            _ui.ShowNotification("Your turn !", 1f);
        }


        private bool IsCompletedMinigame()
        {
            return _player.IsCompleted() && _opponent.IsCompleted();
        }


        private void OnOpponentEndTurn()
        {
            _ui.PlayShowScoreAnimation(_opponent.CurrentTurnScore,() => {
                StartCoroutine(CRCheckResult());
            });

            IEnumerator CRCheckResult()
            {
                var status = _player.CurrentTurnScore > _opponent.CurrentTurnScore ? GameStatus.Win : _player.CurrentTurnScore == _opponent.CurrentTurnScore ? GameStatus.Draw : GameStatus.Lose;
                _ui.PlayerPanel.Scored(status == GameStatus.Win || status == GameStatus.Draw);
                _ui.OpponentPanel.Scored(status == GameStatus.Lose || status == GameStatus.Draw);
                _player.TotalScore = status == GameStatus.Win || status == GameStatus.Draw ? _player.TotalScore + 1 : _player.TotalScore;
                _opponent.TotalScore = status == GameStatus.Lose || status == GameStatus.Draw ? _opponent.TotalScore + 1 : _opponent.TotalScore;

                if (IsCompletedMinigame())
                {
                    yield return new WaitForSeconds(1);
                    var gameStatus = _player.TotalScore == _opponent.TotalScore ? GameStatus.Draw : _player.TotalScore > _opponent.TotalScore ? GameStatus.Win : GameStatus.Lose;
                    switch (gameStatus)
                    {
                        case GameStatus.Draw:
                            Draw();
                            break;
                        case GameStatus.Win:
                            Win();
                            break;
                        case GameStatus.Lose:
                            Lose();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    _player.StartTurn();
                }
            }
        }


        private void Draw()
        {
            _ui.ClearPanel();
            _player.SetMarble(3);
            _opponent.SetMarble(3);
            _player.StartTurn();
        }


        private void Win()
        {
            _levelSaver.LevelUp(GameManager.I.GetGameMode());
            GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
            _ui.CloseSelf();
            _guard.LookAt(_opponent.Head()).PlayShootAnim().ShowLine(0.25f, _opponent.Head(), () => {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_M4_SHOOT);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_F_HIT_01);
                _opponent.OnLose();
            }).ClearLine(0.45f);
            _player.OnWin();
            _cameraController.OnWin(1, () =>
            {
                _cameraController.DropConfettiFx();
                this.InvokeDelay(3f, () =>
                {
                    GameManager.I.Win();
                });
            });

        }


        private void Lose()
        {
            _ui.CloseSelf();
            _guard.LookAt(_player.Head()).PlayShootAnim().ShowLine(0.25f, _player.Head(), () => {
                _player.OnLose();
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_M4_SHOOT);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_F_HIT_01);
            }).ClearLine(0.45f);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_LOSING);
            this.InvokeDelay(2f, () =>
            {
                GameManager.I.Lose();
            });
        }


        private void OnOpponentStartTurn()
        {
            _ui.ShowNotification("Opponent turn !", 1f);
        }

        #endregion

    }

    public enum GameStatus
    {
        Draw,
        Win,
        Lose
    }
}
