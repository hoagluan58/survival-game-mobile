using NFramework;
using SquidGame.LandScape.Game;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.UI
{
    public class WinChallengePopupUI : BaseUIView
    {
        [SerializeField] private Button _homeBTN;
        [SerializeField] private Button _nextBTN;

        public override void OnOpen()
        {
            base.OnOpen();
            _homeBTN.onClick.AddListener(OnHomeButtonClicked);
            _nextBTN.onClick.AddListener(OnNextButtonClicked);
        }

        public override void OnClose()
        {
            base.OnClose();
            _homeBTN.onClick.RemoveListener(OnHomeButtonClicked);
            _nextBTN.onClick.RemoveListener(OnNextButtonClicked);
        }

        private void OnHomeButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            CloseSelf();
            GameManager.I.Exit();
        }

        private void OnNextButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            CloseSelf();
            var curSeasonId = GameManager.I.CurSeasonId;
            GameManager.I.PlayChallengeMode(curSeasonId);
        }
    }
}
