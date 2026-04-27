using System;
using System.Collections;
using UnityEngine;

namespace SquidGame.LandScape.Utils
{
    public class Timer : MonoBehaviour
    {
        private Action<float> _onTimeChanged;
        private Action _onTimerEnd;
        private float _timeLeft;
        private Coroutine _coroutine;
        private bool _isPaused;

        public float TimeLeft => _timeLeft;

        public void Init(float time, Action<float> onTimeChanged = null, Action onTimerEnd = null)
        {
            _timeLeft = time;
            _onTimeChanged = onTimeChanged;
            _onTimerEnd = onTimerEnd;
        }

        public void StartTimer()
        {
            StopTimer();

            _coroutine = StartCoroutine(CRCountdown());

            IEnumerator CRCountdown()
            {
                var waiter = new WaitForSeconds(1f);

                _onTimeChanged?.Invoke(_timeLeft);
                while (_timeLeft > 0)
                {
                    yield return waiter;
                    if (_isPaused) continue;

                    _timeLeft--;
                    _onTimeChanged?.Invoke(_timeLeft);
                }

                _onTimerEnd?.Invoke();
            }
        }

        public void StopTimer()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        public void PauseTimer(bool isPaused) => _isPaused = isPaused;
    }
}