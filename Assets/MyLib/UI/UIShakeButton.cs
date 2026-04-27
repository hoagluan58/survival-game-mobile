using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIShakeButton : MonoBehaviour
{
    [SerializeField] private Vector3 _shakingRotation = default;
    [SerializeField] private int _numLoopPerShake = 3;
    [SerializeField] private float _duration = 0.15f;
    [SerializeField] private Vector2 _interval;

    void Start()
    {
        StartCoroutine(IE_Shake());
    }

    private IEnumerator IE_Shake()
    {
        Tweener t = transform.DOShakeRotation(_duration, _shakingRotation);
        t.SetLoops(_numLoopPerShake, LoopType.Yoyo);
        yield return new WaitForSeconds(Random.Range(_interval.x, _interval.y));
        StartCoroutine(IE_Shake());
    }

    private void OnDisable()
    {
        DOTween.Kill(transform);
    }
}
