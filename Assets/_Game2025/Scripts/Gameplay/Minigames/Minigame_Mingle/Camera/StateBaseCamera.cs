using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    public abstract class StateBaseCamera
    {
        protected GameObject Camera;
        protected CameraController CameraController;

        public virtual void Init(CameraController cameraController) => CameraController = cameraController;
        public virtual void Enter() { }
        public virtual void Exit() { }
    }
}
