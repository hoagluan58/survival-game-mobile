using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame6.Ddakji
{
    public class DdakjiGameUI : MonoBehaviour
    {
        [Header("TARGET")]
        [SerializeField] private GameObject _targetGroup;
        [SerializeField] private Button _targetBTN;

        [Header("FILL")]
        [SerializeField] private GameObject _fillGroup;
        [SerializeField] private Image _fillIMG;
        [SerializeField] private RectTransform _bgBar;
        [SerializeField] private Image _edgeIMG;
        [SerializeField] private RectTransform _containRect;
        [SerializeField] private Button _fillBTN;

        [Header("REF")]
        [SerializeField] private DdakjiGameConfigSO _configSO;

        private DdakjiHandler _ddakjiHandler;

        private void OnEnable()
        {
            _targetBTN.onClick.AddListener(OnTargetButtonClicked);
            _fillBTN.onClick.AddListener(OnFillButtonClicked);
        }

        private void OnDisable()
        {
            _targetBTN.onClick.RemoveListener(OnTargetButtonClicked);
            _fillBTN.onClick.RemoveListener(OnFillButtonClicked);
        }

        private void OnTargetButtonClicked()
        {
            _targetGroup.SetActive(false);
            _ddakjiHandler.EndMovingTarget();
            StartFillGame();
        }

        private void OnFillButtonClicked()
        {
            _fillGroup.SetActive(false);
            _ddakjiHandler.Throw(IsOverLap());
        }

        public void Init(DdakjiHandler ddakjiHandler)
        {
            _ddakjiHandler = ddakjiHandler;
            _targetGroup.SetActive(false);
            _fillGroup.SetActive(false);
        }

        public void StartMovingTarget()
        {
            _targetGroup.SetActive(true);
        }

        public void UpdateTargetButton(bool isValid)
        {
            _targetBTN.interactable = isValid;
        }

        public void StartFillGame()
        {
            _fillGroup.SetActive(true);

            // Tween fill bar
            _fillIMG.DOKill();
            _fillIMG.fillAmount = 0f;
            var duration = 1f;
            _fillIMG.DOFillAmount(1f, duration)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Yoyo)
                    .OnUpdate(() =>
                    {
                        SetEdgePosition();

                        void SetEdgePosition()
                        {
                            var edgePos = _fillIMG.rectTransform.rect.height * _fillIMG.fillAmount;
                            _edgeIMG.rectTransform.anchoredPosition = new Vector2(0f, edgePos);
                        }
                    });

            // Tween contain image
            _containRect.DOKill();
            var maxY = _bgBar.rect.height - _containRect.rect.height;

            _containRect.anchoredPosition = new Vector2(0, maxY);

            var rndStartY = UnityEngine.Random.Range(0f, maxY);
            _containRect.DOAnchorPosY(0, duration)
                       .SetEase(Ease.Linear)
                       .SetLoops(-1, LoopType.Yoyo);
        }

        private bool IsOverLap()
        {
            var rect1 = GetWorldRect(_edgeIMG.rectTransform);
            var rect2 = GetWorldRect(_containRect);

            return rect1.Overlaps(rect2);

            Rect GetWorldRect(RectTransform rectTransform)
            {
                Vector3[] corners = new Vector3[4];
                rectTransform.GetWorldCorners(corners);

                float width = corners[2].x - corners[0].x;
                float height = corners[2].y - corners[0].y;

                return new Rect(corners[0].x, corners[0].y, width, height);
            }
        }
    }
}
