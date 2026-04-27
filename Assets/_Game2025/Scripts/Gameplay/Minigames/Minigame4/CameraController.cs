using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.Minigame4
{
    public class CameraController : MonoBehaviour
    {
        [Header("REF")]
        [SerializeField] private CameraConfig _cameraConfig;
        [SerializeField] private Camera _camera;

        private Transform _cameraTransform => _camera.transform;

        public void PlayIntro(float delay, float moveDuration, UnityAction onCompletedIntroCallback)
        {
            _cameraTransform.DOKill();
            _cameraTransform.position = _cameraConfig.Default.Position;
            _cameraTransform.rotation = Quaternion.Euler(_cameraConfig.Default.Rotation);
            _cameraTransform.DOLocalRotate(_cameraConfig.PlayGame.Rotation, moveDuration).SetDelay(delay);
            _cameraTransform.DOMove(_cameraConfig.PlayGame.Position, moveDuration).SetDelay(delay).OnComplete(() =>
            {
                onCompletedIntroCallback?.Invoke();
            });
        }

        public void FocusPlayer(float delay, float duration, UnityAction onCompletedIntroCallback)
        {
            _cameraTransform.DOKill();
            _cameraTransform.DOLocalRotate(_cameraConfig.PlayerCameraData.Rotation, duration).SetDelay(delay);
            _cameraTransform.DOMove(_cameraConfig.PlayerCameraData.Position, duration).SetDelay(delay).OnComplete(() =>
            {
                onCompletedIntroCallback?.Invoke();
            });
        }

    }
}
