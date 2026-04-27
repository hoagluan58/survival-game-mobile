using CnControls;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game1;
using NFramework;
using SquidGame.LandScape;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    /// <summary>
    /// Broken script . can not use anymore
    /// </summary>
    public class Minigame01MenuUI : BaseUIView
    {
        [Header("INTRO")]
        [SerializeField] private CanvasGroup _introCanvasGroup;
        [SerializeField] private TextMeshProUGUI _introTMP;

        [Header("PLAYING")]
        [SerializeField] private CanvasGroup _gameplayCanvasGroup;
        [SerializeField] private GameObject _redLight;
        [SerializeField] private GameObject _greenLight;
        [SerializeField] private GameObject _warningPNL;
        [SerializeField] private TextMeshProUGUI _timerTMP;
        [SerializeField] private SimpleJoystick _joystick;
        
        


        [Header("BOOSTER")]
        [SerializeField] private Button _boosterBTN;
        [SerializeField] private TextMeshProUGUI _boosterDescriptionTMP;

        private Game1Control _controller;
        public SimpleJoystick Joystick => _joystick;

        private int _prepareTime;

        public override void OnOpen()
        {
            base.OnOpen();
            _boosterBTN.onClick.AddListener(OnUseBootser);
            Game1Control.OnTimerChanged += OnTimerChanged;
            HunterController.OnEnemySinging += OnEnemySinging;
            HunterController.OnWarning += OnWarning;
        }

        public override void OnClose()
        {
            base.OnClose();
            
            _boosterBTN.onClick.RemoveListener(OnUseBootser);
            Game1Control.OnTimerChanged -= OnTimerChanged;
            HunterController.OnEnemySinging -= OnEnemySinging;
            HunterController.OnWarning -= OnWarning;
        }

        public void SetData(Game1Control controller)
        {
            _controller = controller;
            _prepareTime = controller.PrepareTime;
            UpdateLight(false);
            OnWarning(false);
            _joystick.ResetJoystick();
            _boosterBTN.gameObject.SetActive(true);
            _boosterDescriptionTMP.text = $"+{_controller.BoosterTimeAdded}s";
            ShowIntro(true, 0);
            ShowGameplay(false, 0);
          
        }


        public async void CountdownStartgame()
        {
            //ignore "cancel unitask error debug"
            try
            {
                while (_prepareTime > 0)
                {
                    _prepareTime--;
                    _introTMP.text = $"Game start in {_prepareTime} seconds";
                    await UniTask.Delay(1000);
                }
                _introTMP.text = $"Green light: Run - Red light: Stop";
                ShowGameplay(true, 0.5f);
                //GameManager.I.StartMinigame();
                await UniTask.Delay(1000);
                ShowIntro(false, 0.5f);
            }
            catch
            {
                // ignored
            }
        }

        private void OnTimerChanged(float time) => UpdateTimeText(time);

        private void OnEnemySinging(bool isSinging) => UpdateLight(isSinging);

        private void OnWarning(bool isWarning) => _warningPNL.SetActive(isWarning);

        private void UpdateTimeText(float time) => _timerTMP.text = $"{(int)time}";

        private void UpdateLight(bool isSinging)
        {
            _redLight.SetActive(!isSinging);
            _greenLight.SetActive(isSinging);
        }

        private void OnUseBootser()
        {
            GameSound.I.PlaySFXButtonClick();
            _boosterBTN.gameObject.SetActive(false);
            //GameManager.I.UseBooster();
        }

        public void ShowIntro(bool value,float duration)
        {
            _introCanvasGroup.DOFade(value ? 1 : 0, duration);
            _introCanvasGroup.blocksRaycasts = value;
        }

        public void ShowGameplay(bool value, float duration)
        {
            _gameplayCanvasGroup.DOFade(value ? 1 : 0, duration);
            _gameplayCanvasGroup.blocksRaycasts = value;
        }
    }
}
