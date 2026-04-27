using System.Collections;
using DG.Tweening;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.Minigame14
{
    public class MinigameController : BaseMinigameController
    {
        [Header("OBJECTS")]
        [SerializeField] private BaseGuard _guard;
        [SerializeField] private BowlManager _bowlManager;
        [SerializeField] private Unit _player;
        [SerializeField] private Unit _opponent;
        [SerializeField] private Button _startButton;
        [SerializeField] private Canvas _startMinigameCanvas;
        [SerializeField] private ParticleSystem _winFx;

        [Header("CONFIGS")]
        [SerializeField] private Vector3 _startCameraRotation;
        [SerializeField] private Vector3 _defaultCameraRotation;
        [SerializeField] private Vector3 _endCameraPosition;
        [SerializeField] private Vector3 _endCameraRotation;

        private Camera _mainCamera;
        private readonly WaitForSeconds _waitFor1Second = new(1f);
        private readonly WaitForSeconds _waitFor3Second = new(3f);

        private void OnEnable()
        {
            _startButton.onClick.AddListener(OnStartButtonClick);
            _bowlManager.ClickedCorrectBowl += PerformWinMinigame;
            _bowlManager.ClickedWrongBowl += PerformLoseMinigame;
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClick);
            _bowlManager.ClickedCorrectBowl -= PerformWinMinigame;
            _bowlManager.ClickedWrongBowl -= PerformLoseMinigame;
        }

        private void OnStartButtonClick()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_BUTTON_CLICK);
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Selection);
            GameManager.I.StartMinigame();
        }

        private IEnumerator StarGameCoroutine()
        {
            yield return _player.ThrowMarblesCoroutine();
            yield return _bowlManager.AddMarblesCoroutine(_player.Marbles);
            yield return _opponent.ThrowMarblesCoroutine();
            yield return _bowlManager.AddMarblesCoroutine(_opponent.Marbles);
            yield return _bowlManager.ShuffleBowlsCoroutine();
            _bowlManager.SetClickableBowls(true);
        }

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            _mainCamera = Camera.main;
            _bowlManager.Init();
            _startMinigameCanvas.gameObject.SetActive(false);
            StartCoroutine(EnterMinigameCoroutine());
        }

        public override void OnStart()
        {
            base.OnStart();
            GameSound.I.StopBGM();
            _startMinigameCanvas.gameObject.SetActive(false);
            StartCoroutine(StarGameCoroutine());
        }

        private IEnumerator EnterMinigameCoroutine()
        {
            _mainCamera.transform.eulerAngles = _startCameraRotation;
            _mainCamera.transform.DORotate(_defaultCameraRotation, 1f);
            yield return _waitFor1Second;
            _startMinigameCanvas.gameObject.SetActive(true);
        }

        private IEnumerator WinCoroutine()
        {
            yield return _waitFor1Second;
            _mainCamera.transform.DORotate(_endCameraRotation, 1f);
            _mainCamera.transform.DOMove(_endCameraPosition, 1f);
            yield return _waitFor1Second;
            _guard.PlayShootAnim();
            _opponent.PlayLoseAnimation();
            yield return new WaitForSeconds(0.5f);
            _winFx.gameObject.SetActive(true);
            _player.PlayWinAnimation();
            _winFx.Play();
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);
            var winSoundSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
            yield return new WaitForSeconds(4f);
            winSoundSource.Stop();
            GameManager.I.Win();
        }

        private IEnumerator LoseCoroutine()
        {
            yield return _waitFor1Second;
            _mainCamera.transform.DORotate(_endCameraRotation, 1f);
            _mainCamera.transform.DOMove(_endCameraPosition, 1f);
            yield return _waitFor1Second;
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
            _opponent.PlayWinAnimation();
            _guard.PlayShootAnim();
            _player.PlayLoseAnimation();
            yield return _waitFor3Second;
            GameManager.I.Lose();
            GameManager.I.HandleResult();
        }

        private void PerformWinMinigame()
        {
            StartCoroutine(WinCoroutine());
        }

        private void PerformLoseMinigame()
        {
            StartCoroutine(LoseCoroutine());
        }
    }
}