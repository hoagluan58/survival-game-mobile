using NFramework;
using SquidGame;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.UI;
using System;
using System.Collections;
using UnityEngine;

namespace Game6
{
    public class Game6Control : BaseMinigameController
    {
        public static event Action<bool> OnShowTutorial;
        public static event Action<bool, bool> OnShowWarning;
        public static event Action<float> OnPlayerHPChanged;
        public static event Action<float> OnEnemyHPChanged;

        [Header("Refs")]
        [SerializeField] private PlayerControl _playerControl;
        [SerializeField] private EnemyControl _enemyControl;
        [SerializeField] private GameObject _stopBar;

        private Minigame06MenuUI _ui;

        public PlayerControl PlayerControl
        {
            get { return _playerControl; }
        }
        public EnemyControl EnemyControl
        {
            get { return _enemyControl; }
        }

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            UIManager.I.Close(Define.UIName.MINIGAME_06_MENU);
            _ui =  UIManager.I.Open<Minigame06MenuUI>(Define.UIName.MINIGAME_06_MENU);
            _playerControl.Init(this, _ui.Joystick);
            _enemyControl.Init(this);
        }

        public override void OnStart()
        {
            base.OnStart();
            _playerControl.Active();
            InvokeOnShowWarning(false, true);
            if (PlayerPrefs.GetInt("WIN_GAME_6", 0) != 0)
            {
                _stopBar.SetActive(true);
            }
        }

        public void Lose()
        {
            StartCoroutine(IE_GameOver());
            return;

            IEnumerator IE_GameOver()
            {
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Failure);
                InvokeOnShowWarning(false, true);
                _playerControl.Die();
                yield return new WaitForSeconds(1f);
                TryShowRevivePopup(GameManager.I.Revive, OnNoThanks);
            }

            void OnNoThanks()
            {
                this.InvokeDelay(2f, () =>
                {
                    UIManager.I.Close(Define.UIName.MINIGAME_06_MENU);
                    GameManager.I.Lose();
                    GameManager.I.HandleResult();
                });
            }
        }

        public void Win()
        {
            StartCoroutine(IE_GameWin());
            return;

            IEnumerator IE_GameWin()
            {
                UIManager.I.Close(Define.UIName.MINIGAME_06_MENU);
                PlayerPrefs.SetInt("WIN_GAME_6", PlayerPrefs.GetInt("WIN_GAME_6", 0) + 1);
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);
                InvokeOnShowWarning(false, true);
                CameraControl.I.transform.GetChild(0).gameObject.SetActive(true);
                _playerControl.Win();
                var winSoundSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
                yield return new WaitForSeconds(4f);
                winSoundSource.Stop();
                GameManager.I.Win();
            }
        }

        public override void OnRevive()
        {
            _playerControl.Revive();
            if (_playerControl.IsFighting)
            {
                _enemyControl.StartFight();
            }
        }

        public void PrepareFight()
        {
            StartCoroutine(IE_PrepareFight());

            IEnumerator IE_PrepareFight()
            {
                _enemyControl.RunToPlayer(_playerControl.transform.position);
                yield return new WaitForSeconds(0.75f);
                CameraControl.I.MoveToPosCamWin(1.5f, () =>
                {
                    _playerControl.StartFight();
                    _enemyControl.StartFight();
                    InvokeShowTutorial(true);
                });
            }
        }

        public void InvokeShowTutorial(bool value) => OnShowTutorial?.Invoke(value);

        public void InvokeOnShowWarning(bool value, bool isForce) => OnShowWarning?.Invoke(value, isForce);

        public void InvokeOnPlayerHPChanged(float value) => OnPlayerHPChanged?.Invoke(value);

        public void InvokeOnEnemyHPChanged(float value) => OnEnemyHPChanged?.Invoke(value);
    }
}