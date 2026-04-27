using DinoFractureDemo;
using NFramework;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.MinigameMingle;
using System;
using System.Collections;
using UnityEngine;

namespace SquidGame.LandScape.MinigameRockPaperScissors
{

    public class RockPaperScissorController : BaseMinigameController
    {
        [SerializeField] private Revolver _revolver;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private Player _player;
        [SerializeField] private Opponent _opponent;
        [SerializeField] private Guard _guard;
        [SerializeField] private LevelSaver _levelSaver;

        private MinigameRockPaperScissorsMenuUI _ui;
        private int _currentRound = 0;
        private int _targetRound = 0;
        private bool _isPlayerSelected;
        private const int MAX_ROUND = 6;

        public int Level => _levelSaver.GetLevel(GameManager.I.GetGameMode());  

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();

            _ui = UIManager.I.Open<MinigameRockPaperScissorsMenuUI>(Define.UIName.MINIGAME_ROCKPAPERSCISSORS_MENU);
            _ui.Initialized(MAX_ROUND, Level);
            _ui.OnPlayerSelectedAction += OnPlayerSelected;

            _targetRound = UnityEngine.Random.Range(4, MAX_ROUND);
            
            _cameraController.Init();
            _cameraController.IntroPoint();

            _player.Init();
            _opponent.Init();

            this.InvokeDelay(1f, () =>
            {
                GameManager.I.StartMinigame();
            });
        }

        private void OnDisable()
        {
            if (_ui != null) _ui.OnPlayerSelectedAction -= OnPlayerSelected;
        }


        private void OnPlayerSelected(GameResult result)
        {
            _isPlayerSelected = true;
            StartCoroutine(CRCheckResult(result));
        }


        public override void OnStart()
        {
            base.OnStart();
            StartCoroutine(CRStartgame());
        }


        [Button]
        public override void OnRevive()
        {
            base.OnRevive();
            _ui.UpdateMagazine(_currentRound, BulletState.Mask);
            _ui = UIManager.I.Open<MinigameRockPaperScissorsMenuUI>(Define.UIName.MINIGAME_ROCKPAPERSCISSORS_MENU);
            _opponent.OnRevive();
            _player.OnRevive();
            StartCoroutine(CRStartRound());
        }


        private IEnumerator CRCheckResult(GameResult playerResult)
        {
            _ui.SetActiveOption(false);
            var opponentResult = _opponent.GetGameResult(playerResult);
            var isWin = IsPlayerWinRound(playerResult, opponentResult);
            var isDied = _currentRound == _targetRound;
            StartCoroutine(_opponent.CRPlayAnimation(EAnimStyle.Rock_Paper_Scissor1));
            yield return _player.CRPlayAnimation(EAnimStyle.Rock_Paper_Scissor1);
            _ui.ShowResult(playerResult, opponentResult);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_ROCKPAPERSCISSOR_RESULT);
            yield return new WaitForSeconds(0.2f);//count down
            _opponent.PlayAnimation(EAnimStyle.Idle,0.5f);
            _player.PlayAnimation(EAnimStyle.Idle,0.5f);
            yield return new WaitForSeconds(1f);//gd feedback delay sound
            GameSound.I.PlaySFX(isWin ? Define.SoundPath.SFX_ROCKPAPERSCISSOR_CORRECT : Define.SoundPath.SFX_ROCKPAPERSCISSOR_WRONG);
            yield return new WaitForSeconds(1f);
            _opponent.PlayAnimationOneTime(isWin ? EAnimStyle.Head_Shake : EAnimStyle.Win_Round,EAnimStyle.Idle);
            _player.PlayAnimationOneTime(isWin ? EAnimStyle.Win_Round : EAnimStyle.Head_Shake, EAnimStyle.Idle);
            yield return new WaitForSeconds(2f);
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Warning);
            _guard.Shoot(isWin ? _opponent.transform : _player.transform);
            yield return CheckResult(isWin, isDied);
        }


        private IEnumerator CheckResult(bool isWin,bool isDied)
        {
            _ui.UpdateMagazine(_currentRound, isDied ? BulletState.Bullet : BulletState.Empty);
            if (isDied)
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
                if (isWin)
                {
                    _cameraController.OnWin(1f);
                    _player.PlayAnimation(EAnimStyle.Victory_4, 0.2f);
                    _opponent.Dead();
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
                    yield return new WaitForSeconds(1f);
                    _ui.CloseSelf();
                    yield return new WaitForSeconds(2f);
                    GameManager.I.Win();
                    _levelSaver.LevelUp(GameManager.I.GetGameMode());
                }
                else
                {
                    _player.Dead();
                    _ui.CloseSelf();
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_LOSING);
                    this.InvokeDelay(2f, () => {
                        GameManager.I.Lose();
                    });
                }
            }
            else
            {
                _ui.SetNoti("Blank shoot");
                GameSound.I.PlaySFX(Define.SoundPath.SFX_ROCKPAPERSCISSOR_SHOOT_EMPTY);
                yield return new WaitForSeconds(1.5f);
                _ui.SetNoti("Next round");
                yield return new WaitForSeconds(1.5f);
                _guard.ShowGun(false);
                _currentRound++;
                StartCoroutine(CRStartRound());
            }
        }


        private IEnumerator CRStartgame()
        {
            _currentRound = 0; 
            yield return _revolver.CRPlayAnimation();
            yield return _cameraController.SwitchCamera(CameraType.Playing, 2);
            yield return _ui.CRCountdownStartGame(3);
            yield return CRStartRound();
        }


        private IEnumerator CRStartRound()
        {
            _isPlayerSelected = false ;
            _ui.SetActiveMagazineUI(true);
            _ui.RotateMagazine(_currentRound);
            _ui.SetActiveOption(true);
            yield return _ui.CRCountdownSelectOption();
            if(!_isPlayerSelected) OnPlayerSelected(GetRandomEnum<GameResult>());
        }


        private bool IsPlayerWinRound(GameResult player , GameResult opponent)
        {
            switch (player)
            {
                case GameResult.Rock:
                    return opponent == GameResult.Scissor;
                case GameResult.Paper:
                    return opponent == GameResult.Rock;
                case GameResult.Scissor:
                    return opponent == GameResult.Paper;
                default:
                    return false;
            }
        }


        private T GetRandomEnum<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }
    }
}
