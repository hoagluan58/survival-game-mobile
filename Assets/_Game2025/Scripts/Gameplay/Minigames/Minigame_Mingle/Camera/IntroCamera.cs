using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.MinigameMingle
{
    public class IntroCamera : StateBaseCamera
    {
        private Transform _target;
        private event UnityAction _onCompleted;
        private Vector3 _localPosition;
        public override void Init(CameraController cameraController)
        {
            base.Init(cameraController);
            
            Camera = CameraController.IntroCamera;
            _target = CameraController.TargetIntro;
            _localPosition = Camera.transform.localPosition;
        }


        public override void Enter()
        {
            base.Enter();
            Camera.SetActive(true);
            Camera.transform.DOKill();
            Camera.transform.localPosition = _localPosition;
            DOVirtual.DelayedCall(2f, () =>
            {
                Camera.transform.DOMove(_target.position, 3).OnComplete(() => OnCompletedAnimation());
                Camera.transform.DORotate(_target.eulerAngles, 3);
            });
        }

        public override void Exit()
        {
            base.Exit();
            Camera.transform.DOKill();
            Camera.SetActive(false);
            _onCompleted = null;
        }

        public StateBaseCamera AddListernerCompleted(UnityAction action)
        {
            _onCompleted += action;
            return this; 
        }

        public StateBaseCamera OnCompletedAnimation()
        {
            _onCompleted?.Invoke();
            _onCompleted = null;
            return this;
        }
    }
}
