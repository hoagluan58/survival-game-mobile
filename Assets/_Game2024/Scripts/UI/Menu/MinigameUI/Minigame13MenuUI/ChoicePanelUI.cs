using DG.Tweening;
using NFramework;
using Redcode.Extensions;
using SquidGame.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.Minigame13.UI
{
    public class ChoicePanelUI : MonoBehaviour
    {
        private const int CHOICE_COUNT = 3;

        [SerializeField] private RectTransform _panelRt;
        [SerializeField] private RectTransform _arrowRt;
        [SerializeField] private Button _btnStop;
        [SerializeField] private TextMeshProUGUI _turnTMP;
        [SerializeField] private TextMeshProUGUI _tutorialTMP;

        public event Action<EChoice> OnChoiceRunning;

        private EChoice _currentChoice;
        private BattleController _battleController;
        private Tween _arrowTween;

        private void OnEnable() => _btnStop.onClick.AddListener(OnStopButtonClicked);

        private void OnDisable() => _btnStop.onClick.RemoveListener(OnStopButtonClicked);

        private void OnStopButtonClicked() => UserStop();

        public void Init(BattleController battleController) => _battleController = battleController;

        public void SetActive(bool value) => gameObject.SetActive(value);

        public void Show(bool isUser)
        {
            SetActive(true);
            SetTurnText(isUser);
            _btnStop.gameObject.SetActive(isUser);
            RunTweenArrow(OnTweenUpdate);
            ShowTutorialText(isUser);

            if (!isUser)
            {
                var rndTime = UnityEngine.Random.Range(1f, 5f);
                this.InvokeDelay(rndTime, () =>
                {
                    _arrowTween?.Kill();
                    _battleController.SetBotChoice(_currentChoice);
                });
            }

            void OnTweenUpdate()
            {
                _currentChoice = (EChoice)GetCurrentChoice(_arrowRt.anchoredPosition.x);

                if (!isUser)
                {
                    OnChoiceRunning?.Invoke(_currentChoice);
                }
            }
        }

        private void SetTurnText(bool isUser)
        {
            if (isUser)
            {
                _turnTMP.text = GameLocalization.I.GetStringFromTable("STRING_YOUR_TURN");
            }
            else
            {
                _turnTMP.text = GameLocalization.I.GetStringFromTable("STRING_PLAYER_TURN", $"{_battleController.BotName}'s");
            }
        }

        private void RunTweenArrow(Action onTweenUpdate)
        {
            var maxX = _panelRt.rect.width;
            _arrowRt.SetAnchoredPositionX(0f);
            _arrowTween = _arrowRt.DOAnchorPosX(maxX, UnityEngine.Random.Range(1f, 2f))
                                  .OnUpdate(() => onTweenUpdate?.Invoke())
                                  .SetLoops(-1, LoopType.Yoyo)
                                  .SetEase(Ease.Linear);
        }

        private void UserStop()
        {
            _btnStop.gameObject.SetActive(false);
            _arrowTween?.Kill();
            var choice = (EChoice)GetCurrentChoice(_arrowRt.anchoredPosition.x);
            _battleController.SetPlayerChoice(choice);
            ShowTutorialText(false);
        }

        private int GetCurrentChoice(float posX)
        {
            var partSize = _panelRt.rect.width / CHOICE_COUNT;
            var index = Mathf.FloorToInt(posX / partSize);
            return Mathf.Clamp(index, 0, CHOICE_COUNT);
        }

        private void ShowTutorialText(bool value) => _tutorialTMP.gameObject.SetActive(value);
    }
}
