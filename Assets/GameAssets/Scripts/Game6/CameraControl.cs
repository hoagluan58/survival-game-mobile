using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NFramework;

namespace Game6
{
    public class CameraControl : SingletonMono<CameraControl>
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speedFollow;

        [SerializeField] private Transform _posCamFight;

        private Vector3 _offset;
        private bool _isFollow;

        private void Start()
        {
            _isFollow = true;
            _offset = transform.position - _target.position;
        }

        private void LateUpdate()
        {
            if (!_isFollow) return;
            transform.position = Vector3.Lerp(transform.position, _target.position + _offset, Time.deltaTime * _speedFollow);
        }

        public void MoveToPosCamWin(float time, System.Action callBack)
        {
            _isFollow = false;
            Tweener t = transform.DOMove(_posCamFight.position, time);
            transform.DORotateQuaternion(_posCamFight.rotation, time + 0.35f);

            t.OnComplete(() =>
            {
                callBack?.Invoke();
            });
        }

        public void LookAtTarget(Vector3 target, float time)
        {
            Vector3 dir = target - transform.position;
            transform.DORotateQuaternion(Quaternion.LookRotation(dir), time);
        }
    }
}