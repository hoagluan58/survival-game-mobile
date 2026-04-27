using DG.Tweening;
using NFramework;
using Redcode.Extensions;
using SquidGame.Core;
using SquidGame.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.Minigame15
{
    public class Minigame15MenuUI : BaseUIView
    {
        [Header("Minigame15MenuUI")]
        [SerializeField] private Button _startButton;
        [SerializeField] private ScorePanel _scorePanel;

        [SerializeField] private TextMeshProUGUI _announcerTMP;

        public TextMeshProUGUI AnnouncerTMP => _announcerTMP;

        private void Start()
        {
            _startButton.onClick.AddListener(OnStartMinigameButtonClick);
        }

        private void OnStartMinigameButtonClick()
        {
            GameManager.I.StartMinigame();
            _startButton.gameObject.SetActive(false);
            _scorePanel.gameObject.SetActive(true);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _startButton.gameObject.SetActive(true);
            _scorePanel.gameObject.SetActive(false);
            _announcerTMP.gameObject.SetActive(false);
            _scorePanel.Init();
        }

        public void PlayShowScoreAnimation(float score)
        {
            _announcerTMP.gameObject.SetActive(true);
            _announcerTMP.rectTransform.SetAnchoredPositionY(_announcerTMP.rectTransform.anchoredPosition.y - 40f);
            _announcerTMP.rectTransform.DOAnchorPosY(_announcerTMP.rectTransform.anchoredPosition.y + 40f, 0.5f);
            DOVirtual.Float(0, score, 0.5f, value =>
            {
                _announcerTMP.text = GameLocalization.I.GetStringFromTable("STRING_SCORE", value.ToString("F2"));
            });
        }

        public void PlayTextMessageAnimation(string message)
        {
            _announcerTMP.gameObject.SetActive(true);
            _announcerTMP.rectTransform.SetAnchoredPositionY(_announcerTMP.rectTransform.anchoredPosition.y - 40f);
            _announcerTMP.rectTransform.DOAnchorPosY(_announcerTMP.rectTransform.anchoredPosition.y + 40f, 0.5f);
            _announcerTMP.text = message;
        }

        public void UpdateScore(int playerRoundWinCount, int opponentRoundWinCount)
        {
            _scorePanel.UpdateScore(playerRoundWinCount, opponentRoundWinCount);
        }
    }
}