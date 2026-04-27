using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIScaleButton : MonoBehaviour
{
    [SerializeField] private Vector3 _valueScale = new Vector3(1.05f, 1.05f, 1.05f);
    [SerializeField] private float _time = 1f;
    [SerializeField] private Ease _ease = Ease.Linear;

    private void Start()
    {
        Tweener t1 = transform.DOScale(_valueScale, _time);
        t1.SetEase(_ease);
        t1.SetLoops(-1, LoopType.Yoyo);
    }


    private void OnDisable()
    {
        DOTween.Kill(transform);
    }
}
