using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UILightningButton : MonoBehaviour
{
    [SerializeField] private float _posMoveX=250f;
    [SerializeField] private float _timeMove=1f;
    [SerializeField] private Image _imgMove=default;

    private void Start()
    {
        Color color = _imgMove.color;
        color.a = 1f;
        _imgMove.color = color;

        _imgMove.transform.localPosition = Vector3.left * _posMoveX;
        _imgMove.DOFade(0.5f, _timeMove);

        Tweener t= _imgMove.transform.DOLocalMove(Vector3.right * _posMoveX, _timeMove);

        t.OnComplete(() =>
        {
            if (gameObject.activeSelf)
                Start();
        });
    }
}
