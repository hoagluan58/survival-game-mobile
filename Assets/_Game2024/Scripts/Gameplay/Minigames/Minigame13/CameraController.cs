using Cinemachine;
using Redcode.Extensions;
using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using UnityEngine;

namespace SquidGame.Minigame13
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        [SerializeField] private SerializableDictionaryBase<ECameraType, CinemachineVirtualCamera> _virtualCameras;

        private ECameraType _curCameraType;
        private bool _isBlending;
        private Coroutine _crTrackingBlend;

        private void OnEnable() => _cinemachineBrain.m_CameraActivatedEvent.AddListener(OnCameraActivated);

        private void OnDisable() => _cinemachineBrain.m_CameraActivatedEvent.RemoveListener(OnCameraActivated);

        public void Init(ECameraType type)
        {
            _virtualCameras.Values.ForEach(cam => cam.gameObject.SetActive(false));
            _virtualCameras[type].gameObject.SetActive(true);
            _curCameraType = type;
        }

        public void SwitchCamera(ECameraType type)
        {
            StartCoroutine(CRSwitchCamera(type));
        }

        public IEnumerator CRSwitchCamera(ECameraType type)
        {
            if (_curCameraType == type) yield break;

            _virtualCameras.Values.ForEach(cam => cam.gameObject.SetActive(false));
            _virtualCameras[type].gameObject.SetActive(true);
            _isBlending = true;
            _curCameraType = type;
            yield return new WaitUntil(() => !_isBlending);
        }

        private void OnCameraActivated(ICinemachineCamera newCamera, ICinemachineCamera prevCamera)
        {
            if (_crTrackingBlend != null)
            {
                StopCoroutine(_crTrackingBlend);
            }

            _crTrackingBlend = StartCoroutine(WaitForBlendCompletion());

            IEnumerator WaitForBlendCompletion()
            {
                while (_cinemachineBrain.IsBlending) yield return null;

                _crTrackingBlend = null;
                _isBlending = false;
            }
        }

        public enum ECameraType
        {
            Fight,
            Player,
        }
    }
}
