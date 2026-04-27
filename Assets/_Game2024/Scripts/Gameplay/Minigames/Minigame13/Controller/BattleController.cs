using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.Minigame13.UI;
using System.Collections;
using UnityEngine;

namespace SquidGame.Minigame13
{
    public class BattleController : MonoBehaviour
    {
        private const int MAX_SCORE = 2;

        private Minigame13MenuUI _ui;
        private MinigameController _controller;
        private EResult _result;
        private EChoice _botChoice;
        private EChoice _playerChoice;
        private int _playerScore;
        private int _botScore;
        private string _botName;
        private bool _isUserTurn;

        public string BotName => _botName;

        public void Init(MinigameController controller, Minigame13MenuUI ui)
        {
            _controller = controller;
            _ui = ui;
            _botName = "No. " + Random.Range(0, 500);
        }

        public void StartBattle()
        {
            _isUserTurn = false;
            _ui.ShowChoicePNL(_isUserTurn);
        }

        public void SetBotChoice(EChoice choice)
        {
            _botChoice = choice;
            _isUserTurn = true;

            // Wait a bit then user turn 
            this.InvokeDelay(1f, () =>
            {
                _ui.ShowChoicePNL(_isUserTurn);
            });
        }

        public void SetPlayerChoice(EChoice choice)
        {
            _playerChoice = choice;
            HandleResult();
        }

        private void HandleResult()
        {
            StartCoroutine(CRHandleResult());

            IEnumerator CRHandleResult()
            {
                yield return new WaitForSeconds(1f);
                if (_botChoice == _playerChoice)
                {
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG13_GAME_DRAW);
                    _result = EResult.Draw;
                }
                else if ((_playerChoice == EChoice.Rock && _botChoice == EChoice.Scissors)
                         || (_playerChoice == EChoice.Paper && _botChoice == EChoice.Rock)
                         || (_playerChoice == EChoice.Scissors && _botChoice == EChoice.Paper))
                {
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG14_CORRECT);
                    _result = EResult.Win;
                    _playerScore++;
                }
                else
                {
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG14_WRONG);
                    _result = EResult.Lose;
                    _botScore++;
                }

                _ui.UpdateScore(_playerScore, _botScore);
                _ui.ShowRoundResult(_result);
                yield return new WaitForSeconds(2f);
                _ui.HideRoundResultTMP();
                TryEndBattle();
            }
        }

        private void TryEndBattle()
        {
            if (_playerScore == MAX_SCORE)
            {
                GameManager.I.Win(4f);
            }
            else if (_botScore == MAX_SCORE)
            {
                GameManager.I.Lose();
            }
            else
            {
                this.InvokeDelay(1f, () =>
                {
                    StartBattle();
                });
            }
        }
    }

    public enum EChoice
    {
        Rock,
        Scissors,
        Paper,
    }

    public enum EResult
    {
        Win,
        Lose,
        Draw,
    }
}


