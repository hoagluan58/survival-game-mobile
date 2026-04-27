using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class LedLightAnimationUI : MonoBehaviour
    {
        [SerializeField] private EAnimType _type;
        [SerializeField] private Image _image;

        // Pulse
        private float _pulseSpeed = 2f;

        // Wave Motion
        private float _hue = 0f;
        private float _speed = 0.5f;

        private void Update()
        {
            switch (_type)
            {
                case EAnimType.Pulse:
                    PlayPulseEffect();
                    break;
                case EAnimType.WaveMotion:
                    PlayWaveMotionEffect();
                    break;
                default:
                    break;
            }
        }

        private void PlayPulseEffect()
        {
            var alpha = Mathf.PingPong(Time.time * _pulseSpeed, 1f);
            _image.color = new Color(1f, 1f, 1f, alpha); // Adjust alpha
        }

        private void PlayWaveMotionEffect()
        {
            _hue += Time.deltaTime * _speed;
            if (_hue > 1) _hue = 0;
            _image.color = Color.HSVToRGB(_hue, 1, 1);
        }

        public enum EAnimType
        {
            Pulse,
            WaveMotion,
        }
    }
}
