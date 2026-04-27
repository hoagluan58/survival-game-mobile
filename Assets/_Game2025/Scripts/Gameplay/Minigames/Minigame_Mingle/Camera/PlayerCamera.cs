namespace SquidGame.LandScape.MinigameMingle
{
    public class PlayerCamera : StateBaseCamera
    {
        public override void Init(CameraController cameraController)
        {
            base.Init(cameraController);
            Camera = cameraController.PlayerCamera;
        }

        public override void Enter()
        {
            base.Enter();
            Camera.SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();
            Camera.SetActive(false);
        }
    }
}
