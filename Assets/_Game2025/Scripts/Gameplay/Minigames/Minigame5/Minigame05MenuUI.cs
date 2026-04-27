using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame5
{
    public class Minigame05MenuUI : BaseUIView
    {
        // [Header("PLAYING")]
        [SerializeField] private int _startTime = 5;
        [SerializeField] private GameObject _playingPNL;
        [SerializeField] private GameObject _warningObject, _headerStartObject;
        [SerializeField] private TextMeshProUGUI _timeCounterTMP, _timeStartTMP, _levelTMP;
        [SerializeField] private DOTweenAnimation _noticePullAnimation;
        [SerializeField] private UIPowerBar _powerBarUI;
        [SerializeField] private Button _settingButton;


        // [Header("BOOSTER")]
        // [SerializeField] private Button _boosterBTN;
        // [SerializeField] private TextMeshProUGUI _boosterDescriptionTMP;

        private Game5Controller _controller;

        public override void OnOpen()
        {
            base.OnOpen();
            // _playBTN.onClick.AddListener(OnPlayButtonClicked);
            // _boosterBTN.onClick.AddListener(OnBoosterButtonClicked);
            Game5Controller.OnTimerChanged += Game5Control_OnTimerChanged;
            Game5Controller.OnWarning += Game5Control_OnWarning;
            Game5Controller.OnPowerBarChanged += Game5Control_OnPowerBarChanged;

            _settingButton.onClick.AddListener(() =>
            {
                GameSound.I.PlaySFXButtonClick();
                UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
            });
        }


        public override void OnClose()
        {
            base.OnClose();
            // _playBTN.onClick.RemoveListener(OnPlayButtonClicked);
            // _boosterBTN.onClick.RemoveListener(OnBoosterButtonClicked);
            Game5Controller.OnTimerChanged -= Game5Control_OnTimerChanged;
            Game5Controller.OnWarning -= Game5Control_OnWarning;
            Game5Controller.OnPowerBarChanged -= Game5Control_OnPowerBarChanged;

            _settingButton.onClick.RemoveAllListeners();
        }

        // private void OnBoosterButtonClicked()
        // {
        //     GameSound.I.PlaySFXButtonClick();
        // _boosterBTN.gameObject.SetActive(false);
        // GameManager.I.UseBooster();
        // }

        // private void OnPlayButtonClicked()
        // {
        //     GameManager.I.StartMinigame();
        //     _playingPNL.SetActive(true);
        //     _playBTN.gameObject.SetActive(false);
        // }


        public void SetData(Game5Controller controller, PlayerControl playerControl)
        {
            _levelTMP.text = $"Level {playerControl.CurrentLevel + 1}";
            _controller = controller;
            playerControl.Init(_controller, _powerBarUI);
            _headerStartObject.SetActive(true);
            _playingPNL.SetActive(false);
            SetActiveWarningPNL(false);
            // _powerBarUI.UpdateValue(0f);
            // InitBoosterButton();

            StartCoroutine(IE_StartingGame());
            IEnumerator IE_StartingGame()
            {
                _startTime = 5;
                while (_startTime > 0)
                {
                    _timeStartTMP.text = $"Game start in {_startTime} second";
                    _startTime--;
                    yield return new WaitForSeconds(1f);
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_TICK);
                }

                _timeStartTMP.text = $"Start!";
                yield return new WaitForSeconds(1f);

                _controller.StartGame();
                playerControl.Active();
                _headerStartObject.SetActive(false);
                _playingPNL.SetActive(true);
                _noticePullAnimation.DORestart();
            }
        }

        private void SetActiveWarningPNL(bool value) => _warningObject.SetActive(value);
        private void UpdateCounterText(string str) => _timeCounterTMP.text = str;
        private void Game5Control_OnWarning(bool value) => SetActiveWarningPNL(value);
        private void Game5Control_OnPowerBarChanged(float value) => _powerBarUI.UpdateValue(value);
        private void Game5Control_OnTimerChanged(string value) => UpdateCounterText(value);

        // private void InitBoosterButton()
        // {
        // _boosterBTN.gameObject.SetActive(true);
        // _boosterDescriptionTMP.text = $"+{_controller.BoosterTimeAdded}s";
        // }
    }
}
