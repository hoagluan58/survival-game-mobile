using Cinemachine;
using Redcode.Extensions;
using SquidGame.LandScape.Minigame4;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.MinigameMingle
{
    public enum CameraType
    {
        Player = 0,
        Intro = 1,
        Spin   = 2 
    }


    public class CameraController : MonoBehaviour
    {
        [Header("References Intro")]
        [SerializeField] private GameObject _introCamera;
        [SerializeField] public GameObject _mingle;
        [SerializeField] private Transform _targetIntro;
        [Header("References Player")]
        [SerializeField] private GameObject _playerCamera;
        public GameObject _winCamera; 
        

        #region PROPERTIES
        public GameObject IntroCamera => _introCamera;
        public Transform TargetIntro => _targetIntro;
        public GameObject PlayerCamera => _playerCamera;
        public CinemachineFreeLook CinemachineFreeLook => _playerCamera.GetComponent<CinemachineFreeLook>();
        #endregion

        private Dictionary<CameraType, StateBaseCamera> _dicCamera;
        private StateBaseCamera _currentStateCamera;

        public void OnInitialized()
        {
            _dicCamera = new Dictionary<CameraType, StateBaseCamera>();
            _dicCamera.Add(CameraType.Intro, new IntroCamera());
            _dicCamera.Add(CameraType.Player, new PlayerCamera());
            _dicCamera.ForEach(x=> x.Value.Init(this));
        }


        public void PlayIntro(UnityAction onCompleted)
        {
            SwitchCamera(CameraType.Intro);
            var introCam = (IntroCamera)_currentStateCamera;
            introCam.AddListernerCompleted(onCompleted);
        }


        public void SwitchCamera(CameraType type)
        {
            if (!_dicCamera.ContainsKey(type))
            {
                Debug.LogError("Dont have key ! : " + type);
                return;
            }
            _currentStateCamera?.Exit();
            _currentStateCamera = _dicCamera[type];
            _currentStateCamera.Enter();
        }

        public void PlayWinCamera()
        {
            _winCamera.SetActive(true);
        }
    }
}
