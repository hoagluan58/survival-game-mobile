using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoMoveUI : MonoBehaviour
{
    [SerializeField] private Vector3 _posFrom;
    [SerializeField] private Vector3 _posTo;
    [SerializeField] private float _timeMove;

    private void OnEnable()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition3D = _posFrom;
        Tweener t = rect.DOAnchorPos3D(_posTo, _timeMove);
        t.SetEase(Ease.Linear);
        t.SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        DOTween.Kill(transform);
    }
}
