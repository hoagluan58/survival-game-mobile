using Animancer;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using UnityEngine;

namespace Game9
{
    public class EnemyBot : MonoBehaviour
    {
        [SerializeField] private float _speedMove;
        [SerializeField] private Vector2 _rangleX;
        [SerializeField] private Vector2 _rangleZ;

        [Header("Refs")]
        [SerializeField] private GameObject _fxBlood;
        [SerializeField] private BaseCharacter _baseCharacter;

        private bool _isActive = true;
        private Vector3 _target;

        private Game9Control _controller;

        public bool IsActive => _isActive;

        private void Start()
        {
            GetRandomPos();
        }

        public void Init(Game9Control controller)
        {
            _controller = controller;
            _fxBlood.SetActive(false);
        }

        private void GetRandomPos()
        {
            _target.x = Random.Range(_rangleX.x, _rangleX.y);
            _target.z = Random.Range(_rangleZ.x, _rangleZ.y);
        }

        private void Update()
        {
            if (!_isActive) return;

            Vector3 dir = _target - transform.position;
            if (dir.magnitude <= 1f)
            {
                GetRandomPos();
            }

            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(dir.normalized);
                transform.Translate(dir.normalized * _speedMove * Time.deltaTime, Space.World);
                _baseCharacter.Animator.PlayAnimation(EAnimStyle.Run);
            }
            else _baseCharacter.Animator.PlayAnimation(EAnimStyle.Idle);
        }


        public void TakeDamage(Vector3 sourceDirection)
        {
            _isActive = false;
            GameSound.I.PlaySFX(Random.value >= 0.5f ? Define.SoundPath.SFX_MG01_HOSTAGE_F_HIT_01 : Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
            _baseCharacter.Animator.PlayAnimation(EAnimStyle.Die);
            _fxBlood.SetActive(true);
            var knockbackDir = (transform.position - sourceDirection).normalized;
            _baseCharacter.ToggleRagdoll(true);
            _baseCharacter.Ragdoller.ApplyForce(knockbackDir, 20f);
            _controller.EnemyDie();
        }
    }
}
