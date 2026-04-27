using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class OutroController : MonoBehaviour
    {
        [SerializeField] private Transform _tfEnd;
        
        private MinigameController _controller;
        public void Init(MinigameController minigameController)
        {
            _controller = minigameController;
        }

        public void DoOutro(float zoomOutTime, float blendCamTime, Action onComplete = null)
        {
            StartCoroutine(CRDoOutro(onComplete));

            IEnumerator CRDoOutro(Action onComplete = null)
            {
                yield return _controller.CameraController.CRChangeVirtualCam(VirtualCamType.Result, blendCamTime);
                onComplete?.Invoke();
                //Transform camTransform = _controller.CameraController.GetCameraTransform(VirtualCamType.Result);
                //camTransform.DOMove(_tfEnd.position, zoomOutTime).OnComplete(() =>
                //{
                //    onComplete?.Invoke();
                //});
            }
        }
    }
}
