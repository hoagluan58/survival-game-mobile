using EnhancedUI.EnhancedScroller;
using NFramework;
using SquidGame.Core;
using SquidGame.LandScape;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class TrainingModeMenuUI : BaseUIView
    {
        [SerializeField] private TrainingItemScrollerUI _scroller;
        [SerializeField] private Button _homeBTN;
        [SerializeField] private Button _settingsBTN;

        public override void OnOpen()
        {
            base.OnOpen();
            _homeBTN.onClick.AddListener(OnHomeButtonClicked);
            _settingsBTN.onClick.AddListener(OnSettingsButtonClicked);
            _scroller.SetData();
        }

        public override void OnClose()
        {
            base.OnClose();
            _homeBTN.onClick.RemoveListener(OnHomeButtonClicked);
            _settingsBTN.onClick.RemoveListener(OnSettingsButtonClicked);
        }

        private void OnSettingsButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
        }

        private void OnHomeButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            CloseSelf();
        }
    }
}
