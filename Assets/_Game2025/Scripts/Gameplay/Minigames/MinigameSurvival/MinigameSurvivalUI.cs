using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Survival
{
    public class MinigameSurvivalUI : BaseUIView
    {
        [SerializeField] private VariableJoystick _joystick;
        [SerializeField] private UITouchPanel _uiTouchPanel;
        [SerializeField] private Button _attackButton, _settingButton;
        [SerializeField] private GameObject _amountPanel, _headerNoti;
        [SerializeField] private TMP_Text _startText, _amountText, _levelText;
        [SerializeField] private Image _imgHurt;

        private Action _attackAction;
        private Tween _hurtTween;

        private void OnEnable()
        {
            _joystick.OnPointerUp(null);
        }

        private void Awake()
        {
            PlayerController.OnPlayerRevive += OnPlayerRevive;
            PlayerController.OnPlayerEquipWeapon += DisplayButtonAttack;
            PlayerController.OnPlayerHit += DoHurtEffect;
        }

        private void OnDestroy()
        {
            PlayerController.OnPlayerRevive -= OnPlayerRevive;
            PlayerController.OnPlayerEquipWeapon -= DisplayButtonAttack;
            PlayerController.OnPlayerHit -= DoHurtEffect;
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _settingButton.onClick.AddListener(OnSettingButtonClicked);
            _attackButton.gameObject.SetActive(false);
        }

        private void OnPlayerRevive(bool weaponEquipped)
        {
            SubscribeAttackEvent();
            if (weaponEquipped)
                DisplayButtonAttack();
        }

        private void OnSettingButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
        }

        public override void OnClose()
        {
            base.OnClose();
            _attackButton.onClick.RemoveAllListeners();
            _settingButton.onClick.RemoveListener(OnSettingButtonClicked);
        }

        public void Init(PlayerController playerController, CinemachineFreeLookInput cinemachineFreeLookInput)
        {
            _attackAction = playerController.Attack;
            SubscribeAttackEvent();
            playerController.InitJoystick(_joystick);
            cinemachineFreeLookInput.InitTouchPanel(_uiTouchPanel);
        }

        public void ResetGame()
        {
            _headerNoti.gameObject.SetActive(true);
            _amountPanel.SetActive(false);
        }

        public void StartGame()
        {
            _amountPanel.SetActive(true);
            _headerNoti.SetActive(false);
        }

        public void UpdateAmountText(string value)
        {
            _amountText.text = $"{value}";
        }

        public void UpdateStartText(string value)
        {
            _startText.text = $"{value}";
        }

        public void UpdateLevelText(int value)
        {
            _levelText.text = $"Level {value}";
        }

        public void DisplayButtonAttack()
        {
            if (!_attackButton.gameObject.activeSelf)
                _attackButton.gameObject.SetActive(true);
        }

        public void DoHurtEffect()
        {
            _hurtTween?.Kill(true);
            _hurtTween = _imgHurt.DOFade(0, 0);
            _hurtTween = _imgHurt.DOFade(1, 0.2f).SetLoops(2, LoopType.Yoyo);
        }

        public void SubscribeAttackEvent()
        {
            _attackButton.onClick.RemoveAllListeners();
            _attackButton.onClick.AddListener(() => _attackAction?.Invoke());
        }
    }
}
