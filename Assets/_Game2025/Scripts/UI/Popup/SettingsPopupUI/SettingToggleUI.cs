using NFramework;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.UI
{
    [RequireComponent(typeof(Button))]
    public class SettingToggleUI : MonoBehaviour
    {
        [SerializeField] private EToggleType _type = default;
        [SerializeField] private Image _img = default;
        [SerializeField] private Sprite[] _spriteOnOff = default;

        private bool _isOn;
        private Button _button;

        private void Awake() => _button = GetComponent<Button>();

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
            Start();
        }


        private void Start()
        {
            switch (_type)
            {
                case EToggleType.Haptic:
                    _isOn = VibrationManager.I.Status;
                    break;
                case EToggleType.SFX:
                    _isOn = SoundManager.I.SFXStatus;
                    break;
                case EToggleType.BGM:
                    _isOn = SoundManager.I.MusicStatus;
                    break;
            }

            UpdateUI();
        }

        private void OnDisable() => _button.onClick.RemoveListener(OnButtonClicked);

        private void UpdateUI() => _img.sprite = _isOn ? _spriteOnOff[0] : _spriteOnOff[1];

        private void OnButtonClicked()
        {
            _isOn = !_isOn;
            UpdateUI();
            GameSound.I.PlaySFXButtonClick();
            switch (_type)
            {
                case EToggleType.Haptic:
                    VibrationManager.I.Status = _isOn;
                    break;
                case EToggleType.SFX:
                    SoundManager.I.SFXStatus = _isOn;
                    break;
                case EToggleType.BGM:
                    SoundManager.I.MusicStatus = _isOn;
                    break;
            }
            VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
        }
    }

    public enum EToggleType
    {
        Haptic,
        SFX,
        BGM
    }
}