using NFramework;
using SquidGame;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.Minigame09;
using SquidGame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game9
{
    public class Game9Control : BaseMinigameController
    {
        public static event Action<int, int> OnProgressChanged;
        public static event Action<string> OnTimeChanged;

        private const float BOOSTER_TIME_ADD_RATIO = 0.2f;

        [Header("Config")]
        [SerializeField] GameLevel[] _allGameLevels;
        [SerializeField] private Vector2 _rangeX;
        [SerializeField] private Vector2 _rangeZ;

        [Header("Refs")]
        [SerializeField] private CharacterControl _characterControl;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private CharacterArrow _arrow;

        private Minigame09MenuUI _ui;
        private GameLevel _gameLevel;
        private float _timePlay;
        private int _currentEnemyDie;
        private List<EnemyBot> _enemies = new List<EnemyBot>();
        private int _boosterTimeAdded;

        public float BoosterTimeAdded => _boosterTimeAdded;
        public List<EnemyBot> Enemies => _enemies;

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();

            // Level
            var idLevel = PlayerPrefs.GetInt("LEVEL_GAME_9", 0) % _allGameLevels.Length;
            _gameLevel = _allGameLevels[idLevel];
            _boosterTimeAdded = (int)(_gameLevel.Time * BOOSTER_TIME_ADD_RATIO);

            // UI
            UIManager.I.Close(Define.UIName.MINIGAME_09_MENU);
            _ui = UIManager.I.Open<Minigame09MenuUI>(Define.UIName.MINIGAME_09_MENU);
            _ui.SetData(this);

            _characterControl.Init(this, _ui.Joystick);
            CreateEnemy();
            _arrow.Init(this, _characterControl);
        }

        public override void OnStart()
        {
            base.OnStart();
            _characterControl.SetActive(true);
            CameraControl.I.StartFollow();
            OnProgressChanged?.Invoke(_currentEnemyDie, _gameLevel.Goal);
        }

        public override void OnWin()
        {
            StartCoroutine(IE_GameWin());

            IEnumerator IE_GameWin()
            {
                UIManager.I.Close(Define.UIName.MINIGAME_09_MENU);
                PlayerPrefs.SetInt("LEVEL_GAME_9", PlayerPrefs.GetInt("LEVEL_GAME_9", 0) + 1);
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);
                CameraControl.I.transform.GetChild(0).gameObject.SetActive(true);
                CameraControl.I.StopFollow();
                CameraControl.I.ZoomToTarget(_characterControl.transform.position, 0.5f);
                CameraControl.I.LookAtTarget(_characterControl.transform.position + Vector3.up * 0.5f, 0.5f);
                _characterControl.ShowWin();
                var source = GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
                yield return new WaitForSeconds(3f);
                source.Stop();
            }
        }

        public override void OnLose()
        {
            StartCoroutine(IE_GameOver());

            IEnumerator IE_GameOver()
            {
                UIManager.I.Close(Define.UIName.MINIGAME_09_MENU);
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Failure);
                _characterControl.Die();
                yield return new WaitForSeconds(1.5f);
                GameManager.I.HandleResult();
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

        private void CreateEnemy()
        {
            for (int i = 0; i < _gameLevel.NumberEnemy; i++)
            {
                Vector3 pos = Vector3.zero;
                pos.x = UnityEngine.Random.Range(_rangeX.x, _rangeX.y);
                pos.z = UnityEngine.Random.Range(_rangeZ.x, _rangeZ.y);

                EnemyBot enemy = Instantiate(_enemyPrefab, transform).GetComponent<EnemyBot>();
                enemy.Init(this);
                enemy.transform.position = pos;

                _enemies.Add(enemy);
            }
        }

        public EnemyBot GetEnemyInRange(Vector3 posCheck, float range)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i].IsActive && Vector3.Distance(posCheck, _enemies[i].transform.position) <= range)
                    return _enemies[i];
            }

            return null;
        }

        private void Update()
        {
            if (GameManager.I.CurGameState == EGameState.Playing)
            {
                _timePlay += Time.deltaTime;
                float restTime = _gameLevel.Time - _timePlay;
                float second = Mathf.FloorToInt(restTime);
                float miliSecond = restTime - second;
                OnTimeChanged?.Invoke(string.Format("{0}:{1}", second, Mathf.FloorToInt(miliSecond * 100)));

                if (restTime <= 0f)
                {
                    OnTimeChanged?.Invoke("00:00");
                    GameManager.I.Lose();
                }
            }
        }

        public void EnemyDie()
        {
            _currentEnemyDie++;
            if (_currentEnemyDie == _gameLevel.Goal)
            {
                GameManager.I.Win(4f);
            }
            OnProgressChanged?.Invoke(_currentEnemyDie, _gameLevel.Goal);
        }


    }

    [System.Serializable]
    public class GameLevel
    {
        public int NumberEnemy;
        public int Goal;
        public float Time;
    }
}