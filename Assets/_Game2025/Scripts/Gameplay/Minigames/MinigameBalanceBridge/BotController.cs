using System.Collections;
using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace SquidGame.LandScape.BalanceBridge
{
    public class BotController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private Transform _playTransform, _winTransform;

        [SerializeField] private AnimationClip _walkAnim;
        [SerializeField] private AnimationClip _walkLeaningAnim;
        [SerializeField] private AnimationClip _fallLeftAnim;
        [SerializeField] private AnimationClip _fallRightAnim;

        [SerializeField] private float _randomPosition = 10f, _speed = 1f;

        private bool _isActive = true, _isMoving, _isIdle, _isPlaying, _isHeadingToPlay;
        private Vector3 _destination;

        private void Start()
        {
            Idle();
            StartCoroutine(NormalBehaviour());
        }

        private void Update()
        {
            if (!_isActive) return;
            if (_isPlaying)
            {
                transform.Translate(_speed * Time.deltaTime * Vector3.forward);
                if (Vector3.Distance(transform.position, _winTransform.position) <= 1f)
                {
                    _isActive = false;
                    _animator.PlayAnimation(EAnimStyle.Running, 0.2f);
                    Vector3 dancePosition = transform.position + new Vector3(Random.Range(-7f, 7f), 0, Random.Range(0.5f, 2.5f));
                    float distance = Vector3.Distance(transform.position, dancePosition);
                    float moveDuration = distance / 5;
                    transform.DOMove(dancePosition, moveDuration).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        transform.DORotate(new Vector3(0f, 180f, 0f), 0.5f);
                        _animator.PlayAnimation(0.2f, EAnimStyle.Victory_1, EAnimStyle.Victory_2, EAnimStyle.Victory_3);
                    });
                    Quaternion lookRotation = Quaternion.LookRotation((dancePosition - transform.position).normalized);
                    transform.DORotateQuaternion(lookRotation, 0.2f);
                }
                return;
            }

            if (_navMeshAgent.remainingDistance <= 0.05f && _isMoving)
            {
                if (_isHeadingToPlay)
                {
                    Idle();
                    _animator.PlayAnimation(_walkAnim);
                    transform.SetEulerAngleY(0);
                    transform.SetLocalPosX(0);

                    _navMeshAgent.enabled = false;
                    _isPlaying = true;
                    if (PercentChance(50))
                        StartCoroutine(Falling());
                }
                else
                {
                    if (PercentChance(60)) Idle();
                    else RandomMove();
                }
            }
        }

        void RandomMove()
        {
            if (PercentChance(10))
            {
                MoveToPlay();
                return;
            }

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
            _animator.PlayAnimation(EAnimStyle.Idle, 0.2f);
            _isIdle = true;
            _isMoving = false;
        }

        void MoveToPlay()
        {
            _animator.PlayAnimation(EAnimStyle.Running, 0.2f);
            _navMeshAgent.SetDestination(_playTransform.position);

            _isIdle = false;
            _isMoving = true;
            _isHeadingToPlay = true;
        }

        WaitForSeconds _intervalWait = new(0.5f);
        IEnumerator NormalBehaviour()
        {
            while (!_isHeadingToPlay)
            {
                if (_isIdle)
                {
                    if (PercentChance(50))
                        transform.DORotate(new Vector3(0, Random.Range(-35, 35)), Random.Range(0.1f, 0.3f)).SetRelative();
                    else RandomMove();
                }

                yield return _intervalWait;
            }
        }

        IEnumerator Falling()
        {
            yield return new WaitForSeconds(Random.Range(2f, 8f));
            _animator.PlayAnimation(_walkLeaningAnim, 0.2f);
            yield return new WaitForSeconds(2f);
            _isActive = false;
            bool fallRight = PercentChance(50);
            transform.SetPosX(transform.position.x + (fallRight ? 0.6f : -0.6f));
            transform.SetEulerAngleY(fallRight ? -90 : 90);
            _animator.PlayAnimation(fallRight ? _fallRightAnim : _fallLeftAnim, 0.2f);

            transform.DOMoveX(transform.position.x + (fallRight ? 1f : -1f), 0.5f).SetDelay(0.6f)
                .OnComplete(() => GameSound.I.PlaySFX(Define.SoundPath.SFX_BALANCE_BRIDGE_FALL));
            transform.DOMoveY(transform.position.y - 4f, 0.7f).SetDelay(0.8f);
        }

        bool PercentChance(int percent)
        {
            return Random.Range(1, 101) <= percent;
        }
    }
}
