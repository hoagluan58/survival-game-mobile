using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.UI;
using System;
using System.Collections;
using UnityEngine;

namespace SquidGame.Minigame03
{
    public class MinigameController : BaseMinigameController
    {
        public static event Action<bool> OnShowTutorial;
        public static event Action<bool> OnShowWarning;
        public static event Action<bool> OnShowCountdownPanel;
        public static event Action<int> OnTimeChanged;
        public static event Action<bool> OnShowTutorialHoldToMove;

        private const float BOOSTER_TIME_ADD_RATIO = 0.2f;

        [Header("Game")]
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private NeedleController _needleController;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private BreakDalgonaStep _breakDalgonaStep;
        [SerializeField] private Transform _tableParent;
        [SerializeField] private Transform _cakeParent;
        [SerializeField] private ParticleSystem _fxWin;

        [Header("Level")]
        [SerializeField] private Dalgona[] _allDalgonas;

        public NeedleController NeedleController => _needleController;
        public CameraController CameraController => _cameraController;

        private Dalgona _curDalgona;
        private Minigame03MenuUI _ui;
        private int _currentTime;
        private int _boosterTimeAdded;
        private Coroutine _crCountdown;

        public int BoosterTimeAdded => _boosterTimeAdded;

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();

            GameSound.I.PlayBGM(Define.SoundPath.BGM_MINIGAME_03);

            UIManager.I.Close(Define.UIName.MINIGAME_03_MENU);
            _ui = UIManager.I.Open<Minigame03MenuUI>(Define.UIName.MINIGAME_03_MENU);

            _playerController.Init(this);
            _breakDalgonaStep.Init(this, _ui.BreakDalgonaPNL);
            _needleController.Init(this, _cameraController);

            _currentTime = 20;
            _boosterTimeAdded = (int)(_currentTime * BOOSTER_TIME_ADD_RATIO);
            _cameraController.Init(CameraController.ECameraType.Init);

            _ui.SetData(this);
        }

        public override void OnStart()
        {
            base.OnStart();

            StartCoroutine(CRStartGame());

            IEnumerator CRStartGame()
            {
                yield return _cameraController.CRSwitchCamera(CameraController.ECameraType.Path, 0f);
                _cameraController.StartCamPath(2.5f, 0.5f, () =>
                {
                    StartCoroutine(CRStartSelectCase());
                });
            }
        }

        public override void OnWin()
        {
            StartCoroutine(IE_GameWin());

            IEnumerator IE_GameWin()
            {
                GameSound.I.StopBGM();
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);
                var source = GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
                UIManager.I.Close(Define.UIName.MINIGAME_03_MENU);
                PlayerPrefs.SetInt("WIN_GAME_3", PlayerPrefs.GetInt("WIN_GAME_3", 0) + 1);
                _fxWin.Play();
                InvokeShowWarning(false);
                _needleController.Deactivate();
                _needleController.ToggleLine(false);
                yield return new WaitForSeconds(1f);
                yield return _cameraController.CRSwitchCamera(CameraController.ECameraType.Win);
                //_tableParent.gameObject.SetActive(false);
                //_cakeParent.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.3f);
                _playerController.ShowWin();
                yield return new WaitForSeconds(2.5f);
                source.Stop();
            }
        }

        public override void OnUseBooster()
        {
            _currentTime += _boosterTimeAdded;
            OnTimeChanged?.Invoke(_currentTime);
        }

        public override void OnLose()
        {
            StartCoroutine(IE_GameOver());

            IEnumerator IE_GameOver()
            {
                GameSound.I.StopBGM();
                UIManager.I.Close(Define.UIName.MINIGAME_03_MENU);
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Failure);

                InvokeShowWarning(false);
                _needleController.ToggleLine(false);
                _needleController.Deactivate();

                yield return _cameraController.CRSwitchCamera(CameraController.ECameraType.Win);
                _tableParent.gameObject.SetActive(false);
                _cakeParent.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.3f);
                _playerController.Die();
                yield return new WaitForSeconds(3f);
                GameManager.I.HandleResult();
            }
        }

        public override void OnRevive() => GameManager.I.ReloadMinigame();

        public Dalgona GetDalgona()
        {
            _curDalgona = _allDalgonas.RandomItem();
            return _curDalgona;
        }

        public void StartCountDown() => _crCountdown = StartCoroutine(IE_CountDown());

        private IEnumerator IE_CountDown()
        {
            InvokeShowHoldToMoveTutorial(false);
            while (true)
            {
                OnTimeChanged?.Invoke(_currentTime);
                yield return new WaitForSeconds(1f);
                _currentTime--;
                if (GameManager.I.CurGameState != EGameState.Playing)
                    yield break;
                if (_currentTime == 0)
                {
                    OnTimeChanged?.Invoke(_currentTime);
                    _needleController.Deactivate();
                    GameManager.I.Lose();
                    yield break;
                }
            }
        }

        public void MoveStepBreakDalgona()
        {
            InvokeShowHoldToMoveTutorial(false);
            InvokeShowCountdownPanel(false);

            _needleController.Deactivate();
            if (_crCountdown != null)
            {
                StopCoroutine(_crCountdown);
            }

            _breakDalgonaStep.Active(_curDalgona);
        }

        public void InvokeShowTutorial(bool isShow) => OnShowTutorial?.Invoke(isShow);

        public void InvokeShowWarning(bool isShow) => OnShowWarning?.Invoke(isShow);

        public void InvokeShowCountdownPanel(bool isShow) => OnShowCountdownPanel?.Invoke(isShow);

        public void InvokeShowHoldToMoveTutorial(bool isShow) => OnShowTutorialHoldToMove?.Invoke(isShow);

        private IEnumerator CRStartSelectCase()
        {
            yield return _cameraController.CRSwitchCamera(CameraController.ECameraType.Choose);
            _playerController.Active();
        }
    }
}