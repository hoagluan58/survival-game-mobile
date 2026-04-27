using NFramework;
using SquidGame;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.Minigame01;
using SquidGame.UI;
using System;
using System.Collections;
using UnityEngine;

namespace Game1
{
    public class Game1Control : BaseMinigameController
    {
        public static event Action<float> OnTimerChanged;

        private const float BOOSTER_TIME_ADD_RATIO = 0.2f;
        [SerializeField] private int _prepareTime = 5;
        //[SerializeField] private CameraController _cameraController;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private HunterController _hunterController;
        [SerializeField] private BotManager _botManager;
        [SerializeField] private EnviromentHandler _enviromentHandler;
        [SerializeField] private float _timeLeft;

        
        private Minigame01MenuUI _ui;
        private bool _startCounting;
        private int _boosterTimeAdded;

        public HunterController HunterController => _hunterController;
        public PlayerController PlayerController => _playerController;
        public BotManager BotManager => _botManager;
        public float TimeLeft => _timeLeft;
        public float BoosterTimeAdded => _boosterTimeAdded;
        public int PrepareTime => _prepareTime;
        public bool IsStartGame; 

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            IsStartGame = false;
            GameSound.I.PlayBGM(Define.SoundPath.BGM_MINIGAME_01);
            _boosterTimeAdded = (int)(_timeLeft * BOOSTER_TIME_ADD_RATIO);

            // UI
            UIManager.I.Close(Define.UIName.MINIGAME_01_MENU);
            _ui = UIManager.I.Open<Minigame01MenuUI>(Define.UIName.MINIGAME_01_MENU);
            _ui.SetData(this);

            //_cameraController.Init(CameraController.ECameraType.Init);
            _playerController.Init(this, _ui);
            _hunterController.Init(this, _botManager);

            _botManager.Init(this);

            StartCoroutine(CRTransition());
            IEnumerator CRTransition()
            {
                yield return new WaitForSeconds(1f);
                _hunterController.Rotate();
                //yield return _cameraController.CRSwitchCamera(CameraController.ECameraType.Follow);
                _ui.CountdownStartgame();
            }
            _playerController.IsPlaying = true;
        }

        public override void OnStart()
        {
            base.OnStart();
            IsStartGame = true; 
            GameSound.I.StopBGM();
            _startCounting = true;
            _hunterController.StartGame();
            _enviromentHandler.StartGame();
        }

        public override void OnWin()
        {
            base.OnWin();
            UIManager.I.Close(Define.UIName.MINIGAME_01_MENU);
        }

        public override void OnLose()
        {
            _ui.Joystick.ResetJoystick();

            TryShowRevivePopup(GameManager.I.Revive, OnNoThanks);

            void OnNoThanks()
            {
                UIManager.I.Close(Define.UIName.MINIGAME_01_MENU);
                GameManager.I.HandleResult();
            }
        }

        public override void OnRevive()
        {
            _playerController.Revive();
            _hunterController.StartGame();
        }

        public override void OnUseBooster()
        {
            _timeLeft += _boosterTimeAdded;
            OnTimerChanged?.Invoke(_timeLeft);
        }

        private void Update()
        {
            if (_startCounting && _playerController.IsPlaying)
            {
                _timeLeft -= Time.deltaTime;
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
    }
}
