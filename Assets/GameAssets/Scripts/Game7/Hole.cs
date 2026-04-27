using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game7
{
    public class Hole : MonoBehaviour
    {
        [SerializeField] private float _timeHide;
        [SerializeField] private Transform _character;
        [SerializeField] private Transform _leftDoor;
        [SerializeField] private Transform _rightDoor;
        [SerializeField] private Transform _fill;
        [SerializeField] private Transform _circle;

        private float _timeInterval;
        private bool _isOpen;
        private bool _isActive;

        public bool IsOpen { get { return _isOpen; } }

        public void Init(float timeInterval)
        {
            _timeInterval = timeInterval;
        }

        public void Active(float minTime)
        {
            _isActive = true;
            Invoke(nameof(Open), _timeInterval - minTime);
        }

        public void DeActive()
        {
            _isActive = false;
        }

        public void Open()
        {
            if (_isOpen || !_isActive) return;
            _isOpen = true;

            StartCoroutine(IE_Open());
        }

        private IEnumerator IE_Open()
        {
            _leftDoor.DOLocalMoveX(-2f, 0.5f);
            _rightDoor.DOLocalMoveX(2f, 0.5f);
            _character.DOLocalMoveY(-3f, 0.5f);

            _fill.localScale = Vector3.one;
            _fill.DOScaleX(0f, _timeHide);
            yield return new WaitForSeconds(_timeHide);
            Close();
        }

        public void Close()
        {
            if (_isOpen == false) return;
            _isOpen = false;

            _leftDoor.DOLocalMoveX(0f, 0.5f);
            _rightDoor.DOLocalMoveX(0f, 0.5f);
            _character.DOLocalMoveY(-5.5f, 0.5f);

            DOTween.Kill(_fill);
            _fill.localScale = new Vector3(0f, 1f, 1f);

            Invoke(nameof(Open), _timeInterval);
        }

        public void Hit()
        {
            _circle.DOScale(Vector3.one * 2f, 0.5f)
            .OnComplete(() =>
            {
                _circle.localScale = Vector3.zero;
            });
            Close();
        }
    }
}