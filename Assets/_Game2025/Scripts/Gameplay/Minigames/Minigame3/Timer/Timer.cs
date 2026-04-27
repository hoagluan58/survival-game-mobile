using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{    
    public class Timer
    {
        [SerializeField] protected float timer = 0;
        [SerializeField] protected float cooldownTime;
        [SerializeField] protected float timerStop;

        [SerializeField] protected bool isStoppingTime;
        [SerializeField] protected bool isStartCountDown;

        public void SetCooldownTime(float cooldownTime)
        {
            this.cooldownTime = cooldownTime;
            StartTimer();
        }
        public void StartTimer()
        {
            timer = Time.time + cooldownTime;
        }
        public bool CheckTimer()
        {
            if (isStoppingTime) return false;
            if (Time.time >= timer) return true;
            return false;
        }
        public void RefreshTimer()
        {
            float timeToRefresh = cooldownTime - (timer - Time.time);
            timer += timeToRefresh;
        }
        public void CancelTimer()
        {
            timer = Time.time;
            isStoppingTime = false;
        }
        public float GetTimeRemain()
        {
            float timeRemain = timer - Time.time;
            return timeRemain;
        }
        public void StopTimer()
        {
            if (isStoppingTime) return;
            if (CheckTimer()) return;
            timerStop = Time.time;
            isStoppingTime = true;
        }
        public bool IsStoppingTime()
        {
            return isStoppingTime;
        }
        public void ContinueTimer()
        {
            float stopTime = Time.time - timerStop;
            timer += stopTime;
            isStoppingTime = false;
        }
        public float GetCooldownTime()
        {
            return cooldownTime;
        }
    }
}
