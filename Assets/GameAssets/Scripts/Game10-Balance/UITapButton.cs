using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game10
{
    public class UITapButton : MonoBehaviour
    {
        [SerializeField] private GameObject _blurObject;

        private TypeBool _isHighLight = TypeBool.None;

        public void HightlightTap()
        {
            if (_isHighLight == TypeBool.True) return;
            _isHighLight = TypeBool.True;

            transform.localScale = Vector3.one;
            _blurObject.SetActive(false);
        }

        public void UnHightlightTap()
        {
            if (_isHighLight == TypeBool.False) return;
            _isHighLight = TypeBool.False;

            transform.localScale = Vector3.one * 0.85f;
            _blurObject.SetActive(true);
        }

        public void ShowTap()
        {
            DOTween.Kill(transform);
            Vector3 currentScale = Vector3.one;
            if (_isHighLight == TypeBool.False) 
                currentScale = Vector3.one * 0.85f;
            transform.localScale = currentScale;
            Tweener t = transform.DOScale(currentScale * 1.15f, 0.1f);
            t.SetLoops(2, LoopType.Yoyo);
        }
    }

    public enum TypeBool
    {
        None,
        True,
        False
    }
}