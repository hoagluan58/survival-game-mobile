using NFramework;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.UI
{
    public class ShopMenuUI : BaseUIView
    {
        [SerializeField] private Button _backBTN;
        [SerializeField] private ShopScrollerUI _shopScroller;

        public override void OnOpen()
        {
            base.OnOpen();
            _backBTN.onClick.AddListener(OnBackButtonClicked);
            _shopScroller.SetData();
        }

        public override void OnClose()
        {
            base.OnClose();
            _backBTN.onClick.RemoveListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            CloseSelf();
        }
    }
}
