using Cinemachine;
using DG.Tweening;
using Redcode.Extensions;
using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using UnityEngine;

namespace SquidGame.Minigame03
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _disLook;
        [SerializeField] private MyPath _pathCam;

        [Header("NEW")]
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

        public IEnumerator CRSwitchCamera(ECameraType type, float blendTime = 2f)
        {
            if (_curCameraType == type) yield break;

            _cinemachineBrain.m_DefaultBlend.m_Time = blendTime;
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

        public void StartCamPath(float time, float time2, System.Action callBack)
        {
            Vector3[] paths = _pathCam.GetPaths();

            var curVirtualCam = _virtualCameras[_curCameraType];
            curVirtualCam.transform.position = paths[0];

            Tween t = curVirtualCam.transform.DOPath(paths, time);
            t.SetEase(Ease.Linear);
            t.OnComplete(() =>
            {
                callBack?.Invoke();
            });
        }

        public void SetPos(float x, float y, float z) => _virtualCameras[ECameraType.Move].transform.position = new Vector3(x, y, z);

        public enum ECameraType
        {
            Init,
            Choose,
            Follow,
            Path,
            Win,
            Move,
        }

    }
}