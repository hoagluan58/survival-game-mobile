using Animancer;
using DG.Tweening;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using UnityEngine;

namespace Game6
{
    public class EnemyControl : MonoBehaviour
    {
        [SerializeField] private float _currentHP = 1f;
        [SerializeField] private float _damage = 0.1f;
        [SerializeField] private float _intervalFight = 0.5f;

        [Header("ANNIMATIONS")]
        [SerializeField] private CharacterAnimationController _animator;
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private ClipTransition _idleAnim;
        [SerializeField] private ClipTransition _runAnim;
        [SerializeField] private ClipTransition _punchAnim;
        [SerializeField] private AnimationClip[] _winAnims;

        private Game6Control _controller;
        private bool _isActive;
        private float _hitTimer;

        private Tween _delayPlayWinAnim;

        public void Init(Game6Control controller)
        {
            _controller = controller;
            _animancer.Play(_idleAnim);
        }

        public void RunToPlayer(Vector3 pos)
        {
            _animancer.Play(_runAnim);
            var wantedPos = pos + Vector3.forward * 2f;
            transform.DOMove(wantedPos, 1f)
                .SetEase(Ease.Linear)
                .OnComplete(() => { _animancer.Play(_idleAnim); });
        }

        public void StartFight()
        {
            _delayPlayWinAnim?.Kill();
            _animancer.Play(_idleAnim);
            _isActive = true;
            _hitTimer = 0;
        }

        private void FixedUpdate()
        {
            if (_isActive)
            {
                _hitTimer += Time.fixedDeltaTime;
                if (_hitTimer > _intervalFight)
                {
                    _controller.PlayerControl.TakeEnemyDamage(_damage);
                    _animancer.Play(_punchAnim);
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_PUNCH);
                    _hitTimer = 0;
                }
            }
        }

        public void TakeDamage(float dmg)
        {
            _currentHP -= dmg;
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_HIT);
            if (_currentHP <= 0)
            {
                Die();
            }
            _controller.InvokeOnEnemyHPChanged(_currentHP);
        }

        public void Die()
        {
            _isActive = false;
            _currentHP = 0;
            _animator.PlayAnimation(EAnimStyle.Die);
            _controller.Win();
        }

        public void Win()
        {
            _isActive = false;
            _delayPlayWinAnim = DOVirtual.DelayedCall(0.1f, () => _animancer.Play(_winAnims.RandomItem()));
            _controller.Lose();
        }
    }
}