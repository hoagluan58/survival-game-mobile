using NFramework;
using SquidGame;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.UI;
using System;
using System.Collections;
using UnityEngine;

namespace Game7
{
    public class Game7Control : BaseMinigameController
    {
        public static event Action<int, int> OnProgressChanged;
        public static event Action<string> OnTimeChanged;

        private const float BOOSTER_TIME_ADD_RATIO = 0.2f;

        [Header("Config")]
        [SerializeField] GameLevel[] _allGameLevels;

        [Header("Refs")]
        [SerializeField] Hole[] _allHoles;
        [SerializeField] PlayerControl _playerControl;

        private Minigame07MenuUI _ui;
        private GameLevel _gameLevel;
        private float _timePlay;
        private int _currentHitSuccess;
        private int _boosterTimeAdded;
        public float BoosterTimeAdded => _boosterTimeAdded;

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();

            int idLevel = PlayerPrefs.GetInt("LEVEL_GAME_7", 0) % _allGameLevels.Length;
            _gameLevel = _allGameLevels[idLevel];

            for (int i = 0; i < _allHoles.Length; i++)
            {
                _allHoles[i].Init(_gameLevel.TimeIntervalHoles[i]);
            }

            _boosterTimeAdded = (int)(_gameLevel.Time * BOOSTER_TIME_ADD_RATIO);

            // UI
            UIManager.I.Close(Define.UIName.MINIGAME_07_MENU);
            _ui = UIManager.I.Open<Minigame07MenuUI>(Define.UIName.MINIGAME_07_MENU);
            _ui.SetData(this);

            _playerControl.Init(this);

            InvokeOnProgressChanged(_currentHitSuccess, _gameLevel.Goal);
        }

        public override void OnStart()
        {
            base.OnStart();
            _playerControl.Active();

            float minTimeInterval = Mathf.Min(_gameLevel.TimeIntervalHoles);
            for (int i = 0; i < _allHoles.Length; i++)
            {
                _allHoles[i].Active(minTimeInterval);
            }
        }

        public override void OnLose()
        {
            StartCoroutine(IE_GameOver());

            IEnumerator IE_GameOver()
            {
                UIManager.I.Close(Define.UIName.MINIGAME_07_MENU);
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Failure);
                EndGame();
                yield return new WaitForSeconds(1.5f);
                GameManager.I.HandleResult();
            }
        }

        public override void OnWin()
        {
            StartCoroutine(IE_GameWin());

            IEnumerator IE_GameWin()
            {
                UIManager.I.Close(Define.UIName.MINIGAME_07_MENU);
                PlayerPrefs.SetInt("LEVEL_GAME_7", PlayerPrefs.GetInt("LEVEL_GAME_7", 0) + 1);
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);
                EndGame();
                Camera.main.transform.GetChild(0).gameObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
            }
        }

        public override void OnUseBooster()
        {
            base.OnUseBooster();
            _gameLevel.Time += _boosterTimeAdded;
            float restTime = _gameLevel.Time - _timePlay;
            float second = Mathf.FloorToInt(restTime);
            float miliSecond = restTime - second;
            InvokeOnTimeChanged(string.Format("{0}:{1}", second, Mathf.FloorToInt(miliSecond * 100)));
        }

        private void Update()
        {
            if (GameManager.I.CurGameState == EGameState.Playing)
            {
                _timePlay += Time.deltaTime;
                float restTime = _gameLevel.Time - _timePlay;
                float second = Mathf.FloorToInt(restTime);
                float miliSecond = restTime - second;
                InvokeOnTimeChanged(string.Format("{0}:{1}", second, Mathf.FloorToInt(miliSecond * 100)));

                if (restTime <= 0f)
                {
                    InvokeOnTimeChanged("00:00");
                    GameManager.I.Lose();
                }
            }
        }

        public void HitOne()
        {
            _ui.ToggleTutorial(false);
            _currentHitSuccess++;
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG07_SMASH);
            if (_currentHitSuccess == _gameLevel.Goal)
                GameManager.I.Win(2f);
            InvokeOnProgressChanged(_currentHitSuccess, _gameLevel.Goal);
        }

        private void EndGame()
        {
            _playerControl.DeActive();
            for (int i = 0; i < _allHoles.Length; i++)
            {
                _allHoles[i].DeActive();
            }
        }

        public void InvokeOnTimeChanged(string str) => OnTimeChanged?.Invoke(str);

        public void InvokeOnProgressChanged(int current, int total) => OnProgressChanged?.Invoke(current, total);
    }

    [System.Serializable]
    public class GameLevel
    {
        public float Time;
        public int Goal;
        public float[] TimeIntervalHoles;
    }
}
