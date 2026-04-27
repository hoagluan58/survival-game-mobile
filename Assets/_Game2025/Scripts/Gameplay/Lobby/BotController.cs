using System.Collections;
using DG.Tweening;
using SquidGame.LandScape.Game;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace SquidGame.LandScape.Lobby
{
    public class BotController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private TMP_Text _nameText;

        [Header("CONFIG")]
        [SerializeField] private float _randomPosition = 10f;

        private bool _isMoving, _isIdle, _isHeadingToGate;
        private Vector3 _destination, _gatePosition;
        private Transform _cameraTransform;

        private void Awake()
        {
            StartCoroutine(NormalBehaviourInLobby());
        }

        private void Start()
        {
            Idle();
        }

        public void Init(Vector3 gatePosition, int nameId)
        {
            _cameraTransform = Camera.main.transform;
            _gatePosition = gatePosition + new Vector3(Random.Range(-5f, 5f), 0, 0);
            _nameText.text = nameId.ToString("D3");
        }

        private void Update()
        {
            if (_navMeshAgent.remainingDistance <= 0.05f && _isMoving)
            {
                if (_isHeadingToGate)
                {
                    gameObject.SetActive(false);
                    return;
                }

                if (Random.Range(0f, 1f) >= 0.4f) Idle();
                else RandomMove();
            }
        }

        private void LateUpdate()
        {
            _nameText.transform.LookAt(_cameraTransform);
        }

        void RandomMove()
        {
            if (Random.Range(0f, 1f) >= 0.9f)
            {
                MoveToGate();
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

        void MoveToGate()
        {
            _isIdle = false;
            _isMoving = true;
            _isHeadingToGate = true;
            _animator.PlayAnimation(EAnimStyle.Running, 0.2f);
            _navMeshAgent.SetDestination(_gatePosition);

        }

        WaitForSeconds _intervalWait = new WaitForSeconds(0.5f);
        IEnumerator NormalBehaviourInLobby()
        {
            while (!_isHeadingToGate)
            {
                if (_isIdle)
                {
                    if (Random.Range(0f, 1f) >= 0.4f)
                    {
                        transform.DORotate(new Vector3(0, Random.Range(-35, 35)), Random.Range(0.1f, 0.3f)).SetRelative();
                    }
                    else
                    {
                        RandomMove();
                    }
                }

                yield return _intervalWait;
            }
        }

        public void Jump()
        {

        }
    }
}
