using SquidGame.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class DailyNormalItemUI : MonoBehaviour
    {
        [Header("Normal")]
        [SerializeField] private GameObject _normalGroup;
        [SerializeField] private Image _bgIMG;
        [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _claimableSprite;
        [SerializeField] private TextMeshProUGUI _dayTMP;
        [SerializeField] private TextMeshProUGUI _amountTMP;

        [Header("Complete")]
        [SerializeField] private GameObject _completeGroup;
        [SerializeField] private TextMeshProUGUI _dayTMP_2;

        public void SetNormal(int day, int amount, bool isClaimable)
        {
            ToggleState(false);
            _bgIMG.sprite = isClaimable ? _claimableSprite : _normalSprite;
            _dayTMP.text = GameLocalization.I.GetStringFromTable("STRING_DAY", day);
            _amountTMP.text = $"{amount}";
        }

        public void SetComplete(int day)
        {
            ToggleState(true);
            _dayTMP_2.text = GameLocalization.I.GetStringFromTable("STRING_DAY", day);
        }

        private void ToggleState(bool isComplete)
        {
            _normalGroup.SetActive(!isComplete);
            _completeGroup.SetActive(isComplete);
        }
    }
}
