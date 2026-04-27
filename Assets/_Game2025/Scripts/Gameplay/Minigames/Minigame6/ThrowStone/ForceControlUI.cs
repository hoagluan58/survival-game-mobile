using DG.Tweening;
using Redcode.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame6.ThrowStoneGame
{
    public class ForceControlUI : MonoBehaviour
    {
        public enum EState
        {
            None,
            Direction,
            Force
        }

        [SerializeField] private Transform _arrowPivot;

        [Header("DIRECTION")]
        [SerializeField] private float _speed;

        [Header("FORCE")]
        [SerializeField] private Image _forceBar;

        private float _normalizeDirection;
        private float _forceValue;
        private EState _state;
        private ThrowStoneConfigSO _config;
        private IThrowHandler _throwHandler;

        public EState State => _state;

        public void OnEnter(IThrowHandler throwHandler, ThrowStoneConfigSO config)
        {
            _throwHandler = throwHandler;
            _config = config;
            _state = EState.None;
        }

        public void StartDirection()
        {
            _state = EState.Direction;
            SetActive(true);
            _arrowPivot.SetEulerAnglesZ(_config.MaxAngle);
            _arrowPivot?.DOKill();
            _arrowPivot.DORotate(new Vector3(0, 0, _config.MinAngle), _speed)
                       .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }


        public void LockDirection()
        {
            _arrowPivot?.DOKill();
            var zAngle = _arrowPivot.eulerAngles.z > 180f ? _arrowPivot.eulerAngles.z - 360f : _arrowPivot.eulerAngles.z;
            _normalizeDirection = NormalizeRotation(zAngle, _config.MaxAngle, _config.MinAngle, -1, 1);
            StartForce();
        }

        public void StartForce()
        {
            _state = EState.Force;
            _forceBar.fillAmount = 0;
            _forceBar?.DOKill();
            _forceBar.DOFillAmount(1, _speed)
                     .SetEase(Ease.Linear)
                     .SetLoops(-1, LoopType.Yoyo);
        }

        public void LockForce()
        {
            _forceBar?.DOKill();
            _forceValue = _forceBar.fillAmount;
            Throw();
        }

        public void Throw()
        {
            _throwHandler.Throw(_normalizeDirection, _forceValue);
            _forceBar.fillAmount = 0;
            SetActive(false);
        }

        private void SetActive(bool value) => gameObject.SetActive(value);

        private float NormalizeRotation(float input, float minInput, float maxInput, float minOutput, float maxOutput)
            => Mathf.Lerp(minOutput, maxOutput, (input - minInput) / (maxInput - minInput));
    }

    public interface IThrowHandler
    {
        void Throw(float normalizeDirection, float normalizeForce);
    }
}
