using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame3
{
    public class Minigame03MenuUI : BaseUIView
    {
        [SerializeField] private PlayerPanelUI _playerPanelUI;
        [SerializeField] private CutDalgonaPanelUI _cutDalgonaPanelUI;
        [SerializeField] private BreakDalgonaPanelUI _breakDalgonaPanelUI;
        public PlayerPanelUI PlayerPanelUI => _playerPanelUI;
        public CutDalgonaPanelUI CutDalgonaPanelUI => _cutDalgonaPanelUI;
        public BreakDalgonaPanelUI BreakDalgonaPanelUI => _breakDalgonaPanelUI;
        [SerializeField] private TextMeshProUGUI _txtLevel;
        [SerializeField] private Button _btnSetting;
        [SerializeField] private Image _imgBlackScreen;


        private void Awake()
        {
            _btnSetting.onClick.AddListener(OnClickBtnSetting);
        }

        private void OnDestroy()
        {
            _btnSetting.onClick.RemoveListener(OnClickBtnSetting);
        }

        public void Init(MinigameController minigameController)
        {
            _playerPanelUI.Init(minigameController, minigameController.PlayerController, minigameController.CameraController.CinemachineFreeLook);
            _breakDalgonaPanelUI.Init(minigameController.BreakDalgonaController);
        }


        public void SetActivePanel(PanelType panelType)
        {
            SetActiveAllPanel(false);
            switch (panelType)
            {
                case PanelType.Player:
                    _playerPanelUI.gameObject.SetActive(true);
                    break;
                case PanelType.DalgonaCut:
                    _cutDalgonaPanelUI.gameObject.SetActive(true);
                    break;
                case PanelType.DalgonaBreak:
                    _breakDalgonaPanelUI.gameObject.SetActive(true);
                    break;
            }
        }


        public void SetActiveAllPanel(bool isActive)
        {
            _playerPanelUI.gameObject.SetActive(isActive);
            _cutDalgonaPanelUI.gameObject.SetActive(isActive);
            _breakDalgonaPanelUI.gameObject.SetActive(isActive);
        }

        public void SetTextLevel(int level)
        {
            _txtLevel.text = $"Level {level}";
        }

        private void OnClickBtnSetting()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open<SettingsPopupUI>(Define.UIName.SETTINGS_POPUP);
        }
        public void DoFadingBlackScreen(float fadeTime, Action onStartFadeOut = null, Action onComplete = null)
        {
            if(fadeTime <= 0)
            {
                onStartFadeOut?.Invoke();
                onComplete?.Invoke();
                return;
            }
            _imgBlackScreen.raycastTarget = true;
            _imgBlackScreen.DOFade(1f, fadeTime)
                .SetLoops(2, LoopType.Yoyo)
                .OnStepComplete(() =>
                {
                    onStartFadeOut?.Invoke();
                })
                .OnComplete(() =>
                {
                    _imgBlackScreen.raycastTarget = false;
                    onComplete?.Invoke();
                });
        }

    }
    public enum PanelType
    {
        Player,
        DalgonaCut,
        DalgonaBreak,
    }
}
