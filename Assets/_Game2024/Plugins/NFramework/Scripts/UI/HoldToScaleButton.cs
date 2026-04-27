using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NFramework
{
    [RequireComponent(typeof(Button))]
    public class HoldToScaleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Vector3 _scaleValue = new Vector3(0.9f, 0.9f, 0.9f);

        private Vector3 _baseScale;
        private bool _onHold;
        private Button _button;

        private void Awake()
        {
            _baseScale = transform.localScale;
            _button = GetComponent<Button>();
        }

        private void OnDisable()
        {
            _onHold = false;
            transform.localScale = _baseScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _onHold = true && _button.interactable;
            if (_onHold)
                transform.localScale = _scaleValue;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_onHold)
            {
                _onHold = false;
                transform.localScale = _baseScale;
            }
        }
    }
}