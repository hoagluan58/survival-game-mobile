using System;
using DG.Tweening;
using Redcode.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.Minigame15
{
    public class ForceSelector : MonoBehaviour
    {
        public event Action<float, float> SelectCompleteEvent;

        [SerializeField] private Transform _selectPanel;
        [Header("DIRECTION SELECT")]
        [SerializeField] private Button _directionSelectButton;
        [SerializeField] private Image _directionArrow;
        [Header("FORCE SELECT")]
        [SerializeField] private Button _forceSelectButton;
        [SerializeField] private Image _forceBar;

        private float _direction;
        private float _force;

        private void OnEnable()
        {
            _directionSelectButton.onClick.AddListener(OnSelectDirectionButtonClick);
            _forceSelectButton.onClick.AddListener(OnForceSelectButtonClick);
        }

        private void OnDisable()
        {
            _directionSelectButton.onClick.RemoveListener(OnSelectDirectionButtonClick);
            _forceSelectButton.onClick.RemoveListener(OnForceSelectButtonClick);
        }

        private void OnSelectDirectionButtonClick()
        {
            _directionSelectButton.gameObject.SetActive(false);
            _forceSelectButton.gameObject.SetActive(true);

            _directionArrow.transform.DOKill();

            var arrowEulerZ = _directionArrow.rectTransform.localEulerAngles.z >= 300
                ? _directionArrow.rectTransform.localEulerAngles.z - 360
                : _directionArrow.rectTransform.localEulerAngles.z;
            _direction = -arrowEulerZ / 60f;

            _forceBar.DOFillAmount(1f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);
        }

        private void OnForceSelectButtonClick()
        {
            _directionSelectButton.gameObject.SetActive(false);
            _forceSelectButton.gameObject.SetActive(false);

            _forceBar.DOKill();
            _force = _forceBar.fillAmount;
            SelectCompleteEvent?.Invoke(_direction, _force);
        }

        public void Show()
        {
            gameObject.SetActive(true);


            _directionSelectButton.gameObject.SetActive(true);
            _forceSelectButton.gameObject.SetActive(false);

            _forceBar.fillAmount = 0;

            _directionArrow.transform.SetEulerAnglesZ(60);
            _directionArrow.transform.DOLocalRotate(Vector3.forward * -60f, 1f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}