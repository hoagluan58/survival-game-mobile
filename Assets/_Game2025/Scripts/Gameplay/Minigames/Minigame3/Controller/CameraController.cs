using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        [SerializeField] private CinemachineFreeLook _cinemachineFreeLook;
        [SerializeField] private SerializableDictionary<VirtualCamType, CinemachineVirtualCamera> _cameraDic = new SerializableDictionary<VirtualCamType, CinemachineVirtualCamera>();

        private Vector3[] _initPos;
        public CinemachineBrain CinemachineBrain => _cinemachineBrain;
        public CinemachineFreeLook CinemachineFreeLook => _cinemachineFreeLook;

        private MinigameController _minigameController;

        public void Init(MinigameController minigameController)
        {
            _minigameController = minigameController;
            _initPos = new Vector3[_cameraDic.Count];
            foreach (var pair in _cameraDic)
            {
                _initPos[(int)pair.Key] = pair.Value.transform.position;
            }
        }

        public void UseFreeLook()
        {
            _cinemachineFreeLook.gameObject.SetActive(true);
            foreach (var cam in _cameraDic)
            {
                cam.Value.gameObject.SetActive(false);
            }
        }

        public void ChangeVirtualCam(VirtualCamType virtualCamType)
        {
            _cinemachineFreeLook.gameObject.SetActive(false);
            _cinemachineBrain.m_DefaultBlend.m_Time = 0;
            foreach (var cam in _cameraDic)
            {
                cam.Value.gameObject.SetActive(virtualCamType == cam.Key);
            }
        }

        public IEnumerator CRChangeVirtualCam(VirtualCamType virtualCamType, float blendTime = 0)
        {
            _cinemachineFreeLook.gameObject.SetActive(false);
            _cinemachineBrain.m_DefaultBlend.m_Time = blendTime;
            foreach (var cam in _cameraDic)
            {
                cam.Value.gameObject.SetActive(virtualCamType == cam.Key);
            }
            yield return new WaitForSeconds(blendTime);
        }

        public Transform GetCameraTransform(VirtualCamType virtualCamType)
        {
            return _cameraDic[virtualCamType].transform;
        }

        public void ResetAllVirtualCamPosition()
        {
            foreach (var pair in _cameraDic)
            {
                pair.Value.transform.position = _initPos[(int)pair.Key];
            }
        }

    }
    public enum VirtualCamType
    {
        None = -1,
        DalgonaCut = 0,
        DalgonaBreak = 1,
        Result = 2,
    }
}
