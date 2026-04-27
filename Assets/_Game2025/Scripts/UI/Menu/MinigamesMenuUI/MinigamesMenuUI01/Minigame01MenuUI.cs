
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame1
{
    public class Minigame01MenuUI : BaseUIView
    {
        public event Action OnCountdownCompletedAction;

        [Header("INTRO")]
        [SerializeField] private CanvasGroup _introGo;
        [SerializeField] private TextMeshProUGUI _introTMP;

        [Header("PLAYING")]
        [SerializeField] private GameObject _gameplayGo;
        [SerializeField] private GameObject _redLight;
        [SerializeField] private GameObject _greenLight;
        [SerializeField] private GameObject _warningPNL;
        [SerializeField] private TextMeshProUGUI _timerTMP;
        [SerializeField] private InputCharacterBase _inputCharacterBase;
        [SerializeField] private GameObject _alertGo;
        [SerializeField] private Button _settingButton;
        [SerializeField] private TextMeshProUGUI _levelText;
        private MinigameController _controller;
        private int _prepareTime;

        public InputCharacterBase Input => _inputCharacterBase;

        private bool _isClose = true;

        public override void OnOpen()
        {
            base.OnOpen();
            _settingButton.onClick.AddListener(() =>
            {
                GameSound.I.PlaySFXButtonClick();
                UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
            });
            MinigameController.OnTimerChanged += OnTimerChanged;
            HunterController.OnEnemySinging += OnEnemySinging;
            _isClose = false;
        }

        public override void OnClose()
        {
            base.OnClose();
            _isClose = true;
            MinigameController.OnTimerChanged -= OnTimerChanged;
            HunterController.OnEnemySinging -= OnEnemySinging;

            _settingButton.onClick.RemoveAllListeners();

        }

        public override void CloseSelf(bool destroy = false)
        {
            base.CloseSelf(destroy);

        }

        public void SetData(MinigameController controller)
        {
            _controller = controller;
            _prepareTime = controller.PrepareTime;
            UpdateLight(false);
            OnWarning(false);
            ShowIntro(false, 0);
            ShowGameplay(false, 0);
            ShowAlert(false);
        }

        public void SetLevelText(int level)
        {
            _levelText.text = $"Level {level + 1}";
        }


        public async void CountdownStartgame()
        {
            ShowIntro(true, 0.1f);
            while (_prepareTime > 0 && !_isClose)
            {
                _introTMP.text = $"Game start in {_prepareTime} seconds";
                _prepareTime--;
                await UniTask.Delay(1000);
                if (!_isClose) GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_TICK);
            }
            if (_isClose) return;
            _introTMP.text = $"Green light: Run - Red light: Stop";
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_WHISTLE);
            ShowGameplay(true, 0.5f);
            OnCountdownCompletedAction?.Invoke();
            await UniTask.Delay(1500);
            ShowIntro(false, 0.5f);
        }

        private void OnTimerChanged(float time) => UpdateTimeText(time);

        private void OnEnemySinging(bool isSinging) => UpdateLight(isSinging);

        private void OnWarning(bool isWarning) => _warningPNL.SetActive(isWarning);

        private void UpdateTimeText(float time) => _timerTMP.text = $"00:{time.ToString("00")}";

        private void UpdateLight(bool isSinging)
        {
            _redLight.SetActive(!isSinging);
            _greenLight.SetActive(isSinging);
        }

        private void OnUseBootser()
        {
            GameSound.I.PlaySFXButtonClick();
            //GameManager.I.UseBooster();
        }


        /// <summary>
        /// TODO: Need update to improved game
        /// </summary>
        /// <param name="value"></param>
        /// <param name="duration"></param>
        public void ShowIntro(bool value, float duration)
        {
            _introGo.DOFade(value ? 1 : 0, duration);
        }

        /// <summary>
        /// TODO: Need update to improved game
        /// </summary>
        /// <param name="value"></param>
        /// <param name="duration"></param>
        public void ShowGameplay(bool value, float duration)
        {
            _gameplayGo.SetActive(value);
        }

        public void ShowAlert(bool value)
        {
            _alertGo.SetActive(value);
            OnWarning(value);
        }


        public void OnRevive()
        {
            ShowAlert(false);
            UpdateLight(true);
            OnWarning(false);
        }
    }
}
