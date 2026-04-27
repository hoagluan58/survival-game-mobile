using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.Minigame17.UI;
using System.Collections;
using UnityEngine;

namespace SquidGame.Minigame17
{
    public class MinigameController : BaseMinigameController
    {
        public const int PLAY_TIME = 30;

        private const float BOOSTER_TIME_ADD_RATIO = 0.2f;

        [Header("REF")]
        [SerializeField] private BaseGuard _guard;
        [SerializeField] private Character _player;
        [SerializeField] private Transform _playingPos;
        [SerializeField] private Transform _deadPos;
        [SerializeField] private ParticleSystem _fxWin;

        [Header("SUB CONTROLLER")]
        [SerializeField] private BoxController _boxController;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CameraController _cameraController;

        [Header("UI")]
        [SerializeField] private Minigame17MenuUI _ui;

        private Coroutine _countdownCoroutine;
        private int _currentTime;
        private int _boosterTimeAdded;
        public float BoosterTimeAdded => _boosterTimeAdded;

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            _boosterTimeAdded = (int)(PLAY_TIME * BOOSTER_TIME_ADD_RATIO);
            _ui.Init(this);
            _cameraController.Init(CameraController.ECameraType.Init);
            _boxController.Init(this, _ui);
            _playerController.Init(_boxController);
        }

        public override void OnStart()
        {
            base.OnStart();
            StartCoroutine(CRStartGame());

            IEnumerator CRStartGame()
            {
                yield return _cameraController.CRSwitchCamera(CameraController.ECameraType.Follow);
                yield return _player.DORotateY(0f);
                yield return _player.MoveTo(_playingPos.position, 1.5f);
                yield return _cameraController.CRSwitchCamera(CameraController.ECameraType.Playing);
                yield return _boxController.CRShowAllBox();
                yield return new WaitForSeconds(1f);
                _playerController.EnableInput(true);
                StartCountdown(PLAY_TIME);
            }
        }

        public override void OnWin()
        {
            base.OnWin();
            StartCoroutine(CRWinGame());

            IEnumerator CRWinGame()
            {
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);
                _ui.SetActive(false);
                if (_countdownCoroutine != null)
                {
                    StopCoroutine(_countdownCoroutine);
                }
                yield return _cameraController.CRSwitchCamera(CameraController.ECameraType.Result);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
                _fxWin.Play();
                yield return _player.CRWin();
            }
        }

        public override void OnLose()
        {
            base.OnLose();
            StartCoroutine(CRLoseGame());

            IEnumerator CRLoseGame()
            {
                _ui.SetActive(false);
                _playerController.EnableInput(false);
                yield return _cameraController.CRSwitchCamera(CameraController.ECameraType.Result);
                yield return _player.DORotateY(-180f);
                yield return _player.MoveTo(_deadPos.position, 1f);
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Failure);
                _guard.PlayShootAnim();
                _player.Die();
                yield return new WaitForSeconds(4f);
                GameManager.I.HandleResult();
            }
        }

        public override void OnUseBooster()
        {
            base.OnUseBooster();
            _currentTime += _boosterTimeAdded;
            StartCountdown(_currentTime);
        }

        private void StartCountdown(int time)
        {
            if (_countdownCoroutine != null)
            {
                StopCoroutine(_countdownCoroutine);
            }

            _ui.ShowBoosterButton(_isBoosterAvailable);
            _currentTime = time;
            _countdownCoroutine = StartCoroutine(CRCountdown());

            IEnumerator CRCountdown()
            {
                var waiter = new WaitForSeconds(1f);
                _ui.ShowCountdownText(_currentTime, true);
                while (_currentTime > 0)
                {
                    _currentTime--;
                    yield return waiter;
                }
                GameManager.I.Lose();
            }
        }
    }
}
