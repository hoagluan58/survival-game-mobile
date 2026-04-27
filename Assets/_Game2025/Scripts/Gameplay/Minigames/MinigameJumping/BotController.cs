using System.Collections;
using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace SquidGame.LandScape.Jumping
{
    public class BotController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private BaseCharacter _baseCharacter;

        [SerializeField] private float _randomPosition = 10f;

        private bool _isActive = true, _isMoving, _isIdle, _isJumping;
        private Vector3 _destination;
        private Coroutine _coroutine;
        private CharacterController _characterController;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            Idle();
            _coroutine = StartCoroutine(NormalBehaviour());
        }

        private void Update()
        {
            if (!_isActive || !_navMeshAgent.enabled) return;

            if (_navMeshAgent.remainingDistance <= 0.05f && _isMoving)
            {
                if (PercentChance(20)) Idle();
                else RandomMove();
            }
        }

        void FixedUpdate()
        {
            if (_isJumping)
            {
                _velocity.y += _gravity * Time.fixedDeltaTime;
                _characterController.Move(_velocity * Time.deltaTime);
                if (_characterController.isGrounded)
                {
                    _velocity.y = 0;
                    _navMeshAgent.enabled = true;
                    _isJumping = false;
                    if (!_isActive)
                    {
                        _characterController.enabled = false;
                        _navMeshAgent.enabled = false;
                    }
                }
            }
        }

        void RandomMove()
        {
            if (!_navMeshAgent.enabled) return;
            _animator.PlayAnimation(EAnimStyle.Running, 0.2f);
            _destination = transform.position + new Vector3(Random.Range(-_randomPosition, _randomPosition), 0, Random.Range(-_randomPosition, _randomPosition));
            if (NavMesh.SamplePosition(_destination, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                _navMeshAgent.SetDestination(_destination);
            }

            _isIdle = false;
            _isMoving = true;
        }

        void Idle()
        {
            _navMeshAgent.velocity = Vector3.zero;
            _animator.PlayAnimation(EAnimStyle.Idle, 0.2f);
            _isIdle = true;
            _isMoving = false;
        }

        void Die()
        {
            _isActive = false;
            if (_navMeshAgent.enabled)
                _navMeshAgent.isStopped = true;

            if (!_isJumping)
                _characterController.enabled = false;

            _navMeshAgent.enabled = false;
            _characterController.excludeLayers = LayerMask.GetMask("Bot");
            _baseCharacter.ToggleGreyScale(true);
            _animator.PlayAnimation(EAnimStyle.Die, 0.2f);
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }

        WaitForSeconds _intervalWait = new(0.5f);
        IEnumerator NormalBehaviour()
        {
            while (_isActive)
            {
                if (_isIdle)
                {
                    if (PercentChance(20))
                        transform.DORotate(new Vector3(0, Random.Range(-35, 35)), Random.Range(0.1f, 0.3f)).SetRelative();
                    else RandomMove();
                }

                yield return _intervalWait;
            }
        }

        public void OnPlayerDead()
        {
            if (!_isActive) return;
            _navMeshAgent.velocity = Vector3.zero;
            Idle();

            _isMoving = false;
            if (_coroutine != null)
                StopCoroutine(_coroutine);

        }

        public void OnPlayerRevive()
        {
            if (!_isActive) return;
            RandomMove();
            _coroutine = StartCoroutine(NormalBehaviour());
        }

        bool PercentChance(int percent)
        {
            return Random.Range(1, 101) <= percent;
        }

        Vector3 _velocity;
        int _gravity = -15;
        float _jumpHeight = 3f;

        void Jump()
        {
            if (_isJumping) return;

            _animator.PlayAnimation(EAnimStyle.Jump, 0.2f, Animancer.FadeMode.FromStart);
            _isJumping = true;
            _navMeshAgent.enabled = false;
            _velocity.y = Mathf.Sqrt(2 * -_gravity * _jumpHeight);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Dead_Zone"))
            {
                Die();
                return;
            }

            if (other.transform.CompareTag("Bot"))
            {
                if (PercentChance(80))
                    Jump();
            }
        }
    }
}
