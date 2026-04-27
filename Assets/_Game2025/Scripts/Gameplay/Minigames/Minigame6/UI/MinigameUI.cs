using DG.Tweening;
using NFramework;
using Redcode.Extensions;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Minigame6.Ddakji;
using SquidGame.LandScape.Minigame6.ThrowStoneGame;
using SquidGame.LandScape.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame6.UI
{
    public class MinigameUI : BaseUIView
    {
        [SerializeField] private TextMeshProUGUI _timeTMP;
        [SerializeField] private Button _settingBTN;
        [SerializeField] private Button _jumpBTN;
        [SerializeField] private MinigameTutorialPanelUI _tutorialPNL;
        [SerializeField] private GameObject _succeedTMP;

        [Header("THROW STONE GAME")]
        [SerializeField] private ThrowStoneUI _throwStoneUI;

        [Header("DDakji GAME")]
        [SerializeField] private DdakjiGameUI _ddakjiGameUI;

        private Action _onJumpButtonClicked;
        public MinigameTutorialPanelUI TutorialPNL => _tutorialPNL;
        public ThrowStoneUI ThrowStoneUI => _throwStoneUI;
        public DdakjiGameUI DdakjiGameUI => _ddakjiGameUI;

        private void OnEnable()
        {
            _settingBTN.onClick.AddListener(OnSettingButtonClicked);
            _jumpBTN.onClick.AddListener(OnJumpButtonClicked);
        }

        private void OnDisable()
        {
            _settingBTN.onClick.RemoveListener(OnSettingButtonClicked);
            _jumpBTN.onClick.RemoveListener(OnJumpButtonClicked);
        }

        private void OnSettingButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
        }

        private void OnJumpButtonClicked()
        {
            _onJumpButtonClicked?.Invoke();
            GameSound.I.PlaySFXButtonClick();
            SetActiveJumpButton(false);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            SetActiveThrowStoneUI(false);
            SetActiveDdakjiUI(false);
            
        }

        public void Init()
        {
            HideTimeText();
            SetActiveJumpButton(false);
        }

        public void SetJumpButtonCallback(Action callback) => _onJumpButtonClicked = callback;

        public void ShowTimeText(float time)
        {
            _timeTMP.gameObject.SetActive(true);
            UpdateTimeText(time);
        }

        public void UpdateTimeText(float time) => _timeTMP.text = $"{time}";

        public void HideTimeText() => _timeTMP.gameObject.SetActive(false);

        public void SetActiveJumpButton(bool value) => _jumpBTN.gameObject.SetActive(value);

        public void SetActiveThrowStoneUI(bool value) => _throwStoneUI.gameObject.SetActive(value);

        public void SetActiveDdakjiUI(bool value) => _ddakjiGameUI.gameObject.SetActive(value);

        public void ShowSucceedTMP(bool value)
        {
            var rect = _succeedTMP.GetComponent<RectTransform>();
            rect.SetAnchoredPositionY(200);
            rect.DOAnchorPosY(value ? -24 : 200,0.35f).SetEase(Ease.OutBack);
            _succeedTMP.SetActive(value);
        }
    }
}
