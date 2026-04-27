using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame3
{
    public class CutDalgonaPanelUI : MonoBehaviour
    {
        private Action _onPointerDown;
        private Action _onPointerHold;
        private Action _onPointerUp;

        [SerializeField] private TextMeshProUGUI _txtTime;
        [SerializeField] private GameObject _goTutorial;
        [SerializeField] private Image _imgWarning;

        private AudioSource _audioSource;

        private bool _isPointing;
        private bool _isTutorial;
        private bool _isVibrating;
        private Coroutine _waitTutorialCoroutine;
        private Tween _scaleLoopTween;

        private void Awake()
        {
            TimeController.OnTimeChanged += OnUpdateTime;
            TimeController.OnTimeOut += OnTimeOut;
            TimeController.OnHold += OnHold;
        }

        private void OnDestroy()
        {
            TimeController.OnTimeChanged -= OnUpdateTime;
            TimeController.OnTimeOut -= OnTimeOut;
            TimeController.OnHold -= OnHold;
        }

        public void StartTutorial()
        {
            _isTutorial = true;
            ActiveTutorial();
        }

        public void StopTutorial()
        {
            _isTutorial = false;
        }

        private void OnUpdateTime(int time)
        {
            _txtTime.text = time.ToString();
        }

        private void OnTimeOut()
        {
            RemoveAllEvent();
            _imgWarning.SetAlpha(0);
        }

        private void OnHold(float percantageTime)
        {
            _imgWarning.SetAlpha(1 - percantageTime);
            if(percantageTime <= 0.3f && !_isVibrating)
            {
                _isVibrating = true;
                StartCoroutine(CRLoopVibrate());
            }
        }       

        public void SetEvent(Action onPointerDown,Action onPointerHold, Action onPointerUp)
        {
            _onPointerDown = onPointerDown;
            _onPointerHold = onPointerHold;
            _onPointerUp = onPointerUp;
        }

        public void RemoveAllEvent()
        {
            _audioSource?.Stop();
            _isVibrating = false;
            _isPointing = false;
            _onPointerDown = null;
            _onPointerHold = null;
            _onPointerUp = null;
            StopTutorial();
            if (_waitTutorialCoroutine != null)
            {
                StopCoroutine(_waitTutorialCoroutine);
                _waitTutorialCoroutine = null;
            }
            _goTutorial.SetActive(false);
        }

        public void OnPointerDown()
        {            
            VibrationManager.I.Haptic(VibrationManager.EHapticType.MediumImpact);
            _isPointing = true;
            StartCoroutine(CRHandleNeedle());
            _audioSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_MG03_DRAW, true);
            _goTutorial.SetActive(false);
            _onPointerDown?.Invoke();
            if(_waitTutorialCoroutine != null)
            {
                StopCoroutine(_waitTutorialCoroutine);
                _waitTutorialCoroutine = null;
            }
        }

        private IEnumerator CRHandleNeedle()
        {
            while (_isPointing)
            {
                _onPointerHold?.Invoke();
                yield return null;
            }
        }


        public void OnPointerUp()
        {
            _isVibrating = false;
            _imgWarning.SetAlpha(0);
            _audioSource?.Stop();
            _isPointing = false;
            _onPointerUp?.Invoke();
            if (!_isTutorial || !gameObject.activeSelf) return;
            _waitTutorialCoroutine = StartCoroutine(CRWaitToShowTutorial());
        }

        private IEnumerator CRLoopVibrate()
        {
            while (_isVibrating)
            {
                VibrationManager.I.Haptic(VibrationManager.EHapticType.MediumImpact);
                yield return null;
            }
        }

        private IEnumerator CRWaitToShowTutorial()
        {
            yield return new WaitForSeconds(5f);
            if(!_isTutorial) yield break;
            ActiveTutorial();
        }

        private void ActiveTutorial()
        {
            _scaleLoopTween?.Kill();
            _goTutorial.transform.localScale = Vector3.zero;
            _goTutorial.SetActive(true);
            _goTutorial.transform.DOScale(1, 0.6f).OnComplete(() =>
            {
                DoScale();
            });
        }

        private void DoScale()
        {
            _scaleLoopTween = _goTutorial.transform.DOScale(0.9f, 0.7f).SetLoops(-1, LoopType.Yoyo);
        }


    }
}
