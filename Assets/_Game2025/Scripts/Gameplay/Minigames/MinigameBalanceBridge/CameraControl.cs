using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NFramework;

namespace SquidGame.LandScape.BalanceBridge
{
    public class CameraControl : SingletonMono<CameraControl>
    {
        [SerializeField] GameObject _freeLookObject, _playCameraObject;

        public void ActivePlayCamera(bool value)
        {
            _freeLookObject.SetActive(!value);
            _playCameraObject.SetActive(value);
        }
    }
}