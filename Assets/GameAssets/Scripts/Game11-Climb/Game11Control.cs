using System;
using System.Collections;
using NFramework;
using SquidGame;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.UI;
using UnityEngine;

namespace Game11
{
    public class Game11Control : BaseMinigameController
    {
        public static event Action<string> OnTimeChanged;
        public static event Action<bool> OnShowWarning;

        private const float BOOSTER_TIME_ADD_RATIO = 0.2f;

        [Header("Config")]
        [SerializeField] GameLevel[] _allGameLevels;

        [Header("Refs")]
        [SerializeField] private PlayerControl _playerControl;

        public Level Level => _level;

        private GameLevel _gameLevel;
        private float _timePlay;
        private Level _level;
        private bool _isCountingTime;
        private Minigame11MenuUI _ui;
        private int _boosterTimeAdded;
        public float BoosterTimeAdded => _boosterTimeAdded;

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();

            // Init level
            var idLevel = PlayerPrefs.GetInt("LEVEL_GAME_11", 0) % _allGameLevels.Length;
            _gameLevel = _allGameLevels[idLevel];
            _level = _gameLevel.Map;
            _level.gameObject.SetActive(true);
            _boosterTimeAdded = (int)(_gameLevel.Time * BOOSTER_TIME_ADD_RATIO);

            UIManager.I.Close(Define.UIName.MINIGAME_11_MENU);
            _ui = UIManager.I.Open<Minigame11MenuUI>(Define.UIName.MINIGAME_11_MENU);
            _ui.SetData(this);
            _playerControl.Init(this);
        }

        public override void OnStart()
        {
            base.OnStart();
            CameraControl.I.StartFollow();
            _playerControl.SetActive(true);
            _isCountingTime = true;

            // Update timer
            var restTime = _gameLevel.Time - _timePlay;
            var second = Mathf.FloorToInt(restTime);
            var miliSecond = restTime - second;
            OnTimeChanged?.Invoke($"{second}:{Mathf.FloorToInt(miliSecond * 100)}");
        }

        public override void OnLose()
        {
            _isCountingTime = false;
            StartCoroutine(IE_GameOver());

            IEnumerator IE_GameOver()
            {
                UIManager.I.Close(Define.UIName.MINIGAME_11_MENU);
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Failure);
                CameraControl.I.StopFollow();
                _playerControl.Fall();
                yield return new WaitForSeconds(1.5f);
                GameManager.I.HandleResult();
            }
        }

        public void Win()
        {
            _isCountingTime = false;
            StartCoroutine(IE_GameWin());

            IEnumerator IE_GameWin()
            {
                UIManager.I.Close(Define.UIName.MINIGAME_11_MENU);
                PlayerPrefs.SetInt("LEVEL_GAME_11", PlayerPrefs.GetInt("LEVEL_GAME_11", 0) + 1);
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);
                CameraControl.I.transform.GetChild(0).gameObject.SetActive(true);
                CameraControl.I.StopFollow();
                CameraControl.I.ZoomToTarget(_playerControl.transform.position, 0.5f);
                CameraControl.I.LookAtTarget(_playerControl.transform.position + Vector3.up * 0.5f, 0.5f);
                var winSoundSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
                yield return new WaitForSeconds(4f);
                winSoundSource.Stop();
                GameManager.I.Win();
            }
        }

        public override void OnUseBooster()
        {
            base.OnUseBooster();
            _gameLevel.Time += _boosterTimeAdded;
            float restTime = _gameLevel.Time - _timePlay;
            float second = Mathf.FloorToInt(restTime);
            float miliSecond = restTime - second;
            OnTimeChanged?.Invoke(string.Format("{0}:{1}", second, Mathf.FloorToInt(miliSecond * 100)));
        }

        private void Update()
        {
            if (_isCountingTime)
            {
                _timePlay += Time.deltaTime;
                var restTime = _gameLevel.Time - _timePlay;
                var second = Mathf.FloorToInt(restTime);
                var miliSecond = restTime - second;
                OnTimeChanged?.Invoke($"{second}:{Mathf.FloorToInt(miliSecond * 100)}");

                if (restTime <= 0f)
                {
                    OnTimeChanged?.Invoke("00:00");
                    GameManager.I.Lose();
                }
            }
        }

        public void InvokeOnShowWarning(bool value) => OnShowWarning?.Invoke(value);

        public void StopTimer()
        {
            _isCountingTime = false;
        }
    }

    [Serializable]
    public class GameLevel
    {
        public Level Map;
        public float Time;
    }
}