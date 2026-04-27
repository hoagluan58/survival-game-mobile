using Animancer;
using DG.Tweening;
using NFramework;
using Sirenix.Utilities;
using SquidGame;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using UnityEngine;

namespace Game11
{
    public class PlayerControl : MonoBehaviour
    {
        [Header("CONFIGS")]
        [SerializeField] private float _speedSwipePos;
        [SerializeField] private float _speedSwipeTime;

        [SerializeField] private float _speedFlyUp;
        [SerializeField] private float _speedFlyDown;

        [SerializeField] private float _disPer100m;

        [SerializeField] private Vector2 _posClimbFly;

        [Header("Refs")]
        [SerializeField] private StickTarget _stickTarget;
        [SerializeField] private GameObject[] _sticksModels;

        [Header("ANIMATION")]
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private ClipTransition _idleClimbClip;
        [SerializeField] private ClipTransition _winClimbClip;
        [SerializeField] private ClipTransition _climbingClip;
        [SerializeField] private ClipTransition[] _winClip;
        [SerializeField] private AudioSource _fallAudioSrc;

        private bool _isActive;
        private bool _isFlyUp;
        private bool _isFlyDown;
        private bool _isStopFly;
        private bool _isFall;

        private bool _isStartCheckSwipe;

        private Vector3 _posTouchOld;
        private float _timeTouchOld;
        private Game11Control _controller;

        public void Init(Game11Control controller)
        {
            _controller = controller;
            _animancer.Play(_idleClimbClip);
        }

        public void SetActive(bool b)
        {
            _isActive = b;
            _animancer.Play(_idleClimbClip);
        }

        private bool IsFly() => _isFlyUp || _isFlyDown;

        private void Update()
        {
            if (_isFall)
            {
                transform.Translate(Vector3.down * _speedFlyDown * Time.deltaTime);
            }

            if (!_isActive) return;

            if (IsFly() == false)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _posTouchOld = Input.mousePosition;
                    _timeTouchOld = Time.time;

                    _isStartCheckSwipe = true;
                }
                else if (Input.GetMouseButtonUp(0) && _isStartCheckSwipe)
                {
                    Vector2 deltaTouch = Input.mousePosition - _posTouchOld;
                    float deltaTime = Time.time - _timeTouchOld;

                    float value = -_speedSwipePos * deltaTouch.y * _speedSwipeTime / deltaTime * Time.deltaTime;
                    if (value > 0f)
                        Fly(value);
                }
            }

            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!_stickTarget.IsCanStick())
                    {
                        _controller.InvokeOnShowWarning(true);
                        GameSound.I.PlaySFX(Define.SoundPath.SFX_MG11_PICK_HITTING_ROCK);
                        CameraManager.I.ShakeCamera(0.25f, 0.25f);
                        DOTween.Kill(transform);
                        _isStopFly = true;
                        _isFlyUp = false;
                        _isFlyDown = true;

                        _animancer.Play(_climbingClip);
                        transform.DOLocalMoveZ(_posClimbFly.x, 0.25f)
                            .OnComplete(() =>
                            {
                                _animancer.Play(_idleClimbClip);
                                VibrationManager.I.Haptic(VibrationManager.EHapticType.Warning);
                                _isStopFly = false;
                            });
                    }
                    else
                    {
                        CameraManager.I.ShakeCamera(0.25f, 0.25f);
                        DOTween.Kill(transform);
                        _isFlyUp = false;
                        _isFlyDown = false;
                        _isStartCheckSwipe = false;
                        _stickTarget.SetActive(false);

                        _animancer.Play(_climbingClip);
                        transform.DOLocalMoveZ(_posClimbFly.x, 0.25f)
                            .OnComplete(() =>
                            {
                                VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
                                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG11_CLING);
                                _animancer.Play(_idleClimbClip);
                            });
                    }
                }
            }

            if (_isFlyDown && !_isStopFly)
            {
                transform.Translate(Vector3.down * _speedFlyDown * Time.deltaTime);
                if (!_fallAudioSrc.isPlaying)
                {
                    _fallAudioSrc = GameSound.I.PlaySFX(Define.SoundPath.SFX_MG04_FALL);
                }
            }

            if (transform.position.y <= 0f)
            {
                _stickTarget.SetActive(false);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG10_FALL);
                GameManager.I.Lose();
            }
        }


        private void Fly(float val)
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG11_JUMP);
            val = Mathf.Clamp(val, 0f, _disPer100m);
            _isFlyUp = true;
            Vector3 wantedPos = transform.position + Vector3.up * val;
            wantedPos.z = _posClimbFly.y;
            float timeMove = val / _speedFlyUp;
            Tweener t = transform.DOMove(wantedPos, timeMove);
            t.SetDelay(0.25f);
            t.OnComplete(() =>
            {
                _isFlyUp = false;
                _isFlyDown = true;
            });

            _stickTarget.SetActive(true);
        }

        public void Fall()
        {
            _isActive = false;
            _isFall = true;
            Vector3 wantedPos = transform.position;
            wantedPos.z = _posClimbFly.y;
            transform.DOMove(wantedPos, 0.25f);
        }

        private void RollToWin(Vector3 endTriggerPosition)
        {
            _isActive = false;
            transform.DOKill();
            _stickTarget.SetActive(false);
            _sticksModels.ForEach(model => model.SetActive(false));

            _controller.StopTimer();

            // Play win animation
            var endPosition = _controller.Level.PosRoll.position;
            var midPosition = endPosition + new Vector3(0, 2, -1.5f);
            _animancer.Play(_winClimbClip);
            transform.DOPath(new[] { midPosition, endPosition }, 1f, PathType.CatmullRom)
                .OnComplete(() =>
                {
                    transform.DORotate(new Vector3(0, 180f, 0f), 0.5f)
                        .OnComplete(() =>
                        {
                            _controller.Win();
                            _animancer.Play(_winClip.RandomItem());
                        });
                });
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isActive) return;

            if (other.gameObject.CompareTag(Tag.Check_Point))
            {
                RollToWin(other.transform.position);
            }
        }
    }
}