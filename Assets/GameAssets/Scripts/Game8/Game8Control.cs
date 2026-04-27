using System;
using System.Collections;
using NFramework;
using SquidGame;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.UI;
using UnityEngine;

namespace Game8
{
    public class Game8Control : BaseMinigameController
    {
        public static event Action<float> OnTimeChanged;

        [Header("Config")]
        [SerializeField] GameLevel[] _allGameLevels;

        [Header("Refs")]
        [SerializeField] private PlayerControl _playerControl;
        [SerializeField] private BoardControl _boardControl;

        public PlayerControl PlayerControl => _playerControl;
        public BoardControl BoardControl => _boardControl;

        private GameLevel _gameLevel;
        private float _timer;
        private bool _isStartCountDown;
        private Minigame08MenuUI _minigame08;

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            UIManager.I.Close(Define.UIName.MINIGAME_08_MENU);
            _minigame08 = UIManager.I.Open<Minigame08MenuUI>(Define.UIName.MINIGAME_08_MENU);
            _minigame08.SetData(this);
            _minigame08.JumpButtonPanel.gameObject.SetActive(true);

            var idLevel = PlayerPrefs.GetInt("LEVEL_GAME_8", 0) % _allGameLevels.Length;
            _gameLevel = _allGameLevels[idLevel];
            _playerControl.Init(this);
            _boardControl.Init(this);
        }

        public override void OnStart()
        {
            base.OnStart();
            _boardControl.Active();
            CameraControl.I.MoveToPosStartFollow(0.5f, () => CameraControl.I.StartFollow(true));
            OnTimeChanged?.Invoke(_gameLevel.Time);
        }

        public void Win()
        {
            StartCoroutine(IE_GameWin());

            IEnumerator IE_GameWin()
            {
                UIManager.I.Close(Define.UIName.MINIGAME_08_MENU);
                PlayerPrefs.SetInt("LEVEL_GAME_8", PlayerPrefs.GetInt("LEVEL_GAME_8", 0) + 1);
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);
                CameraControl.I.StopFollow();
                CameraControl.I.transform.GetChild(0).gameObject.SetActive(true);
                CameraControl.I.MoveToPosCamWin(0.5f, null);
                var winSoundSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
                yield return new WaitForSeconds(4f);
                winSoundSource.Stop();
                GameManager.I.Win();
            }
        }

        public override void OnLose()
        {
            StartCoroutine(IE_GameOver());

            IEnumerator IE_GameOver()
            {
                _minigame08.JumpButtonPanel.gameObject.SetActive(false);
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Failure);
                CameraControl.I.StopFollow();
                yield return new WaitForSeconds(1.5f);
                TryShowRevivePopup(GameManager.I.Revive, OnNoThanks);

                void OnNoThanks()
                {
                    UIManager.I.Close(Define.UIName.MINIGAME_08_MENU);
                    GameManager.I.HandleResult();
                }
            }
        }

        public override void OnRevive()
        {
            base.OnRevive();
            _minigame08.JumpButtonPanel.gameObject.SetActive(true);
            _isStartCountDown = false;
            _playerControl.Revive();
            _boardControl.Revive();
            CameraControl.I.StartFollow();
        }

        public void SetCountDown(bool b)
        {
            _isStartCountDown = b;
            _timer = _gameLevel.Time;
        }

        private void Update()
        {
            if (GameManager.I.CurGameState == EGameState.Playing && _isStartCountDown)
            {
                _timer -= Time.deltaTime;
                OnTimeChanged?.Invoke(_timer);

                if (_timer <= 0f)
                {
                    OnTimeChanged?.Invoke(0);
                    _boardControl.BreakAll();
                    _canRevive = false;
                    GameManager.I.Lose();
                }
            }
        }
    }

    [Serializable]
    public class GameLevel
    {
        public float Time;
    }

    public enum TypeShape
    {
        Triangle = 0,
        Circle = 1,
        Square = 2
    }
}