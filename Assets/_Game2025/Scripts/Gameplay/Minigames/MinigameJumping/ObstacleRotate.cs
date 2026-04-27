using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class ObstacleRotate : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private GameObject _botDetectJumpObject;
        [SerializeField] private float _rotateSpeed = 50f;
        [SerializeField] private float _speedUpDuration = 2.5f;
        [SerializeField] private float _intervalSpeedUp = 5f;
        [SerializeField] private float _intervalSwitchDirection = 10f;

        private float _currentRotateSpeed = 0;
        private int _direction = 1;

        private Coroutine _rotateCoroutine;
        private Tween _tween;

        void Start()
        {
            _collider.isTrigger = false;
        }

        void FixedUpdate()
        {
            transform.Rotate(Vector3.up, _currentRotateSpeed * _direction * Time.fixedDeltaTime);
        }

        private void StartRotation()
        {
            _collider.isTrigger = true;
            _tween = DOTween.To(() => _currentRotateSpeed, x => _currentRotateSpeed = x, _rotateSpeed, _speedUpDuration);
        }

        private Tween StopRotation()
        {
            _tween?.Kill();
            _tween = DOTween.To(() => _currentRotateSpeed, x => _currentRotateSpeed = x, 0, 2f);
            return _tween;
        }

        private void IncreaseRotationSpeed()
        {
            _rotateSpeed *= 1.1f;
            _tween = DOTween.To(() => _currentRotateSpeed, x => _currentRotateSpeed = x, _rotateSpeed, _speedUpDuration);
        }

        private void SwitchDirection()
        {
            StopRotation().OnComplete(() =>
            {
                _direction *= -1;
                _tween = DOTween.To(() => _currentRotateSpeed, x => _currentRotateSpeed = x, _rotateSpeed, _speedUpDuration);
            });
        }

        private IEnumerator StartRotationAndWork(float delay = 0f)
        {
            yield return new WaitForSeconds(delay);
            StartRotation();
            while (true)
            {
                yield return new WaitForSeconds(_intervalSpeedUp);
                IncreaseRotationSpeed();
                yield return new WaitForSeconds(_intervalSwitchDirection - _intervalSpeedUp);
                SwitchDirection();
            }
        }

        public void SetActive(bool value, float delay = 0f)
        {
            _botDetectJumpObject.SetActive(value);
            if (value)
            {
                _rotateCoroutine = StartCoroutine(StartRotationAndWork(delay));
            }
            else
            {
                if (_rotateCoroutine != null)
                    StopCoroutine(_rotateCoroutine);

                _collider.isTrigger = false;
                StopRotation();
            }
        }

        public void OnWin()
        {
            _currentRotateSpeed = 0;
            _tween?.Kill();
            if (_rotateCoroutine != null)
                StopCoroutine(_rotateCoroutine);
        }

        public void Init(float intervalSwitchDirectionTime)
        {
            _intervalSwitchDirection = intervalSwitchDirectionTime;
        }
    }
}
