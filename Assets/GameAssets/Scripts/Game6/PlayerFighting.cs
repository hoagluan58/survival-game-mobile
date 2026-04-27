using Animancer;
using DG.Tweening;
using SquidGame.Core;
using SquidGame.LandScape;
using UnityEngine;

namespace Game6
{
    public class PlayerFighting : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private float _damage;
        [Header("Refs")]
        [SerializeField] private Transform _posPlayerFight;
        [Header("ANIM")]
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private ClipTransition _idleAnim;
        [SerializeField] private ClipTransition _runAnim;
        [SerializeField] private ClipTransition _punchAnim;

        private Game6Control _controller;
        private bool _isActive;

        private void OnEnable()
        {
            _punchAnim.Events.OnEnd += OnPunchStateEnded;
        }

        private void OnDisable()
        {
            _punchAnim.Events.OnEnd -= OnPunchStateEnded;
        }

        private void OnPunchStateEnded() => _animancer.Play(_idleAnim);

        public void Init(Game6Control controller) => _controller = controller;

        public void MoveToPosFight()
        {
            Vector3 dir = _posPlayerFight.position - transform.position;
            transform.DORotateQuaternion(Quaternion.LookRotation(dir), 0.25f);

            Tweener t = transform.DOMove(_posPlayerFight.position, 1.5f);
            t.SetEase(Ease.Linear);
            t.OnComplete(() =>
            {
                transform.DORotateQuaternion(Quaternion.LookRotation(Vector3.forward), 0.25f);
                _controller.PrepareFight();
                _animancer.Play(_idleAnim);
            });
        }

        public void SetActive(bool b)
        {
            _isActive = b;
        }

        private void Update()
        {
            if (_isActive && Input.GetMouseButtonDown(0))
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_PUNCH);
                _animancer.Play(_punchAnim);
                _controller.EnemyControl.TakeDamage(_damage);
            }
        }
    }
}