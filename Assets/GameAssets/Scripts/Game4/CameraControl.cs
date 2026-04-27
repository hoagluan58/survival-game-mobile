using DG.Tweening;
using NFramework;
using UnityEngine;

namespace Game4
{
    public class CameraControl : SingletonMono<CameraControl>
    {
        [SerializeField] private Transform _posCamWinLose;

        public void MoveToPosCamWinLose(float time)
        {
            transform.DOMove(_posCamWinLose.position, time);
            transform.DORotateQuaternion(_posCamWinLose.rotation, time);
        }
    }
}
