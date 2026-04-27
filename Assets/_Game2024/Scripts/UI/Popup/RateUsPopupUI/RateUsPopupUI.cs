using NFramework;
using Redcode.Extensions;
using SquidGame.LandScape;
using SquidGame.SaveData;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
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
        }

        private void OnSubmitButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UserData.I.IsShowRateUsPopup = true;

        }
        IEnumerator StartRate()
        {
            UserData.I.IsShowRateUsPopup = true;
            if (_currentStarIndex + 1 >= 4)
            {
                // In-app review removed (mobile-only feature)
            }
            yield return new WaitForSeconds(0.5f);
            CloseSelf();
        }
        private void OnStarButtonClicked(int index)
        {
            GameSound.I.PlaySFXButtonClick();
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
