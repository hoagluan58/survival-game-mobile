using Game6;
using NFramework;
using SquidGame.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class Minigame06MenuUI : BaseUIView
    {
        [SerializeField] private Button _playBTN;

        [Header("PLAYING")]
        [SerializeField] private GameObject _playingPNL;
        [SerializeField] private UIHPBar _playerHPBar;
        [SerializeField] private UIHPBar _enemyHPBar;
        [SerializeField] private TextMeshProUGUI _enemyNameTMP;
        [SerializeField] private GameObject _warningPNL;
        [SerializeField] private GameObject _tutFight;
        [SerializeField] private VariableJoystick _joystick;

        public VariableJoystick Joystick => _joystick;

        private bool _isForceHideWarning;

        public override void OnOpen()
        {
            base.OnOpen();
            SetData();
            _playBTN.onClick.AddListener(OnPlayButtonClicked);
            Game6Control.OnShowTutorial += Game6Control_OnShowTutorial;
            Game6Control.OnShowWarning += Game6Control_OnShowWarning;
            Game6Control.OnPlayerHPChanged += Game6Control_OnPlayerHPChanged;
            Game6Control.OnEnemyHPChanged += Game6Control_OnEnemyHPChanged;
        }

        public override void OnClose()
        {
            base.OnClose();
            _playBTN.onClick.RemoveListener(OnPlayButtonClicked);
            Game6Control.OnShowTutorial -= Game6Control_OnShowTutorial;
            Game6Control.OnShowWarning -= Game6Control_OnShowWarning;
            Game6Control.OnPlayerHPChanged -= Game6Control_OnPlayerHPChanged;
            Game6Control.OnEnemyHPChanged -= Game6Control_OnEnemyHPChanged;
        }

        private void Game6Control_OnEnemyHPChanged(float value) => UpdateUIEnemyHpBar(value);

        private void Game6Control_OnPlayerHPChanged(float value) => UpdateUIPlayerHpBar(value);

        private void Game6Control_OnShowWarning(bool value, bool isForce) => SetActiveWarningPNL(value, isForce);

        private void Game6Control_OnShowTutorial(bool value) => ShowTutorialFight(value);

        private void OnPlayButtonClicked()
        {
            GameManager.I.StartMinigame();
            _playingPNL.SetActive(true);
            _playBTN.gameObject.SetActive(false);
        }

        private void UpdateUIPlayerHpBar(float value)
        {
            if (_isForceHideWarning == false)
            {
                if (value <= 0.15f)
                    SetActiveWarningPNL(true);
                else SetActiveWarningPNL(false);
            }
            _playerHPBar.UpdateUI(value);
        }

        public void UpdateUIEnemyHpBar(float value) => _enemyHPBar.UpdateUI(value);

        private void SetData()
        {
            _playBTN.gameObject.SetActive(true);
            _playingPNL.SetActive(false);
            _enemyNameTMP.text = "No." + Random.Range(0, 500);
            UpdateUIEnemyHpBar(1f);
            ShowTutorialFight(false);
        }

        private void SetActiveWarningPNL(bool value, bool isForceHide = false)
        {
            _isForceHideWarning = isForceHide;
            _warningPNL.SetActive(value);
        }

        private void ShowTutorialFight(bool value) => _tutFight.SetActive(value);
    }
}
