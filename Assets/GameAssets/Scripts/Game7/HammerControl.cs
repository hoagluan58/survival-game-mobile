using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game7
{
    public class HammerControl : MonoBehaviour
    {
        [SerializeField] private Vector3 _startRot;
        [SerializeField] private Vector3 _endRot;
        [SerializeField] private float _time;
        [SerializeField] private float _timeHide;

        public void Hit()
        {
            if (gameObject.activeSelf) return;
            gameObject.SetActive(true);
            transform.eulerAngles = _startRot;
            transform.DORotate(_endRot, _time).SetEase(Ease.Linear);
            StartCoroutine(IE_Hide());
        }

        private IEnumerator IE_Hide()
        {
            yield return new WaitForSeconds(_timeHide);
            gameObject.SetActive(false);
        }
    }
}