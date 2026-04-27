using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NFramework;

namespace Game9
{
    public class CameraControl : SingletonMono<CameraControl>
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speedFollow;

        [SerializeField] private Transform _posCamWin;
        [SerializeField] private Transform _posStartFollow;

        private Vector3 _offset;
        private bool _isFollow;

        private void LateUpdate()
        {
            if (!_isFollow) return;
            Vector3 wantedPos = _target.position + _offset;
            transform.position = Vector3.Lerp(transform.position, wantedPos, Time.deltaTime * _speedFollow);
        }

        public void StartFollow()
        {
            _isFollow = true;
            _offset = transform.position - _target.position;
        }

        public void StopFollow()
        {
            _isFollow = false;
        }

        public void MoveToPosCamWin(float time, System.Action callBack)
        {
            _isFollow = false;
            Tweener t = transform.DOMove(_posCamWin.position, time);
            transform.DORotateQuaternion(_posCamWin.rotation, time + 0.35f);

            t.OnComplete(() =>
            {
                callBack?.Invoke();
            });
        }

        public void MoveToPosStartFollow(float time, System.Action callBack)
        {
            _isFollow = false;
            Tweener t = transform.DOMove(_posStartFollow.position, time);
            transform.DORotateQuaternion(_posStartFollow.rotation, time + 0.35f);

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

        public void ZoomToTarget(Vector3 target, float time)
        {
            Vector3 dir = transform.position - target;
            Vector3 wantedPos = target + dir.normalized * 10f;
            transform.DOMove(wantedPos, time);
        }
    }
}