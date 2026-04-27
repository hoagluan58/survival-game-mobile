using AssetKits.ParticleImage;
using DG.Tweening;
using SquidGame.LandScape;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.Minigame03.UI
{
    public class BreakDalgonaPanelUI : MonoBehaviour
    {
        private float MIN = -55f;
        private float MAX = 55f;

        [SerializeField] private Transform _arrowPivot;
        [SerializeField] private Button _stopBTN;

        private bool _isValid;
        private BreakDalgonaStep _step;
        private Tween _tweenArrow;

        private void OnEnable() => _stopBTN.onClick.AddListener(OnStopButtonClicked);

        private void OnDisable() => _stopBTN.onClick.RemoveListener(OnStopButtonClicked);

        private void OnStopButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            ToggleTweenArrow(false);
            var z = GetCurArrowRotationZ();
            if (z <= MAX && z >= MIN)
            {
                _step.BreakTrue();
            }
            else
            {
                _step.BreakWrong();
            }
        }

        public void Init(BreakDalgonaStep step) => _step = step;

        public void SetActive(bool value) => gameObject.SetActive(value);

        public void ToggleTweenArrow(bool value, float speed = 2f)
        {
            _stopBTN.gameObject.SetActive(value);
            if (!value)
            {
                _tweenArrow.Pause();
                return;
            }
            _arrowPivot.localEulerAngles = new Vector3(0, 0, 85f);
            _tweenArrow?.Kill();
            _tweenArrow = _arrowPivot.DOLocalRotate(new Vector3(0, 0, -85f), speed)
                                     .SetLoops(-1, LoopType.Yoyo)
                                     .SetEase(Ease.Linear)
                                     .Pause();
            _tweenArrow.Play();
        }

        private float GetCurArrowRotationZ()
        {
            var curZ = _arrowPivot.localEulerAngles.z;
            if (curZ > 180f)
            {
                curZ -= 360f;
            }
            return curZ;
        }
    }
}
