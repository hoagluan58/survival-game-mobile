using SquidGame.LandScape.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class TimeController : MonoBehaviour
    {
        public static Action<int> OnTimeChanged;
        public static Action<float> OnHold;
        public static Action OnTimeOut;

        [SerializeField] private float _cutDalgonaTime = 20f;
        [SerializeField] private float _holdDalgonaTime = 2f;

        private Dictionary<TimerType, TimerInfor> timerDic = new Dictionary<TimerType, TimerInfor>();

        private MinigameController _controller;
        private AudioSource _audioSource;

        private Timer _countdownTimer = new Timer();
        private Timer _holdTimer = new Timer();

        public void Init(MinigameController minigameController)
        {
            _controller = minigameController;
            OnTimeChanged?.Invoke((int)_cutDalgonaTime);
            timerDic.Add(TimerType.CutStep, new TimerInfor(_countdownTimer, _cutDalgonaTime));
            timerDic.Add(TimerType.HoldNeedle, new TimerInfor(_holdTimer, _holdDalgonaTime));
        }

        public void SetMaxTime()
        {
            OnTimeChanged?.Invoke((int)_cutDalgonaTime);
        }

        public void StartCountDown(TimerType timerType)
        {
            TimerInfor timerInfor = timerDic[timerType];
            timerInfor.StartTimer();
            StopCountDown(timerType);
            timerInfor.SaveCoroutine(StartCoroutine(CRCheckTimer()));

            IEnumerator CRCheckTimer()
            {                
                yield return new WaitUntil(() =>
                {
                    if(timerType == TimerType.CutStep)
                    {
                        if ((int)timerInfor.Timer.GetTimeRemain() <= 5 && _audioSource == null)
                        {
                            _audioSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_MG05_COUNT_DOWN);
                        }
                        OnTimeChanged?.Invoke((int)timerInfor.Timer.GetTimeRemain());
                    }
                    else
                    {
                        OnHold?.Invoke(timerInfor.Timer.GetTimeRemain() / _holdDalgonaTime);
                    }
                    return timerInfor.Timer.CheckTimer();
                });
                if(timerType == TimerType.HoldNeedle)
                {
                    _controller.BreakDalgonaController.BreakWrong();
                }
                OnTimeOut?.Invoke();
                ClearAllCoroutines();
                _audioSource?.Stop();
                _audioSource = null;
                _controller.LoseLevel();
            }
        }

        public void StopCountDown(TimerType timerType)
        {
            _audioSource?.Stop();
            _audioSource = null;
            timerDic[timerType].ClearCoroutine((coroutine) =>
            {
                StopCoroutine(coroutine);
            });
        }

        public void ClearAllCoroutines()
        {
            foreach (var timerInfor in timerDic)
            {
                StopCountDown(timerInfor.Key);
            }
        }
        
    }

    public enum TimerType
    {
        CutStep,
        HoldNeedle
    }

    public class TimerInfor
    {        
        public Timer Timer;
        public float Time;
        public Coroutine CacheCoroutine;

        public TimerInfor(Timer timer, float time)
        {
            Timer = timer;
            Time = time;
        } 

        public void StartTimer()
        {
            Timer.SetCooldownTime(Time);
        }

        public void SaveCoroutine(Coroutine coroutine)
        {
            CacheCoroutine = coroutine;
        }

        public void ClearCoroutine(Action<Coroutine> callback)
        {
            if(CacheCoroutine != null)
            {
                callback?.Invoke(CacheCoroutine);
                CacheCoroutine = null;
            }
        }
    }
}
