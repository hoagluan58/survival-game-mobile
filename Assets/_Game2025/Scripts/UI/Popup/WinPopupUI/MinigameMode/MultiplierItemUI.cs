using TMPro;
using UnityEngine;

namespace SquidGame.LandScape.UI
{
    public class MultiplierItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        [Header("CONFIG")]
        [SerializeField] private int _multiplyAmount;

        private RectTransform _rectTransform;
        public int MultiplyAmount => _multiplyAmount;

        private void Awake() => _rectTransform = GetComponent<RectTransform>();

        private void OnEnable() => _text.SetText($"x{_multiplyAmount}");

        public bool IsArrowInPos(float xPos)
        {
            var itemX = _rectTransform.anchoredPosition.x;
            var halfWidth = _rectTransform.sizeDelta.x / 2f;

            return xPos >= (itemX - halfWidth) && xPos <= (itemX + halfWidth);
        }
    }
}
