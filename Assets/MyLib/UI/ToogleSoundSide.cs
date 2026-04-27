using DG.Tweening;
using NFramework;
using UnityEngine;

namespace SquidGame.UI
{
    public class ToogleSoundSide : MonoBehaviour
    {
        [SerializeField] private EToggleType _type = default;
        [SerializeField] RectTransform _scrollRect = default;
        [SerializeField] private float _deltaScroll = 1f;
        [SerializeField] private float _timeScroll = 0.5f;

        private Vector3 _initPos;
        private bool _isOn;

        private void Start()
        {
            _initPos = _scrollRect.anchoredPosition3D;

            switch (_type)
            {
                case EToggleType.Haptic:
                    _isOn = VibrationManager.I.Status;
                    break;
                case EToggleType.SFX:
                    _isOn = NFramework.SoundManager.I.SFXStatus;
                    break;
                case EToggleType.BGM:
                    _isOn = NFramework.SoundManager.I.MusicStatus;
                    break;
            }

            UpdateUI(0f);
        }

        private void UpdateUI(float time)
        {
            if (!_isOn)
            {
                _scrollRect.DOAnchorPos3DX(_initPos.x - _deltaScroll, time);
            }
            else
            {
                _scrollRect.DOAnchorPos3DX(_initPos.x, time);
            }
        }

        public void ClickMe()
        {
            _isOn = !_isOn;
            UpdateUI(_timeScroll);

            switch (_type)
            {
                case EToggleType.Haptic:
                    VibrationManager.I.Status = _isOn;
                    break;
                case EToggleType.SFX:
                    NFramework.SoundManager.I.SFXStatus = _isOn;
                    break;
                case EToggleType.BGM:
                    NFramework.SoundManager.I.MusicStatus = _isOn;
                    break;
            }
        }
    }
}
