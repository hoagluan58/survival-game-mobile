using NFramework;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.MinigameMarblesVer2;
using System;
using System.Collections;
using UnityEngine;

namespace SquidGame.LandScape.Minigame16
{
    public class MinigameController : BaseMinigameController
    {
        [SerializeField] private LightController _lightController;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private PlayerCharacter _playerCharacter;
        [SerializeField] private Guard _guard;
        [SerializeField] private LevelSaver _levelSaver;
        [SerializeField] private ParticleSystem _winFx;

        private Minigame16MenuUI _ui;

        public int Level => _levelSaver.GetLevel(_gameMode);
        private EGameMode _gameMode;


        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            _gameMode = GameManager.I.GetGameMode();
            _ui = UIManager.I.Open<Minigame16MenuUI>(Define.UIName.MINIGAME_16_MENU);
            _cameraController.Init();
            _lightController.Init(this, _ui);
            _playerController.Init(this, _lightController, _ui);
            _ui.SetData(this);
            _playerCharacter.Init();
        }



        public override void OnStart()
        {
            base.OnStart();
            StartCoroutine(CRStartGame());

            IEnumerator CRStartGame()
            {
                _playerCharacter.DORotate(Vector3.zero, 1f);
                _lightController.PrepareGame();
                yield return _cameraController.SwitchCamera(CameraController.ECameraType.Playing);
                yield return CRGoNextRound();
            }
        }

        [Button]
        public override void OnRevive()
        {
            base.OnRevive();
            StartCoroutine(CRRevive());

            IEnumerator CRRevive()
            {
                _lightController.RandomCurrentRound();
                _playerCharacter.PlayIdleAnim();
                _playerCharacter.DORotate(Vector3.zero, 1f);
                _ui = UIManager.I.Open<Minigame16MenuUI>(Define.UIName.MINIGAME_16_MENU);
                yield return _cameraController.SwitchCamera(CameraController.ECameraType.Playing);
                yield return CRGoNextRound();
            }
        }

        public override void OnWin()
        {
            base.OnWin();
          
        }

        public override void OnLose()
        {
            base.OnLose();
           
        }

        public void TryWin()
        {
            if (_lightController.IsLastRound())
            {
                StartCoroutine(CRWinGame());

                IEnumerator CRWinGame()
                {
                    _ui.CloseSelf();
                    _playerCharacter.DORotate(new Vector3(0f, 180f, 0f), 0.5f);
                    yield return _cameraController.SwitchCamera(CameraController.ECameraType.Idle);
                    _winFx.gameObject.SetActive(true);
                    _winFx.Play();
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
                    _playerCharacter.PlayWinAnim();
                    _levelSaver.LevelUp(_gameMode);
                    yield return new WaitForSeconds(3f);
                    GameManager.I.Win();
                }
            }
            else
            {
                _ui.ShowLightPanelUI(false);
                _lightController.GoNextRound();
                this.InvokeDelay(0.5f, () =>
                {
                    StartCoroutine(CRGoNextRound());
                });
            }
        }

        private IEnumerator CRGoNextRound()
        {
            _ui.ShowLightPanelUI(false);
            yield return new WaitForSeconds(1f);
            _ui.ShowFocusText(true);
            yield return _lightController.CRPlayLightSequence();
            _ui.ShowFocusText(false);
            _ui.ShowRepeatItText(true);
            yield return new WaitForSeconds(2f);
            _ui.ShowRepeatItText(false);
            _ui.ShowLightPanelUI(true);
            _playerController.OnNewRound();
        }

        internal void LoseGame()
        {
            StartCoroutine(CRLoseGame());

            IEnumerator CRLoseGame()
            {
                _ui.CloseSelf();
                _playerCharacter.DORotate(new Vector3(0f, 180f, 0f), 0.5f);
                yield return _cameraController.SwitchCamera(CameraController.ECameraType.Idle);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
                _guard.LookAt(_playerCharacter.Head()).PlayShootAnim().ShowLine(0.25f, _playerCharacter.Head(), () => {
                    _playerCharacter.PlayDieAnim();
                }).ClearLine(0.45f);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_LOSING);
                yield return new WaitForSeconds(2f);
                GameManager.I.Lose();
            }
        }
    }
}
