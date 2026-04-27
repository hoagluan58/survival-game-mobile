using Animancer;
using SquidGame.LandScape.Game;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private BaseCharacter _baseCharacter;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerDetectDoor _playerDetectDoor;
        [SerializeField] private ParticleSystem _fxSand;
        [SerializeField] private ParticleSystem _fxBlood;
        [SerializeField] private Transform _tfHead;
        [SerializeField] private Transform _tfName;
        private VariableJoystick _joystick;
        private MinigameController _controller;
        private CharacterAnimator _charAnimator;

        public CharacterController CharacterController => _characterController;
        public PlayerMovement PlayerMovement => _playerMovement;
        public VariableJoystick Joystick => _joystick;
        public ParticleSystem FxSand => _fxSand;
        public Transform TfHead => _tfHead;


        private void LateUpdate()
        {
            _tfName.LookAt(_controller.CameraController.CinemachineBrain.transform);
        }

        public void Init(MinigameController minigameController, VariableJoystick joystick)
        {
            _controller = minigameController;
            _joystick = joystick;
            _charAnimator = _baseCharacter.GetCom<CharacterAnimator>();
            _charAnimator.PlayAnimation(Game.EAnimStyle.Idle);
            _playerMovement.Init(this);
            _playerDetectDoor.Init(this);
        }

        public void OnWin()
        {
            _charAnimator.PlayAnimation(EAnimStyle.Victory_1, EAnimStyle.Victory_2, EAnimStyle.Victory_3);
        }

        public void OnLose()
        {
            _charAnimator.PlayAnimation(EAnimStyle.Die);
            _fxBlood.Play();
            _baseCharacter.ToggleGreyScale(true);
        }

        public void OnReplay()
        {
            _baseCharacter.ToggleGreyScale(false);
        }

        public void SwitchPlayerAnimation(EAnimStyle style, float fadeDuration = 0.2f, FadeMode fadeMode = FadeMode.FixedSpeed)
        {
            _charAnimator.PlayAnimation(style, fadeDuration, fadeMode);
        }

        public void ResetPlayer()
        {
            _playerMovement.enabled = true;
        }
    }
}
