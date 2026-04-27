using DG.Tweening;
using NFramework;
using UnityEngine;

namespace Game2
{
    public class CameraControl : SingletonMono<CameraControl>
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Vector3 _wantedRotation;
        [SerializeField] private float _speedFollow;
        [SerializeField] private float _speedFollowPhysic;
        [SerializeField] private bool _isKeepPosX;
        [SerializeField] private bool _isKeepPosY;

        [SerializeField] private float _speedRotate;

        [Header("For Game Win")]
        [SerializeField] private Vector3 _posGameWin;
        [SerializeField] private Vector3 _rotGameWin;

        private bool _isActiveFollow;
        private bool _isRotateAround;
        private bool _isLookInTarget;
        private bool _isPhysicTarget;

        private float _initPosX;
        private float _initPosY;

        private void Start()
        {
            _initPosX = transform.position.x;
            _initPosY = transform.position.y;
        }

        public void MoveToPosGameWin(float time)
        {
            _target = null;
            Vector3 wantedPos = _posGameWin + Vector3.forward * transform.position.z;
            transform.DOMove(wantedPos, time);
            transform.eulerAngles = _rotGameWin;
        }

        public void LookAtTarget(Transform target, float time)
        {
            Vector3 dir = target.position - transform.position;
            transform.DORotateQuaternion(Quaternion.LookRotation(dir), time);
        }

        private void Update()
        {
            if (_target == null) return;
            if (_isRotateAround)
            {
                transform.RotateAround(_target.transform.position, Vector3.up, -Time.deltaTime * _speedRotate);
            }

            if (_isActiveFollow && _isPhysicTarget == false)
            {
                Vector3 wantedPos = _target.position + _offset;
                if (_isKeepPosX)
                    wantedPos.x = _initPosX;
                if (_isKeepPosY)
                    wantedPos.y = _initPosY;

                transform.position = Vector3.Lerp(transform.position, wantedPos, Time.deltaTime * _speedFollow);

                if (_target.position.x > 0f)
                    _wantedRotation.y = -5f;
                else if (_target.position.x < 0f)
                    _wantedRotation.y = 5f;
                else _wantedRotation.y = 0f;

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_wantedRotation), Time.deltaTime * _speedRotate);

                if (_isLookInTarget)
                    transform.LookAt(_target);
            }
        }

        private void FixedUpdate()
        {
            if (_target == null) return;
            if (_isActiveFollow && _isPhysicTarget)
            {
                Vector3 wantedPos = _target.position + _offset;
                if (_isKeepPosX) wantedPos.x = 0f;
                transform.position = Vector3.Lerp(transform.position, wantedPos, Time.fixedDeltaTime * _speedFollowPhysic);

                if (_isLookInTarget)
                    transform.LookAt(_target);
            }
        }

        public void ActiveFollow(bool b)
        {
            _isActiveFollow = b;
        }

        public void ActiveRotateAround(bool b)
        {
            _isRotateAround = b;
        }

        public void SetSpeedFollow(float val)
        {
            _speedFollow = val;
        }

        public void SetTarget(Transform target, bool isPhysicTarget, bool isLookInTarget)
        {
            _isPhysicTarget = isPhysicTarget;
            _isLookInTarget = isLookInTarget;
            _target = target;
        }
    }
}