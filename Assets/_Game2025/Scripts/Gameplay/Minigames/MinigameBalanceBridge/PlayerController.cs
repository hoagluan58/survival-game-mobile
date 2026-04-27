using System.Collections;
using Animancer;
using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using UnityEngine;

namespace SquidGame.LandScape.BalanceBridge
{
    public class PlayerController : MonoBehaviour
    {
        [Header("--- Config ---")]
        [SerializeField] private float _speedMove;
        [SerializeField] private float _speedChangeDirTap;
        [SerializeField] private float _speedChangeDirRandom;
        [SerializeField] private float _timeRandomChangeDir;

        [SerializeField] private Vector2 _rangleBridge;

        [Space(8)]
        [Header("--- Animation ---")]
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private AnimationClip _walkAnim;
        [SerializeField] private AnimationClip _walkLeaningAnim;
        [SerializeField] private AnimationClip _fallLeftAnim;
        [SerializeField] private AnimationClip _fallRightAnim;
        // [SerializeField] private ClipTransition[] _winAnims;

        private bool _isActive;
        private float _currentVal;
        private int _dirRandom;
        private BalanceBridgeManager _manager;


        public void Init(BalanceBridgeManager manager)
        {
            _manager = manager;
        }

        public void SetActive(bool value)
        {
            _isActive = value;
            _animator.PlayAnimation(_walkAnim);
            transform.DOKill();
            transform.SetLocalPosY(0f);
            transform.SetEulerAngleY(0);
            transform.SetLocalPosX(0);
            _currentVal = 0.5f;
            if (value)
                StartCoroutine(IE_RandomDir());
        }

        private IEnumerator IE_RandomDir()
        {
            while (_isActive)
            {
                _dirRandom = (int)Mathf.Sign(Random.Range(-1f, 1f));
                yield return new WaitForSeconds(_timeRandomChangeDir);
            }
        }

        private void Update()
        {
            if (!_isActive) return;

            transform.Translate(Vector3.forward * Time.deltaTime * _speedMove);
            if (transform.position.z >= _rangleBridge.x)
            {
                _currentVal += _dirRandom * Time.deltaTime * _speedChangeDirRandom;

                if (Input.GetMouseButtonDown(0))
                {
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_BUTTON_CLICK);

                    if (Input.mousePosition.x / Screen.width >= 0.5f)
                    {
                        _currentVal += Time.deltaTime * _speedChangeDirTap;
                        _manager.InvokeOnShowTapRight();
                    }
                    else
                    {
                        _currentVal -= Time.deltaTime * _speedChangeDirTap;
                        _manager.InvokeOnShowTapLeft();
                    }
                }
            }

            if (_currentVal >= 0.5f)
            {
                _manager.InvokeOnTapLeftHighlight(true);
                _manager.InvokeOnTapRightHighlight(false);
            }
            else
            {
                _manager.InvokeOnTapLeftHighlight(false);
                _manager.InvokeOnTapRightHighlight(true);
            }

            _manager.InvokeOnProgressChanged(_currentVal);

            if (_currentVal >= 0.6f || _currentVal <= 0.4f)
            {
                _animator.PlayAnimation(_walkLeaningAnim);
                VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
            }

            if (_currentVal >= 0.9f || _currentVal <= 0.1f)
            {
                Fall();
            }

            if (transform.position.z >= _rangleBridge.y)
                Win();
        }


        private void Fall()
        {
            _isActive = false;
            bool fallRight = _currentVal >= 0.5f;
            transform.SetPosX(transform.position.x + (fallRight ? 0.6f : -0.6f));
            transform.SetEulerAngleY(fallRight ? -90 : 90);
            _animator.PlayAnimation(fallRight ? _fallRightAnim : _fallLeftAnim, 0.2f);

            transform.DOMoveX(transform.position.x + (fallRight ? 1f : -1f), 0.5f).SetDelay(0.6f)
                .OnComplete(() => GameSound.I.PlaySFX(Define.SoundPath.SFX_BALANCE_BRIDGE_FALL));
            transform.DOMoveY(transform.position.y - 4f, 0.7f).SetDelay(0.8f);
            _manager.Lose();
        }

        private void Win()
        {
            _isActive = false;
            _animator.PlayAnimation(_walkAnim);
            Vector3 wantedPos = transform.position + Vector3.forward;
            Tween t = transform.DOMove(wantedPos, 1f);
            t.OnComplete(() =>
            {
                _animator.PlayAnimation(EAnimStyle.Victory_1, EAnimStyle.Victory_2, EAnimStyle.Victory_3);
                transform.DORotate(new Vector3(0f, 180f, 0f), 0.5f);
                _manager.Win();
            });
        }
    }
}