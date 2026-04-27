using Cinemachine;
using NFramework;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.UI;
using System;
using System.Collections;
using UnityEngine;


namespace SquidGame.LandScape.Minigame1
{
    public class MinigameController : BaseMinigameController
    {
        public static event Action<float> OnTimerChanged;

        private const float BOOSTER_TIME_ADD_RATIO = 0.2f;
        [Header("Configs")]
        public bool IsStartGame;
        [SerializeField] private int _prepareTime = 5;
        [SerializeField] private float _timeLeft;
        [SerializeField] private EGameMode _gameMode;

        [Header("References")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private HunterController _hunterController;
        [SerializeField] private BotManager _botManager;
        [SerializeField] private EnviromentHandler _enviromentHandler;
        [SerializeField] private LevelSaver _levelSaver;
        [SerializeField] private ObstacleSpawner _obstacleSpawner;
        [SerializeField] private CinemachineFreeLook _cinemachineFreeLook;
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private CameraSystem _cameraSystem;
        private Minigame01MenuUI _ui;
        private bool _startCounting;
        private int _boosterTimeAdded;
        protected bool _canRevive;
        protected bool _isBoosterAvailable;
        private LevelContent _levelContent;
        private bool _isTriggerSound = false;
        private AudioSource _countSource;


        public HunterController HunterController => _hunterController;
        public PlayerController PlayerController => _playerController;
        public BotManager BotManager => _botManager;
        public float TimeLeft => _timeLeft;
        public float BoosterTimeAdded => _boosterTimeAdded;
        public int PrepareTime => _prepareTime;

        [Button]
        public void WinGameEditor()
        {
            GameManager.I.Win();
        }



        public void PlayCountSound(bool isPlay)
        {
            if (isPlay)
            {
                if (_countSource == null) _countSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_COUNT_DOWN_8s);
            }
            else
            {
                if (_countSource != null)
                {
                    _countSource.Stop();
                    _countSource = null;
                }
            }
        }

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            InitializeUI();
            Initialized();
        }


        private void Initialized()
        {
            IsStartGame = false;
            _gameMode = GameManager.I.GetGameMode();
            GameSound.I.PlayBGM(Define.SoundPath.BGM_MINIGAME_01);
            if (_levelSaver.GetLevel(_gameMode) == 0)
                _timeLeft += 10;
            _boosterTimeAdded = (int)(_timeLeft * BOOSTER_TIME_ADD_RATIO);
            _levelContent = _levelConfig.GetLevelContent(_levelSaver.GetLevel(_gameMode));

            _playerController.Init(this, _ui);
            _playerController.IsPlaying = true;

            _hunterController.Init(this, _botManager);
            _hunterController.SetFixedPitch(_levelContent.IsFixedPitch && GameManager.I.CurSeasonId != 2);
            _hunterController.SetAlertTrigger(_ui.ShowAlert);
            _botManager.Init(this);

            _obstacleSpawner.SpawnObstacle(_levelContent.ObstacleLimit);
            _ui.SetLevelText(_levelSaver.GetLevel(_gameMode));

            _cameraSystem.OnInitialized();

            StartCoroutine(CRTransition());
        }


        private IEnumerator CRTransition()
        {
            yield return new WaitForSeconds(1f);
            _cameraSystem.PlayIntro(3, () =>
            {
                _hunterController.Rotate();
                _ui.CountdownStartgame();
            });
        }


        private void InitializeUI()
        {
            UIManager.I.Close(Define.UIName.MINIGAME_01_MENU);
            _ui = UIManager.I.Open<Minigame01MenuUI>(Define.UIName.MINIGAME_01_MENU);
            _ui.SetData(this);
            _ui.Input.SetJumpAction(_playerController.Jump);
            _ui.Input.InitializeFreeLookController(_cinemachineFreeLook);
            _ui.OnCountdownCompletedAction += StartGame;
        }


        private void StartGame()
        {
            GameManager.I.StartMinigame();
        }


        public override void OnStart()
        {
            base.OnStart();
            IsStartGame = true;
            GameSound.I.StopBGM();
            _startCounting = true;
            _botManager.StartGame();
            _hunterController.StartGame();
            _enviromentHandler.StartGame();

        }


        public override void OnWin()
        {
            base.OnWin();
            _levelSaver.LevelUp(_gameMode);
            UIManager.I.Close(Define.UIName.MINIGAME_01_MENU);
            _cinemachineFreeLook.Follow = _playerController.TargetCamera;
            _cinemachineFreeLook.LookAt = _playerController.TargetCamera;
            PlayCountSound(false);
        }


        public override void OnLose()
        {
            _hunterController.Stop();
            _hunterController.enabled = false;
            _botManager.Stop();
            UIManager.I.Close(Define.UIName.MINIGAME_01_MENU);
            PlayCountSound(false);
        }


        public override void OnRevive()
        {
            _timeLeft = _timeLeft <= 0 ? 15 : _timeLeft;
            _playerController.Revive();
            _botManager.Revive();
            _hunterController.enabled = true;
            _hunterController.StartGame();
            _startCounting = true;
            _ui = UIManager.I.Open<Minigame01MenuUI>(Define.UIName.MINIGAME_01_MENU);
            _ui.OnRevive();
        }


        public override void OnReload()
        {
            base.OnReload();
            _ui.OnCountdownCompletedAction -= StartGame;
            UIManager.I.Close(Define.UIName.MINIGAME_01_MENU, true);
            _isTriggerSound = false;
        }




        private void CheckSoundEndGame()
        {
            if (_isTriggerSound) return;

            if (_timeLeft <= 8)
            {
                _isTriggerSound = true;
                PlayCountSound(true);
            }
        }


        private void Update()
        {
            if (_startCounting && _playerController.IsPlaying)
            {
                _timeLeft -= Time.deltaTime;
                CheckSoundEndGame();
                if (_timeLeft <= 0)
                {
                    _timeLeft = 0;
                    _canRevive = false;
                    _startCounting = false;
                    StartCoroutine(_playerController.DieCoroutine());
                }
                OnTimerChanged?.Invoke(_timeLeft);
            }
        }


        public void LoseGame()
        {
            if (_timeLeft <= 0)
            {
                OnLose();
                GameManager.I.SetGameState(EGameState.Lose);
                UIManager.I.Open<LoseChallengePopupUI>(Define.UIName.LOSE_CHALLENGE_POPUP).ShowRevivePopup(false);
                return;
            }

            GameManager.I.Lose();
        }
    }
}
