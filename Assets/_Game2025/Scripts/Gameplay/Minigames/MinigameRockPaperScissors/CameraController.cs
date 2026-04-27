using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.MinigameRockPaperScissors
{
    public enum CameraType
    {
        Intro = 0 ,
        Playing = 1 ,
        Win = 2
    }

    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _cam;
        [SerializeField] private CameraPoint _introPoint;
        [SerializeField] private CameraPoint _playPoint;
        [SerializeField] private CameraPoint _winPoint;
        [SerializeField] private ParticleSystem _winFx;

        private Dictionary<CameraType, CameraPoint> _dicCameras;
        public Camera GetMainCamera() => _cam;


        [Button]
        public void IntroPoint()
        {
            _cam.fieldOfView = _introPoint.FieldOfView;
            _cam.transform.position = _introPoint.transform.position;
            _cam.transform.eulerAngles = _introPoint.transform.eulerAngles;
        }
        [Button]
        public void PlayPoint()
        {
            _cam.fieldOfView = _playPoint.FieldOfView;
            _cam.transform.position = _playPoint.transform.position;
            _cam.transform.eulerAngles = _playPoint.transform.eulerAngles;
        }
        [Button]
        public void WinPoint()
        {
            _cam.fieldOfView = _winPoint.FieldOfView;
            _cam.transform.position = _winPoint.transform.position;
            _cam.transform.eulerAngles = _winPoint.transform.eulerAngles;
        }


        public void Init()
        {
            _dicCameras = new Dictionary<CameraType, CameraPoint>();
            _dicCameras.Add(CameraType.Intro, _introPoint);
            _dicCameras.Add(CameraType.Playing, _playPoint);
            _dicCameras.Add(CameraType.Win, _winPoint);

            _introPoint.Init(_cam);
            _playPoint.Init(_cam);
            _winPoint.Init(_cam);
        }

        public IEnumerator SwitchCamera(CameraType type, float duration)
        {
            _dicCameras[type].Play(duration);
            yield return new WaitForSeconds(duration);
        }

        public void OnWin(float duration)
        {
            _winPoint.Play(duration);
            _winFx.Play();
        }
    }
}
