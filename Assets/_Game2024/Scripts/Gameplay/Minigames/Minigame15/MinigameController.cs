using DG.Tweening;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using System.Collections;
using UnityEngine;

namespace SquidGame.Minigame15
{
    public class MinigameController : BaseMinigameController
    {
        [SerializeField] private MarblesManager _marblesManager;
        [SerializeField] private Player _player;
        [SerializeField] private Opponent _opponent;
        [SerializeField] private ParticleSystem _winFx;
        [SerializeField] private BaseGuard _guard;
        [SerializeField] private Transform _winCameraPosition;

        private int _playerRoundWinCount, _opponentRoundWinCount;
        private Minigame15MenuUI _minigame15MenuUI;

        private void OnEnable()
        {
            _player.EndTurnEvent += OnPlayerEndTurn;
            _opponent.EndTurnEvent += OnOpponentEndTurn;
        }

        private void OnDisable()
        {
            _player.EndTurnEvent -= OnPlayerEndTurn;
            _opponent.EndTurnEvent -= OnOpponentEndTurn;
        }

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            _minigame15MenuUI = UIManager.I.Open(Define.UIName.MINIGAME_15_MENU) as Minigame15MenuUI;
            if (_minigame15MenuUI != null)
            {
                _opponent.Init(_marblesManager, _minigame15MenuUI);
                _player.Init(_marblesManager, _minigame15MenuUI);
            }
        }

        public override void OnStart()
        {
            base.OnStart();
            _player.OnStartTurn();
        }

        private IEnumerator WinGameCoroutine()
        {
            _minigame15MenuUI.CloseSelf();
            _guard.PlayShootAnim();
            _opponent.PlayLoseAnimation();
            yield return new WaitForSeconds(1f);
            Camera.main.transform.DOMove(_winCameraPosition.position, 1f);
            Camera.main.transform.DORotate(_winCameraPosition.eulerAngles, 1f);
            yield return new WaitForSeconds(1f);
            _player.PlayWinAnimation();
            _winFx.gameObject.SetActive(true);
            _winFx.Play();
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);
            var winSoundSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
            yield return new WaitForSeconds(4f);
            winSoundSource.Stop();
            GameManager.I.Win();
        }

        private IEnumerator LoseGameCoroutine()
        {
            _minigame15MenuUI.CloseSelf();
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
            _guard.PlayShootAnim();
            _opponent.PlayWinAnimation();
            _player.PlayLoseAnimation();
            yield return new WaitForSeconds(2f);
            GameManager.I.Lose();
            GameManager.I.HandleResult();
        }

        public void OnOpponentEndTurn()
        {
            StartCoroutine(CalculateScoreCoroutine());

            IEnumerator CalculateScoreCoroutine()
            {
                yield return new WaitForSeconds(0.5f);
                var turnWinner = _player.CurrentTurnScore > _opponent.CurrentTurnScore ? Side.Player : Side.Opponent;
                _minigame15MenuUI.PlayTextMessageAnimation(turnWinner == Side.Player ? GameLocalization.I.GetStringFromTable("STRING_YOU_WIN") : GameLocalization.I.GetStringFromTable("STRING_YOU_LOSE"));
                if (turnWinner == Side.Player)
                {
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG14_CORRECT);
                    _playerRoundWinCount++;
                }
                else
                {
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG14_WRONG);
                    _opponentRoundWinCount++;
                }
                _minigame15MenuUI.UpdateScore(_playerRoundWinCount, _opponentRoundWinCount);
                yield return new WaitForSeconds(2f);
                _minigame15MenuUI.AnnouncerTMP.gameObject.SetActive(false);
                _marblesManager.ClearMarbles();
                if (_playerRoundWinCount == 2 || _opponentRoundWinCount == 2)
                {
                    var gameWinner = _playerRoundWinCount > _opponentRoundWinCount ? Side.Player : Side.Opponent;
                    StartCoroutine(gameWinner == Side.Player ? WinGameCoroutine() : LoseGameCoroutine());
                }
                else
                {
                    _player.OnStartTurn();
                }
            }
        }

        public void OnPlayerEndTurn()
        {
            _opponent.OnStartTurn();
        }
    }

    public enum Side
    {
        Player,
        Opponent
    }
}