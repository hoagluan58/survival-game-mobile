using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.Minigame1
{
    public class CameraSystem : MonoBehaviour
    {
        [SerializeField] private GameObject _cameraIntro;
        [SerializeField] private GameObject _cameraPlayer;
        [SerializeField] private Transform _targetIntro;
        

        public void OnInitialized()
        {
            SetActivePlayerCamera(false);
        }


        private void SetActivePlayerCamera(bool value)
        {
            _cameraIntro.gameObject.SetActive(!value);
            _cameraPlayer.gameObject.SetActive(value);
        }


        public void PlayIntro(float duration, UnityAction onCompleted)
        {
            _cameraIntro.transform.DOKill();
            _cameraIntro.transform.DOMove(_targetIntro.transform.position,duration);
            _cameraIntro.transform.DORotate(_targetIntro.transform.eulerAngles,duration).OnComplete(()=> {
                SetActivePlayerCamera(true);
                onCompleted?.Invoke();
            });
        }
    }
}
