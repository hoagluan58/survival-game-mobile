using System.Collections;
using NFramework;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.Minigame18
{
    public class Minigame18MenuUI : BaseUIView
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private TextMeshProUGUI _countdownTMP;
        [SerializeField] private TextMeshProUGUI _tutorialTMP;

        protected void OnEnable()
        {
            _startButton.onClick.AddListener(OnStartButtonClick);
            PlayerController.FirstJumpEvent += OnPlayerFirstJump;
        }

        protected void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClick);
            PlayerController.FirstJumpEvent -= OnPlayerFirstJump;
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _startButton.gameObject.SetActive(true);
            _tutorialTMP.gameObject.SetActive(false);
            _countdownTMP.gameObject.SetActive(false);
        }

        private IEnumerator CountdownCoroutine()
        {
            GameManager.I.StartMinigame();
            var waitSecond = new WaitForSeconds(1f);
            _countdownTMP.text = "3";
            for (var i = 2; i > 0; i--)
            {
                yield return waitSecond;
                _countdownTMP.text = i.ToString();
            }
            yield return waitSecond;
            _countdownTMP.gameObject.SetActive(false);
            _tutorialTMP.gameObject.SetActive(true);
        }


        private void OnStartButtonClick()
        {
            GameSound.I.PlaySFXButtonClick();
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Selection);
            _startButton.gameObject.SetActive(false);
            _countdownTMP.gameObject.SetActive(true);
            StartCoroutine(CountdownCoroutine());
        }

        private void OnPlayerFirstJump()
        {
            _tutorialTMP.gameObject.SetActive(false);
        }
    }
}