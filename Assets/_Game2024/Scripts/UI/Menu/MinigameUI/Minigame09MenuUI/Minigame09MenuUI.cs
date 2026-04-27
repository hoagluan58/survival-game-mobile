using CnControls;
using Game9;
using NFramework;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class Minigame09MenuUI : BaseUIView
    {
        [SerializeField] private GameObject _playBTN;

        [Header("PLAYING")]
        [SerializeField] private GameObject _playingPNL;
        [SerializeField] private Image _fillIMG;
        [SerializeField] private TextMeshProUGUI _progressTMP;
        [SerializeField] private TextMeshProUGUI _timeCountdownTMP;
        [SerializeField] private SimpleJoystick _joystick;
        [SerializeField] private CanvasGroup _canvasGroupJoystick;

        [Header("BOOSTER")]
        [SerializeField] private Button _boosterBTN;
        [SerializeField] private TextMeshProUGUI _boosterDescriptionTMP;

        private Game9Control _controller;
        private bool _isDraggedJoystick = false;

        public SimpleJoystick Joystick => _joystick;

        public override void OnOpen()
        {
            base.OnOpen();
            Game9Control.OnProgressChanged += Game9Control_OnProgressChanged;
            Game9Control.OnTimeChanged += Game9Control_OnTimeChanged;
            _boosterBTN.onClick.AddListener(OnBoosterButtonClicked);
        }

        public override void OnClose()
        {
            base.OnClose();
            Game9Control.OnProgressChanged -= Game9Control_OnProgressChanged;
            Game9Control.OnTimeChanged -= Game9Control_OnTimeChanged;
            _boosterBTN.onClick.RemoveListener(OnBoosterButtonClicked);
        }

        private void OnBoosterButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            _boosterBTN.gameObject.SetActive(false);
            GameManager.I.UseBooster();
        }

        private void Game9Control_OnTimeChanged(string value) => UpdateCountdownText(value);

        private void Game9Control_OnProgressChanged(int current, int total) => UpdateProgress(current, total);

        private void Update()
        {
            if (!_isDraggedJoystick)
            {
                if (_joystick.VerticalAxis.Value != 0 || _joystick.HorizintalAxis.Value != 0)
                {
                    OnPlayButtonClicked();
                }
            }
        }

        private void OnPlayButtonClicked()
        {
            GameManager.I.StartMinigame();
            _playingPNL.SetActive(true);
            _playBTN.gameObject.SetActive(false);
            _canvasGroupJoystick.alpha = 1;
            _isDraggedJoystick = true;
        }

        public void SetData(Game9Control controller)
        {
            _controller = controller;
            _playBTN.gameObject.SetActive(true);
            _playingPNL.SetActive(false);
            _isDraggedJoystick = false;
            _canvasGroupJoystick.alpha = 0;
            _joystick.ResetJoystick();
            InitBoosterButton();
        }

        private void UpdateProgress(int current, int total)
        {
            _fillIMG.fillAmount = (float)current / total;
            _progressTMP.text = current + "/" + total;
        }

        private void UpdateCountdownText(string str) => _timeCountdownTMP.text = str;

        private void InitBoosterButton()
        {
            _boosterBTN.gameObject.SetActive(true);
            _boosterDescriptionTMP.text = $"+{_controller.BoosterTimeAdded}s";
        }
    }
}
