using DG.Tweening;
using NFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SquidGame.Minigame12
{
    public class Ddakji : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private BoxCollider _boxCollider;

        [Header("CONFIG")]
        [SerializeField] private float _throwForce = 50f;
        [SerializeField] private float _jumpHeight = 2f;
        [SerializeField] private float _jumpDuration = 1f;

        private bool _isThrowCorrect;
        private bool _isDetectCollision = true;

        public bool IsThrowCorrect => _isThrowCorrect;

        public void Init(bool isDetectCollision)
        {
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            _rigidbody.isKinematic = true;
            _isDetectCollision = isDetectCollision;
        }

        public void Throw(Vector3 targetPos, bool isThrowCorrect)
        {
            _isThrowCorrect = isThrowCorrect;
            _isDetectCollision = false;
            _rigidbody.isKinematic = false;
            var direction = (targetPos - transform.position).normalized;
            _rigidbody.velocity = direction * _throwForce;
        }

        public void FlipAndRotate(bool isUpsideDown)
        {
            _isDetectCollision = false;
            _rigidbody.isKinematic = false;
            transform?.DOKill();
            transform.DOJump(transform.position, _jumpHeight, 1, _jumpDuration);
            var correctRotationX = new float[] { 180f, 540f, };
            var wrongRotationX = new float[] { 0f, 360f };
            var rndTime = Random.Range(0.5f, 1.2f);
            transform.DOLocalRotate(isUpsideDown ? new Vector3(correctRotationX.RandomItem(), 0f, 0f) : new Vector3(wrongRotationX.RandomItem(), 0f, 0f),
                                    rndTime, RotateMode.WorldAxisAdd)
                     .OnComplete(() =>
                     {
                         _rigidbody.isKinematic = true;
                     });
        }

        public bool IsWithinBounds(Vector3 position)
        {
            var boundsY = _boxCollider.bounds.center.y;
            position.y = boundsY;
            return _boxCollider.bounds.Contains(position);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_isDetectCollision) return;

            var ddakji = collision.gameObject.GetComponent<Ddakji>();
            if (ddakji != null)
            {
                var isTargetThrowCorrect = ddakji.IsThrowCorrect;
                FlipAndRotate(isTargetThrowCorrect);
            }
        }

        [Button]
        public void AddForce(Vector3 vector3)
        {
            _rigidbody.AddForce(vector3);
        }
    }
}
