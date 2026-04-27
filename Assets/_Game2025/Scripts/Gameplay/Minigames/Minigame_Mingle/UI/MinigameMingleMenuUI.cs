using Cinemachine;
using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SquidGame.LandScape.MinigameMingle
{
    public class MinigameMingleMenuUI : BaseUIView
    {
        [SerializeField] private CanvasGroup _sceneCanvasGroup;
        [SerializeField] private CanvasGroup _inputCanvasGroup;
        [SerializeField] private FreeLookController _freeLookController;
        [SerializeField] private VariableJoystick _variableJoystick;
        [SerializeField] private Button _jumpButton;
        [SerializeField] private NotificationPanel _notificationPanel;
        [SerializeField] private Button _settingButton;

        public VariableJoystick VariableJoystick => _variableJoystick;
        public NotificationPanel Panel => _notificationPanel;

        public MinigameMingleMenuUI InitFreeLookController(CinemachineFreeLook cinemachineFreeLook)
        {
            _freeLookController.Init(cinemachineFreeLook);
            return this;
        }


        public MinigameMingleMenuUI InitJumpButton(UnityAction action)
        {
            _jumpButton.onClick.AddListener(action);
            return this;
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _settingButton.onClick.AddListener(() =>
            {
                GameSound.I.PlaySFXButtonClick();
                UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
            });
        }

        public override void OnClose()
        {
            base.OnClose();
            _settingButton.onClick.RemoveAllListeners();
        }


        public void ShowInput(bool value, float duration)
        {
            _variableJoystick.ResetJoystick();
            _inputCanvasGroup.DOKill();
            _inputCanvasGroup.DOFade(value ? 1 : 0, duration).OnComplete(() =>
            {
                _inputCanvasGroup.blocksRaycasts = value ? true : false;
            });
        }


        public void Init()
        {
            ShowScene(false, 0);
            _notificationPanel.Hide(0f);
        }


        public void ShowScene(bool value, float duration)
        {
            if (duration <= 0)
            {
                _sceneCanvasGroup.alpha = value ? 1 : 0;
                _sceneCanvasGroup.blocksRaycasts = value ? true : false;
                return;
            }

            _sceneCanvasGroup.DOFade(value ? 1 : 0, duration);
            _sceneCanvasGroup.blocksRaycasts = value ? true : false;
        }


    }
}
