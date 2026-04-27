using Cinemachine;
using RotaryHeart.Lib.SerializableDictionary;
using Sirenix.Utilities;
using System.Collections;
using UnityEngine;

namespace SquidGame.LandScape.Minigame16
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        [SerializeField] private SerializableDictionaryBase<ECameraType, CinemachineVirtualCamera> _virtualCameras;
        [SerializeField] private ECameraType _defaultCam;

        private ECameraType _curCameraType;
        private bool _isBlending;
        private Coroutine _crTrackingBlend;

        private void OnEnable() => _cinemachineBrain.m_CameraActivatedEvent.AddListener(OnCameraActivated);

        private void OnDisable() => _cinemachineBrain.m_CameraActivatedEvent.RemoveListener(OnCameraActivated);

        public void Init()
        {
            _virtualCameras.Values.ForEach(cam => cam.enabled = false);
            _virtualCameras[_defaultCam].enabled = true;
            _curCameraType = _defaultCam;
        }

        public IEnumerator SwitchCamera(ECameraType type)
        {
            if (_curCameraType == type) yield break;

            _virtualCameras.Values.ForEach(cam => cam.enabled = false);
            _virtualCameras[type].enabled = true;
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
            Idle,
            Playing
        }
    }
}
