using NFramework;
using Redcode.Extensions;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.UI
{
    public class RateUsPopupUI : BaseUIView
    {
        [SerializeField] private RectTransform _contentPanel;
        [SerializeField] private Button _submitBTN;
        [SerializeField] private Button[] _rateButtons;
        [SerializeField] private Image[] _starImages;

        private int _currentStarIndex = -1;

        public override void OnOpen()
        {
            base.OnOpen();
            _contentPanel.DOPunchScalePopup();
            _submitBTN.gameObject.SetActive(false);

            _currentStarIndex = -1;
            DisableAllStar();

            _submitBTN.onClick.AddListener(OnSubmitButtonClicked);
            for (var i = 0; i < _rateButtons.Length; i++)
            {
                var index = i;
                _rateButtons[i].onClick.AddListener(() => OnStarButtonClicked(index));
            }
        }

        public override void OnClose()
        {
            base.OnClose();
            _submitBTN.onClick.RemoveListener(OnSubmitButtonClicked);
            _rateButtons.ForEach(x => x.onClick.RemoveAllListeners());

            var userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
            userData.IsShowRateUsPopup = true;
        }

        private void OnSubmitButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            CloseSelf();
        }

        IEnumerator StartRate()
        {
            if (_currentStarIndex + 1 >= 4)
            {

            }
            yield return new WaitForSeconds(0.5f);
        }
        private void OnStarButtonClicked(int index)
        {
            AnimUtils.DOScaleShow(_submitBTN.gameObject);
            UpdateStar(index);

            StartCoroutine(StartRate());
        }

        private void UpdateStar(int index)
        {
            if (index == _currentStarIndex) return;

            DisableAllStar();
            _currentStarIndex = index;
            StartCoroutine(CRStarAnim());

            IEnumerator CRStarAnim()
            {
                var waiter = new WaitForSeconds(0.1f);
                for (var i = 0; i <= _currentStarIndex; i++)
                {
                    _starImages[i].SetAlpha(1);
                    yield return waiter;
                }
            }
        }

        private void DisableAllStar() => _starImages.ForEach(star => star.SetAlpha(0));
    }
}
