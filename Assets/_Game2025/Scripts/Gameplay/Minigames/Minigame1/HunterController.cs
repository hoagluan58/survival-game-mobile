using DG.Tweening;
using NFramework;
using SquidGame.Core;
using SquidGame.LandScape.Minigame4;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.Minigame1
{
    public class HunterController : MonoBehaviour
    {
        public static event Action<bool> OnEnemySinging;
        public static event Action<bool> OnWarning;
        private UnityAction<bool> _onAlertTrigger;

        [SerializeField] protected List<Guard> _guards;
        [SerializeField] private Guard _playerGuard;

        [Header("CONFIG")]
        [SerializeField] protected float _startPitch = 1f;
        [SerializeField] protected Transform _headTf;
        public bool IsSilent => _isSilent;
        public bool IsRotateToBot => _isRotateToBot;

        protected bool _isSilent;
        protected float _curPitch = 1;

        protected bool _isStopGame;
        protected bool _isRotateToBot;
        protected PlayerController _playerController;
        protected MinigameController _controller;
        protected BotManager _botManager;

        private bool _isAlertOn = false;
        private bool _isFixedPitch;
        private AudioSource _searchingSource;
        private AudioSource _singingSource;
        private AudioSource _timeLeftSource;
        private Tween _singingTweener;
        private Tween _scanningTweener;


        


        public void Init(MinigameController controller, BotManager botManager)
        {
            _controller = controller;
            _botManager = botManager;
            _playerController = _controller.PlayerController;

            _curPitch = 1;
            _headTf.localEulerAngles = Vector3.zero;
        }


        private void OnDestroy()
        {
            OnEnemySinging = null;
            OnWarning = null;
            _onAlertTrigger = null;
            _singingTweener.Kill();
            _scanningTweener.Kill();
            PlaySearchSound(false);
            PlaySingSound(false, 1);
        }

        public void PlaySingSound(bool isPlay, float soundPitch)
        {
            if (isPlay)
            {
                if (_singingSource == null ) _singingSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_SING, pitch: soundPitch);
            }
            else
            {
                if (_singingSource != null)
                {
                    _singingSource.Stop();
                    _singingSource = null;
                }
            }
        }

        public void PlaySearchSound(bool isPlay)
        {
            if (isPlay)
            {
                if (_searchingSource == null ) _searchingSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_SEARCHING);
            }
            else
            {
                if (_searchingSource != null)
                {
                    _searchingSource.Stop();
                    _searchingSource = null;
                }
            }
        }

        public void Rotate()
        {
            _headTf.DOKill();
            _headTf.DOLocalRotate(new Vector3(0, -180f, 0), 1f);
        }

        public void StartGame()
        {
            //StartCoroutine(CRSingBehaviour());
            StartCheck();
        }


        private void StartCheck()
        {
            _isSilent = false;
            _isRotateToBot = false;
            _singingTweener.Kill();
            _scanningTweener.Kill();
            PlaySingSound(false, 1);
            PlaySearchSound(false);

            OnEnemySinging?.Invoke(true);
            _curPitch = _isFixedPitch ? _startPitch : _startPitch + UnityEngine.Random.Range(-0.2f, 0.5f);
            PlaySingSound(true, _curPitch);
            var timeSinging = 3.1f / _curPitch;

            _singingTweener = DOVirtual.DelayedCall(timeSinging, OnSingingCompleted).OnUpdate(() => {
                timeSinging -= Time.deltaTime;
                CheckAlert(timeSinging);
            });
        }


        private void OnSingingCompleted()
        {
           
            _headTf.DOLocalRotate(Vector3.zero, 0.25f).OnComplete(() => {

                VibrationManager.I.Haptic(VibrationManager.EHapticType.Warning);
                _isSilent = true;
                _isRotateToBot = true;
                OnEnemySinging?.Invoke(false);

                if (!_isStopGame)
                {
                    OnWarning?.Invoke(true);
                }

                PlaySearchSound(true); 
                var randomBot = UnityEngine.Random.Range(1, 4);
                var bots = _botManager.PickRandomeAliveBots(randomBot);
                var enemies = _guards.PickRandom(bots.Count);
                ShootLaser(bots, enemies);

               _scanningTweener = DOVirtual.DelayedCall(2, () => {

                   _isRotateToBot = false;
                   _headTf.DOLocalRotate(new Vector3(0, -180, 0), 0.25f).OnComplete(() => {
                       _isAlertOn = false;
                       _onAlertTrigger?.Invoke(false);
                       StartCheck();
                   });
               }); 
            });
        }



        public virtual IEnumerator CRSingBehaviour()
        {
            while (_playerController.IsPlaying)
            {
                _isSilent = false;
                _isRotateToBot = false;

                OnEnemySinging?.Invoke(true);
                _curPitch = _isFixedPitch ? _startPitch : _startPitch + UnityEngine.Random.Range(-0.2f, 0.5f);
                _singingSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_SING, pitch: _curPitch);
                // Time singing
                var timeSinging = 3.1f / _curPitch;

                while (timeSinging > 0f)
                {
                    timeSinging -= Time.deltaTime;
                    CheckAlert(timeSinging);
                    yield return null;
                }
                yield return _headTf.DOLocalRotate(Vector3.zero, 0.25f).WaitForCompletion();
                yield return CRScanning();
                yield return _headTf.DOLocalRotate(new Vector3(0, -180, 0), 0.25f).WaitForCompletion();
                _isAlertOn = false;
                _onAlertTrigger?.Invoke(false);
            }
        }



        private void CheckAlert(float time)
        {
            if (time < 1f && !_isAlertOn)
            {
                _isAlertOn = true;
                _onAlertTrigger?.Invoke(true);
            }
        }


        public void SetActive(bool isActive)
        {
            _isStopGame = !isActive;
            OnWarning?.Invoke(_isStopGame);

            if (_isStopGame)
            {
                _isSilent = false;
            }
        }


        private IEnumerator CRScanning()
        {
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Warning);
            _isSilent = true;
            _isRotateToBot = true;
            OnEnemySinging?.Invoke(false);

            if (!_isStopGame)
            {
                OnWarning?.Invoke(true);
            }

            yield return CRKillBots();
        }


        public virtual IEnumerator CRKillBots()
        {
            _searchingSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_SEARCHING);
            var waiter = new WaitUntil(() => _isRotateToBot);
            yield return waiter;

            var randomBot = UnityEngine.Random.Range(1, 4);
            var bots = _botManager.PickRandomeAliveBots(randomBot);
            var enemies = _guards.PickRandom(bots.Count);
            ShootLaser(bots, enemies);
            yield return new WaitForSeconds(2f);
            _isRotateToBot = false;
        }


        private void ShootLaser(List<Bot> bots, List<Guard> guards)
        {
            for (int i = 0; i < bots.Count; i++)
            {
                var bot = bots[i];
                var guard = guards[i];
                var baseGuard = guard.LookAtTarget(bot.HeadPos).GetBaseGuard();
                baseGuard.PlayShootAnim().OnShootCompleted(() =>
                {
                    guard.transform.localEulerAngles = new Vector3(0, -45, 0);
                });
                bot.HandleDie();
                baseGuard.ShowLine(0, bot.HeadPos).ClearLine(0.2f);
            }
        }


        public Guard GetFreeEnemy()
        {
            return _playerGuard;
        }

        public void SetAlertTrigger(UnityAction<bool> alertCallback)
        {
            _onAlertTrigger = alertCallback;
        }

        public void SetFixedPitch(bool isFixedPitch)
        {
            _isFixedPitch = isFixedPitch;
        }


        public void Stop()
        {
            _headTf.DOKill();
            _headTf.DOLocalRotate(new Vector3(0, -180, 0),0.25f);
            _singingTweener.Kill();
            _scanningTweener.Kill();
            PlaySearchSound(false);
            PlaySingSound(false,1);
        }


        public void OnWin()
        {
            _headTf.DOKill();
            _headTf.DOLocalRotate(new Vector3(0, -180, 0), 0.25f);
            _singingTweener.Kill();
            _scanningTweener.Kill();
            PlaySearchSound(false);
            PlaySingSound(false,1);
        }
    }
}