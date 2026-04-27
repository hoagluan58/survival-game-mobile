using NFramework;
using Redcode.Extensions;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Core;

// using SquidGame;
// using SquidGame.Core;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.UI;
using SquidGame.UI;
using System;
using System.Collections;
using UnityEngine;
using Define = SquidGame.LandScape.Core.Define;

namespace SquidGame.LandScape.Minigame5
{
    public class Game5Controller : BaseMinigameController
    {
        public static event Action<string> OnTimerChanged;
        public static event Action<bool> OnWarning;
        public static event Action<float> OnPowerBarChanged;

        // private const float BOOSTER_TIME_ADD_RATIO = 0.2f;

        [Header("Refs")]
        [SerializeField] private PlayerControl _playerControl;
        [SerializeField] private GameObject _mainRope;
        [SerializeField] private GameObject _breakRope;

        [Header("Configs")]
        [SerializeField] private float _totalTime = 45f;
        [SerializeField] private Character[] _playerTeams;
        [SerializeField] private Character[] _enemyTeams;
        [SerializeField] private Transform _deadCharacterParent;

        public Transform DeadCharacterParent => _deadCharacterParent;
        private float _timePlay;
        private int _boosterTimeAdded;
        private bool _isPlaying, _isWin;
        private AudioSource _cheerSound, _winSound, _countDownSound;

        public int BoosterTimeAdded => _boosterTimeAdded;
        public bool IsPlaying => _isPlaying;
        public bool IsWin => _isWin;

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            Init();
        }

        public override void OnReload()
        {
            base.OnReload();
            _totalTime = 60f;
        }

        public override void OnRevive()
        {
            base.OnRevive();
            GameManager.I.ReloadMinigame();
        }

        private void Init()
        {

            _playerTeams.ForEach(x => x.Init(this));
            _enemyTeams.ForEach(x => x.Init(this));
            // _boosterTimeAdded = (int)(_totalTime * BOOSTER_TIME_ADD_RATIO);

            UIManager.I.Close(Define.UIName.MINIGAME05_MENU);
            UIManager.I.Open<Minigame05MenuUI>(Define.UIName.MINIGAME05_MENU).SetData(this, _playerControl);
            GameManager.I.StartMinigame();
        }

        public void StartGame()
        {
            _isPlaying = true;
            _cheerSound = GameSound.I.PlaySFX(Define.SoundPath.SFX_MG05_CHEER, true);

            _playerTeams.ForEach(player => player.PullAnimation());
            _enemyTeams.ForEach(enemy => enemy.PullAnimation());
        }

        void OnDisable()
        {
            StopSounds();
        }

        void StopSounds()
        {
            _countDownSound?.Stop();
            _cheerSound?.Stop();
            _winSound?.Stop();
        }

        private void Update()
        {
            if (!_isPlaying) return;

            _timePlay += Time.deltaTime;
            float restTime = _totalTime - _timePlay;
            float second = Mathf.FloorToInt(restTime);
            float miliSecond = restTime - second;
            OnTimerChanged?.Invoke($"{second}");
            if (second <= 10 && _countDownSound == null)
            {
                _countDownSound = GameSound.I.PlaySFX(Define.SoundPath.SFX_MG05_COUNT_DOWN, true);
            }

            if (restTime <= 0f)
            {
                OnTimerChanged?.Invoke("0");
                _playerControl.EndGame();
            }
        }

        public void ShowAnimationLose()
        {
            StopSounds();
            InvokeOnWarning(false);
            _isPlaying = false;

            StartCoroutine(IE_GameOver());
            IEnumerator IE_GameOver()
            {
                UIManager.I.Close(Define.UIName.MINIGAME05_MENU);
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Failure);

                for (int i = 0; i < _enemyTeams.Length; i++)
                    _enemyTeams[i].Win();
                // yield return new WaitForSeconds(1.5f);
                _mainRope.SetActive(false);
                _breakRope.SetActive(true);
                // yield return new WaitForSeconds(0.5f);
                CameraControl.I.MoveToPosCamLose();
                for (int i = 0; i < _playerTeams.Length; i++)
                    _playerTeams[i].Lose();
                yield return new WaitForSeconds(3.5f);
                GameManager.I.Lose();
                GameSound.I.PlaySFX(Define.SoundPath.SFX_LOSING);
            }
        }

        public void ShowAnimationWin()
        {
            _isWin = true;
            StopSounds();
            InvokeOnWarning(false);
            _isPlaying = false;


            StartCoroutine(IE_GameWin());
            IEnumerator IE_GameWin()
            {
                UIManager.I.Close(Define.UIName.MINIGAME05_MENU);
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);

                for (int i = 0; i < _playerTeams.Length; i++)
                    _playerTeams[i].Win();
                CameraControl.I.MoveToPosCamWin(1f);
                yield return new WaitForSeconds(1f);
                for (int i = 0; i < _playerTeams.Length; i++)
                    _enemyTeams[i].Lose();
                _mainRope.SetActive(false);
                _breakRope.SetActive(true);
                yield return new WaitForSeconds(2f);
                CameraControl.I.MoveToPosShowVictory(1f);
                _winSound = GameSound.I.PlaySFX(Define.SoundPath.SFX_MG05_WIN, true);
                CameraControl.I.transform.GetChild(0).gameObject.SetActive(true);
                yield return new WaitForSeconds(3f);
                GameManager.I.Win();
                GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
            }
        }

        public void InvokeOnWarning(bool value) => OnWarning?.Invoke(value);
        public void InvokeOnPowerBarChanged(float value) => OnPowerBarChanged?.Invoke(value);

        // [Button]
        // public void StartGame()
        // {
        //     GameManager.I.StartMinigame();
        // }

        // [Button]
        // public void WinGame()
        // {
        //     GameManager.I.Win();
        // }
    }
}