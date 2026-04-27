using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.MinigameMarblesVer2
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _cam;
        [SerializeField] private CameraPoint _introPoint;
        [SerializeField] private CameraPoint _playPoint;
        [SerializeField] private CameraPoint _winPoint;
        [SerializeField] private ParticleSystem _winFx;
        public Camera GetMainCamera() => _cam;


        [Button]
        public void IntroPoint()
        {
            _cam.transform.position = _introPoint.transform.position;
            _cam.transform.eulerAngles = _introPoint.transform.eulerAngles;
        }
        [Button]
        public void PlayPoint()
        {
            _cam.transform.position = _playPoint.transform.position;
            _cam.transform.eulerAngles = _playPoint.transform.eulerAngles;
        }
        [Button]
        public void WinPoint()
        {
            _cam.transform.position = _winPoint.transform.position;
            _cam.transform.eulerAngles = _winPoint.transform.eulerAngles;
        }


        public void Init()
        {
            _introPoint.Init(_cam.transform);
            _playPoint.Init(_cam.transform);  
            _winPoint.Init(_cam.transform);
        }

        public void PlayIntro(UnityAction onCompletedIntro)
        {
            _playPoint.Play(3,DG.Tweening.Ease.OutBack, onCompleted : onCompletedIntro);
        }

        public void OnWin(float duration, UnityAction onAnimationCompleted)
        {
            _winPoint.Play(duration, onCompleted: onAnimationCompleted);
        }

        public void DropConfettiFx()
        {
            _winFx.Play();
        }
    }
}
