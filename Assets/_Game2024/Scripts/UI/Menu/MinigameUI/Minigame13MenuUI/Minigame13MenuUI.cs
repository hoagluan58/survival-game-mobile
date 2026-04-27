using DG.Tweening;
using Redcode.Extensions;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.Minigame13.UI
{
    public class Minigame13MenuUI : MonoBehaviour
    {
        [SerializeField] private Button _playBTN;

        [Header("PLAYING")]
        [SerializeField] private GameObject _playingPNL;
        [SerializeField] private Image _choiceIMG;
        [SerializeField] private ScorePanelUI _playerScorePNL;
        [SerializeField] private ScorePanelUI _enemyScorePNL;
        [SerializeField] private ChoicePanelUI _choicePNL;
        [SerializeField] private TextMeshProUGUI _resultTMP;

        [Header("ASSETS")]
        [SerializeField] private Sprite _rockIcon;
        [SerializeField] private Sprite _scissorsIcon;
        [SerializeField] private Sprite _paperIcon;

        private MinigameController _controller;

        private void OnEnable()
        {
            _playBTN.onClick.AddListener(OnPlayButtonClicked);
            _choicePNL.OnChoiceRunning += ChoicePNL_OnChoiceRunning;
        }

        private void OnDisable()
        {
            _playBTN.onClick.RemoveListener(OnPlayButtonClicked);
            _choicePNL.OnChoiceRunning -= ChoicePNL_OnChoiceRunning;
        }

        private void ChoicePNL_OnChoiceRunning(EChoice choice)
        {
            SetActiveBotChoicePNL(true);
            SetChoiceColor(choice);
        }

        private void OnPlayButtonClicked()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_BUTTON_CLICK);
            _playBTN.gameObject.SetActive(false);
            GameManager.I.StartMinigame();
        }

        public void SetData(MinigameController controller)
        {
            _controller = controller;
            _choicePNL.Init(_controller.BattleController);
            _playBTN.gameObject.SetActive(true);
            ShowPlayingPNL(false);
            _playerScorePNL.SetData(GameLocalization.I.GetStringFromTable("STRING_YOU"));
            _enemyScorePNL.SetData(_controller.BattleController.BotName);
            SetActiveBotChoicePNL(false);
        }

        public void ShowPlayingPNL(bool value) => _playingPNL.SetActive(value);

        public void ShowChoicePNL(bool isUser) => _choicePNL.Show(isUser);

        public void UpdateScore(int userScore, int botScore)
        {
            SetActiveBotChoicePNL(false);
            _choicePNL.SetActive(false);
            _playerScorePNL.UpdateScore(userScore);
            _enemyScorePNL.UpdateScore(botScore);
        }

        public void ShowRoundResult(EResult result)
        {
            _resultTMP.gameObject.SetActive(true);
            _resultTMP.rectTransform.SetAnchoredPositionY(_resultTMP.rectTransform.anchoredPosition.y - 40f);
            _resultTMP.rectTransform.DOAnchorPosY(_resultTMP.rectTransform.anchoredPosition.y + 40f, 0.5f);
            _resultTMP.text = result switch
            {
                EResult.Draw => GameLocalization.I.GetStringFromTable("STRING_DRAW"),
                EResult.Win => GameLocalization.I.GetStringFromTable("STRING_YOU_WIN"),
                _ => GameLocalization.I.GetStringFromTable("STRING_YOU_LOSE")
            };
        }

        private void SetActiveBotChoicePNL(bool value) => _choiceIMG.gameObject.SetActive(value);

        private void SetChoiceColor(EChoice choice)
        {
            switch (choice)
            {
                case EChoice.Rock:
                    _choiceIMG.sprite = _rockIcon;
                    break;
                case EChoice.Paper:
                    _choiceIMG.sprite = _paperIcon;
                    break;
                case EChoice.Scissors:
                    _choiceIMG.sprite = _scissorsIcon;
                    break;
                default:
                    _choiceIMG.sprite = _rockIcon;
                    break;
            }
        }

        public void HideRoundResultTMP()
        {
            _resultTMP.gameObject.SetActive(false);
        }
    }
}