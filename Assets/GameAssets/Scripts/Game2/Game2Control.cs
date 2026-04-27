using NFramework;
using SquidGame;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.UI;
using System;
using System.Collections;
using UnityEngine;

namespace Game2
{
    public class Game2Control : BaseMinigameController
    {
        public static event Action<int> OnTimerChanged;

        private const string LEVEL_KEY = "minigame2_completed_times";
        private const float BOOSTER_TIME_ADD_RATIO = 0.2f;

        [SerializeField] private GameLevel[] _allLevels;

        [Header("Refs")]
        [SerializeField] private PlayerController _playerControl;
        [SerializeField] private BotManager _botManager;
        [SerializeField] private Transform _endMap;
        [SerializeField] private GameObject _effWin;
        [SerializeField] private GameObject _doorObject;

        [Header("TEST")]
        [SerializeField] private bool _isTesting;

        private int _currentStep;
        private int _currentTime;
        private int _boosterTimeAdded;
        private bool _isShowTut;
        private bool _isCountdown;
        private GameLevel _currentLevel;
        private Minigame02MenuUI _ui;

        public int BoosterTimeAdded => _boosterTimeAdded;

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();

            GameSound.I.PlayBGM(Define.SoundPath.BGM_MINIGAME_02);

            // Init bridge
            // Random level
            LoadLevel();

            _currentTime = _isTesting ? 9999 : _currentLevel.TotalTime;
            _boosterTimeAdded = (int)(_currentTime * BOOSTER_TIME_ADD_RATIO);
            _endMap.transform.position = _currentLevel.PosEndArea.position;

            // Init player
            _playerControl.Init(this);

            // Init bots
            _botManager.Init(_currentLevel, _endMap);

            // UI
            UIManager.I.Close(Define.UIName.MINIGAME_02_MENU);
            _ui = UIManager.I.Open<Minigame02MenuUI>(Define.UIName.MINIGAME_02_MENU);
            _ui.SetData(this);

        }

        public override void OnStart()
        {
            base.OnStart();
            _currentStep = 0;
            _playerControl.Active();
            StartCoroutine(IE_Start());
            CameraControl.I.ActiveFollow(true);
            OnTimerChanged?.Invoke(_currentTime);
        }

        public override void OnLose()
        {
            base.OnLose();
            StartCoroutine(IE_GameOver());
        }

        public override void OnUseBooster()
        {
            _currentTime += _boosterTimeAdded;
            OnTimerChanged?.Invoke(_currentTime);
        }

        public override void OnRevive()
        {
            CameraControl.I.ActiveFollow(true);
            _playerControl.Revive();
            StartCoroutine(IE_CountDown());
        }

        public void StartCountDown()
        {
            if (_isShowTut)
            {
                _isShowTut = false;
                StartCoroutine(IE_CountDown());
            }
        }

        private IEnumerator IE_Start()
        {
            for (int i = 0; i < _currentLevel.AllSteps.Length; i++)
            {
                _currentLevel.AllSteps[i].ShowTrueMove();
            }
            yield return new WaitForSeconds(2f);
            for (var i = 0; i < _currentLevel.AllSteps.Length; i++)
            {
                _currentLevel.AllSteps[i].HideTrueMove();
            }
            ActiveStep(false);
            _isShowTut = true;
            _botManager.StartBotsBehaviour();
            StartCountDown();
        }

        private IEnumerator IE_CountDown()
        {
            var waitForOneSecond = new WaitForSeconds(1f);

            _isCountdown = true;
            while (_isCountdown)
            {
                if (GameManager.I.CurGameState != EGameState.Playing)
                    yield return null;

                yield return waitForOneSecond;
                if (!_isCountdown) break;

                // Modify time
                _currentTime--;
                OnTimerChanged?.Invoke(_currentTime);

                // Time out
                if (_currentTime == 0)
                {
                    OnTimerChanged?.Invoke(_currentTime);
                    BreakAll();
                    _playerControl.FallDown();
                    _botManager.FallAllBots();
                    _canRevive = false;
                    _isCountdown = false;
                    GameManager.I.Lose();
                }
            }
        }

        public void ActiveStep(bool isNext = true)
        {
            if (isNext)
            {
                _currentStep++;
                if (_currentStep == _currentLevel.AllSteps.Length)
                {
                    _playerControl.JumpToWin(_endMap.position, OnPlayerJumpedToWin);
                    return;
                }
            }
            _currentLevel.AllSteps[_currentStep].EnableJump();
        }

        public void ReviveStep() => _currentLevel.AllSteps[_currentStep].EnableJump(true);

        public void DeActiveStep() => _currentLevel.AllSteps[_currentStep].DisableJump();

        public void BreakAll()
        {
            for (int i = 0; i < 2; i++)
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_GLASS_BREAK);
            }
            for (int i = 0; i < _currentLevel.AllSteps.Length; i++)
            {
                _currentLevel.AllSteps[i].BreakAll();
            }
        }

        private IEnumerator IE_GameOver()
        {
            _isCountdown = false;
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Failure);
            CameraControl.I.ActiveFollow(false);
            CameraControl.I.LookAtTarget(_playerControl.transform, 0.5f);
            yield return new WaitForSeconds(2.5f);
            TryShowRevivePopup(GameManager.I.Revive, OnNoThanks);

            void OnNoThanks()
            {
                UIManager.I.Close(Define.UIName.MINIGAME_02_MENU);
                GameManager.I.HandleResult();
            }
        }

        public void StopCountTime() => _isCountdown = false;

        private void OnPlayerJumpedToWin()
        {
            var wait025 = new WaitForSeconds(0.25f);
            StartCoroutine(WinGameCoroutine());

            IEnumerator WinGameCoroutine()
            {
                _isCountdown = false;
                UIManager.I.Close(Define.UIName.MINIGAME_02_MENU);
                PlayerPrefs.SetInt("LEVEL_GAME_2", PlayerPrefs.GetInt("LEVEL_GAME_2", 0) + 1); // To modify random level
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);
                _doorObject.gameObject.SetActive(false);
                CameraControl.I.ActiveFollow(false);
                CameraControl.I.MoveToPosGameWin(0.5f);
                yield return new WaitForSeconds(0.8f);
                BreakAll();
                _botManager.FallAllBots();
                yield return wait025;
                var source = GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
                _effWin.SetActive(true);
                _playerControl.PlayWinAnimation();
                yield return new WaitForSeconds(4f);
                source.Stop();
                GameManager.I.Win();
                PlayerPrefs.SetInt(LEVEL_KEY, PlayerPrefs.GetInt(LEVEL_KEY) + 1);
            }
        }

        private void LoadLevel()
        {
            var levelIndex = PlayerPrefs.GetInt(LEVEL_KEY);

            if (levelIndex > _allLevels.Length - 1)
            {
                _currentLevel = _allLevels[UnityEngine.Random.Range(0, _allLevels.Length)];
            }
            else
            {
                _currentLevel = _allLevels[levelIndex];
            }

            _currentLevel.gameObject.SetActive(true);
        }
    }
}