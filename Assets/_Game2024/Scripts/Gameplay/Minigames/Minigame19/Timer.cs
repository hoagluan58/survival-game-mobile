using System;
using UnityEngine;

namespace SquidGame.Minigame19
{
    public class Timer : MonoBehaviour
    {
        public event Action OnTimeEnd;
        private float _currentTime;
        private bool _isRunning;

        private Minigame19MenuUI _minigame19MenuUI;
        
        public float CurrentTime => _currentTime;

        public void Init(Minigame19MenuUI minigame19MenuUI)
        {
            _minigame19MenuUI = minigame19MenuUI;
        }

        public void StartTimer(float time)
        {
            _isRunning = true;
            _currentTime = time;
        }

        public void PauseTimer(bool isPause)
        {
            _isRunning = !isPause;
        }

        private void FixedUpdate()
        {
            if (!_isRunning) return;
            if (_currentTime <= 0) return;

            _currentTime -= Time.fixedDeltaTime;
            _minigame19MenuUI.UpdateTimer(_currentTime);
            if (_currentTime <= 0)
            {
                OnTimeEnd?.Invoke();
            }
        }
    }
}