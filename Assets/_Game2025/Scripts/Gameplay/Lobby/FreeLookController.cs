using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SquidGame.LandScape.Game
{
    public class FreeLookController : MonoBehaviour, /* IDragHandler, */ IPointerDownHandler, IPointerUpHandler
    {
        CinemachineFreeLook _cinemachineFreeLock;
        Image _image;

        string _strMouseX = "Mouse X", _strMouseY = "Mouse Y";

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void Init(CinemachineFreeLook cinemachineFreeLook)
        {
            _cinemachineFreeLock = cinemachineFreeLook;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _image.rectTransform,
                eventData.position,
                eventData.enterEventCamera,
                out Vector2 position))
            {
                _cinemachineFreeLock.m_XAxis.m_InputAxisName = _strMouseX;
                _cinemachineFreeLock.m_YAxis.m_InputAxisName = _strMouseY;
                // _cinemachineFreeLock.m_RecenterToTargetHeading.m_enabled = false;

            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _cinemachineFreeLock.m_XAxis.m_InputAxisName = null;
            _cinemachineFreeLock.m_YAxis.m_InputAxisName = null;
            _cinemachineFreeLock.m_XAxis.m_InputAxisValue = 0;
            _cinemachineFreeLock.m_YAxis.m_InputAxisValue = 0;
            // _cinemachineFreeLock.m_RecenterToTargetHeading.m_enabled = true;
        }
    }
}
