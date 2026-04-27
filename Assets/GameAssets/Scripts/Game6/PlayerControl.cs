using Animancer;
using DG.Tweening;
using NFramework;
using SquidGame;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using UnityEngine;

namespace Game6
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private float _startHP = 0.25f;
        [SerializeField] private float _speedHPDown = 0.01f;
        [SerializeField] private SerializableDictionary<TypeItemCollect, float> _configItems;
        [SerializeField] private ParticleSystem _fxHeal;

        [Header("ANNIMATIONS")]
        [SerializeField] private CharacterAnimationController _animator;
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private ClipTransition[] _winAnims;

        private PlayerMovement _playerMovement;
        private PlayerFighting _playerFighting;

        private float _currentHP;
        private bool _isActive;

        private bool _isFightBoss;
        private Game6Control _controller;
        public bool IsFighting => _isFightBoss;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerFighting = GetComponent<PlayerFighting>();
        }

        public void Init(Game6Control controller, VariableJoystick joystick)
        {
            _controller = controller;
            _playerMovement.Init(joystick);
            _playerFighting.Init(_controller);
            _animator.PlayAnimation(EAnimStyle.Idle);
        }

        public void Active()
        {
            _isActive = true;
            _playerMovement.SetActive(true);
            _animator.PlayAnimation(EAnimStyle.Run);
            _currentHP = _startHP;
            _controller.InvokeOnPlayerHPChanged(_currentHP);
        }

        public void StartFight() => _playerFighting.SetActive(true);

        public void TakeEnemyDamage(float dmg)
        {
            _currentHP -= dmg;
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_HIT);
            if (_currentHP <= 0)
            {
                _currentHP = 0;
                _controller.EnemyControl.Win();
            }
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Warning);
            _controller.InvokeOnPlayerHPChanged(_currentHP);
        }

        public void Die()
        {
            _isActive = false;
            _playerFighting.SetActive(false);
            _playerMovement.SetActive(false);
            _animator.PlayAnimation(EAnimStyle.Die);
        }

        public void Win()
        {
            _isActive = false;
            _playerFighting.SetActive(false);
            _animancer.Play(_winAnims.RandomItem());

            Vector3 dirToCam = CameraControl.I.transform.position - transform.position;
            dirToCam.y = 0f;
            transform.DORotateQuaternion(Quaternion.LookRotation(dirToCam), 0.5f);
            CameraControl.I.LookAtTarget(transform.position + Vector3.up * 0.5f, 0.25f);
        }

        private void Update()
        {
            if (!_isActive) return;
            if (_isFightBoss) return;

            _currentHP -= _speedHPDown * Time.deltaTime;
            if (_currentHP <= 0)
            {
                _currentHP = 0;
                _controller.Lose();
            }
            _controller.InvokeOnPlayerHPChanged(_currentHP);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!_isActive) return;
            if (other.CompareTag(Tag.Player_Collect))
            {
                PlayerCollect playerCollect = other.GetComponent<PlayerCollect>();
                if (playerCollect)
                {
                    var damageTake = _configItems[playerCollect.TypeItem];
                    _currentHP += damageTake;
                    if (_currentHP > 1) _currentHP = 1;
                    else if (_currentHP <= 0)
                    {
                        _currentHP = 0;
                        _controller.Lose();
                    }
                    VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
                    _controller.InvokeOnPlayerHPChanged(_currentHP);
                    if (damageTake < 0)
                    {
                        GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_HIT);
                    }
                    else
                    {
                        GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_PICKUP);
                        _fxHeal.Play();
                    }
                    other.gameObject.SetActive(false);
                }
            }

            else if (other.CompareTag(Tag.Check_Point))
            {
                _isActive = false;
                _playerMovement.SetActive(false);
                _playerFighting.MoveToPosFight();
                _isFightBoss = true;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_isActive) return;
            if (collision.gameObject.CompareTag(Tag.Player_Collect))
            {
                PlayerCollect playerCollect = collision.gameObject.GetComponent<PlayerCollect>();
                if (playerCollect)
                {
                    var damageTake = _configItems[playerCollect.TypeItem];
                    _currentHP += damageTake;
                    if (_currentHP > 1) _currentHP = 1;
                    else if (_currentHP <= 0)
                    {
                        _currentHP = 0;
                        _controller.Lose();
                    }
                    _controller.InvokeOnPlayerHPChanged(_currentHP);
                    if (damageTake < 0)
                    {
                        GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_HIT);
                    }
                    else
                    {
                    }
                    VibrationManager.I.Haptic(VibrationManager.EHapticType.Warning);
                }

                FakeAddForce(collision.GetContact(0).point);
            }
        }

        private void FakeAddForce(Vector3 posCollision)
        {
            _playerMovement.SetActive(false);
            Vector3 dir = transform.position - posCollision;
            dir.y = 0f;
            Vector3 wantedPos = transform.position + dir.normalized * 1.5f;
            Tweener t = transform.DOMove(wantedPos, 0.25f);
            t.OnComplete(() =>
            {
                if (_isActive)
                    _playerMovement.SetActive(true);
            });
        }

        public void Revive()
        {
            _currentHP = 1f;
            _controller.InvokeOnPlayerHPChanged(_currentHP);
            if (_isFightBoss)
            {
                _animator.PlayAnimation(EAnimStyle.Idle);
                _playerFighting.SetActive(true);
            }
            else
            {
                _isActive = true;
                _playerMovement.SetActive(true);
                _animator.PlayAnimation(EAnimStyle.Run);
            }
        }
    }
}