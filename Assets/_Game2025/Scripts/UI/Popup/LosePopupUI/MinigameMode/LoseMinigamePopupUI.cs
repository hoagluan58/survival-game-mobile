using NFramework;
using SquidGame.LandScape.Game;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.UI
{
    public class LoseMinigamePopupUI : BaseUIView
    {
        [SerializeField] private Button _homeBTN;
        [SerializeField] private Button _replayBTN;

        public override void OnOpen()
        {
            base.OnOpen();
            _homeBTN.onClick.AddListener(OnHomeButtonClicked);
            _replayBTN.onClick.AddListener(OnReplayButtonClicked);
        }

        public override void OnClose()
        {
            base.OnClose();
            _homeBTN.onClick.RemoveListener(OnHomeButtonClicked);
            _replayBTN.onClick.RemoveListener(OnReplayButtonClicked);
        }

        private void OnHomeButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            CloseSelf();
            GameManager.I.Exit();
        }

        private void OnReplayButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            CloseSelf();
            GameManager.I.ReloadMinigame();
        }
    }
}
