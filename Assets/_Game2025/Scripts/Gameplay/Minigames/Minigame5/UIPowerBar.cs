using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame5
{
    public class UIPowerBar : MonoBehaviour
    {
        [SerializeField] private Image _fill;
        [SerializeField] private RectTransform _fillRect;
        [SerializeField] private RectTransform _lineTarget;
        [SerializeField] private RectTransform _leftArrow;
        [SerializeField] private RectTransform _rightArrow;
        [SerializeField] private Vector2 _posRange;
        [SerializeField] private Color[] _fillColor;

        float _padding = 12f;
        Vector2 _powerRange;

        public void Init(int level, Vector2 range)
        {
            UpdateValue(0f);
            ShowTargetRange(level == 0);

            void ShowTargetRange(bool firstLevel)
            {
                _powerRange = range;
                _lineTarget.gameObject.SetActive(firstLevel);
                _leftArrow.gameObject.SetActive(!firstLevel);
                _rightArrow.gameObject.SetActive(!firstLevel);

                if (firstLevel)
                    _lineTarget.anchoredPosition = new Vector2((_posRange.y - _padding) * 2 * range.x, _lineTarget.anchoredPosition.y);
                else
                {
                    _leftArrow.anchoredPosition = new Vector2((_posRange.y - _padding) * 2 * range.x, _leftArrow.anchoredPosition.y);
                    _rightArrow.anchoredPosition = new Vector2((_posRange.y - _padding) * 2 * range.y, _rightArrow.anchoredPosition.y);
                }
            }
        }

        public void UpdateValue(float fill)
        {
            _fill.fillAmount = fill;

            Vector3 fillPos = _fillRect.anchoredPosition3D;
            fillPos.x = Mathf.Lerp(_posRange.x, _posRange.y, fill);
            _fillRect.anchoredPosition3D = fillPos;

            if (fill >= _powerRange.x && fill <= _powerRange.y)
            {
                _fill.color = _fillColor[1];
            }
            else
            {
                _fill.color = _fillColor[0];
            }
        }
    }
}
