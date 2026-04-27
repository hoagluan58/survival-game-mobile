using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NFramework;

public class CameraManager : SingletonMono<CameraManager>
{
    [SerializeField] private Vector3 _valueShake;


    private void OnEnable()
    {
        EventManager.OnGameInitDone += OnGameInitDone;
    }

    private void OnDisable()
    {
        EventManager.OnGameInitDone -= OnGameInitDone;
    }

    private void OnGameInitDone()
    {

    }


    public void ShakeCamera(float time, float delay)
    {
        transform.DOShakePosition(time, _valueShake, 25).SetDelay(delay);
    }
}
