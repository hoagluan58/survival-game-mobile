using System;
using CnControls;
using NFramework;
using SquidGame.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.Minigame19
{
    public class Minigame19MenuUI : BaseUIView
    {
        [SerializeField] private Button _startMinigameButton;
        [SerializeField] private GameObject _timerPanel;
        [SerializeField] private TextMeshProUGUI _timerTMP;
        [SerializeField] private SimpleJoystick _joystick;

        public SimpleJoystick Joystick => _joystick;

        private void OnEnable()
        {
            _startMinigameButton.onClick.AddListener(OnStartMinigameButtonClick);
        }

        private void OnDisable()
        {
            _startMinigameButton.onClick.RemoveListener(OnStartMinigameButtonClick);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _startMinigameButton.gameObject.SetActive(true);
            _timerPanel.gameObject.SetActive(false);
            _joystick.gameObject.SetActive(false);
        }

        private void OnStartMinigameButtonClick()
        {
            _startMinigameButton.gameObject.SetActive(false);
            _timerPanel.gameObject.SetActive(true);
            _joystick.gameObject.SetActive(true);
            GameManager.I.StartMinigame();
        }

        public void UpdateTimer(float secondsLeft)
        {
            _timerTMP.text = $"{secondsLeft:N0}";
        }
    }
}