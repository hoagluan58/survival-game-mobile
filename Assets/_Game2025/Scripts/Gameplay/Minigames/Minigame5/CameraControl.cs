using DG.Tweening;
using NFramework;
using UnityEngine;

namespace SquidGame.LandScape.Minigame5
{
    public class CameraControl : SingletonMono<CameraControl>
    {
        [SerializeField] private float _speedFollow;

        [SerializeField] private Transform _posFollow;
        [SerializeField] private Transform _posCamWin;
        [SerializeField] private Transform _posCamVictory;
        [SerializeField] private Transform _playerName;
        // [SerializeField] private Transform[] _posCamLoseList;

        private Vector3 _offset;
        private bool _isFollow, _isLose;

        private void Start()
        {
            _isFollow = true;
            _offset = transform.position - _posFollow.position;
        }

        private void LateUpdate()
        {
            _playerName.LookAt(transform);
            if (!_isFollow) return;

            transform.position = Vector3.Lerp(transform.position, _posFollow.position + _offset + (_isLose ? transform.forward * 8f : Vector3.zero), Time.deltaTime * _speedFollow);
        }

        public void MoveToPosCamWin(float time)
        {
            _isFollow = false;
            _playerName.gameObject.SetActive(false);
            transform.DOMove(_posCamWin.position, time);
            transform.DORotateQuaternion(_posCamWin.rotation, time);
        }

        public void MoveToPosShowVictory(float time)
        {
            transform.DOMove(_posCamVictory.position, time);
            transform.DORotateQuaternion(_posCamVictory.rotation, time);
        }

        public void MoveToPosCamLose()
        {
            _isLose = true;
            _playerName.gameObject.SetActive(false);

            // var d = Vector3.Lerp(transform.position, _posFollow.position, 0.5f);

            // _isFollow = false;
            // transform.DORotateQuaternion(_posCamLoseList[1].rotation, 1.5f);
            // Sequence sq = DOTween.Sequence();
            // sq.Append(transform.DOMove(_posCamLoseList[0].position, 0.5f).SetEase(Ease.InQuad));
            // sq.Append(transform.DOMove(_posCamLoseList[1].position, 1f).SetEase(Ease.OutQuad));
        }
    }
}