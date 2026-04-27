using DG.Tweening;
using NFramework;
using Redcode.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame3
{
    public class BreakDalgonaPanelUI : MonoBehaviour
    {
        private float MIN = -55f;
        private float MAX = 55f;

        [SerializeField] private Image _imgPanel;
        [SerializeField] private RectTransform _rtfGreenArea;
        [SerializeField] private RectTransform _arrowPivot;
        [SerializeField] private Button _stopBTN;

        private float _initXSize;
        private BreakDalgonaController _step;
        private Tween _tweenArrow;

        private void OnEnable() => _stopBTN.onClick.AddListener(OnStopButtonClicked);

        private void OnDisable() => _stopBTN.onClick.RemoveListener(OnStopButtonClicked);

        public void LoadConfig(Minigame03Config config)
        {
            MIN = config.MinValue;
            MAX = config.MaxValue;
            _rtfGreenArea.SetSizeDeltaX(_initXSize * config.GreenAreaPercentage);
        }

        private void OnStopButtonClicked()
        {
            //GameSound.I.PlaySFXButtonClick();
            VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
            ToggleTweenArrow(false);
            var z = _arrowPivot.anchoredPosition.x;
            if (z <= MAX && z >= MIN)
            {
                _step.BreakTrue();
            }
            else
            {
                _step.BreakWrong();
            }
        }

        public void Init(BreakDalgonaController step)
        {
            _step = step;
            if (_initXSize != 0) return;
            _initXSize = _rtfGreenArea.sizeDelta.x;
        }

        public void SetActive(bool value) => gameObject.SetActive(value);

        public void ToggleTweenArrow(bool value, float speed = 2f)
        {
            _stopBTN.gameObject.SetActive(value);
            if (!value)
            {
                _tweenArrow.Pause();
                return;
            }
            _arrowPivot.SetAnchoredPositionX(0);
            _tweenArrow?.Kill();
            _tweenArrow = _arrowPivot.DOAnchorPosX(_imgPanel.rectTransform.sizeDelta.x, speed)
                                     .SetLoops(-1, LoopType.Yoyo)
                                     .SetEase(Ease.Linear)
                                     .Pause();
            _tweenArrow.Play();
        }

        //private float GetCurArrowRotationZ()
        //{
        //    var curZ = _arrowPivot.localEulerAngles.z;
        //    if (curZ > 180f)
        //    {
        //        curZ -= 360f;
        //    }
        //    return curZ;
        //}
    }
}
