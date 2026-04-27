using DG.Tweening;
using SquidGame.Gameplay;
using System.Collections;
using UnityEngine;

namespace SquidGame.Minigame21
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerArrow _arrow;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private FollowerController _followerController;
        [SerializeField] private PlayerRoomInteractionHandler _playerRoomInteractionHandler;
        [SerializeField] private BaseCharacter _model;

        [Header("CONFIG")]
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _gravity = -9.8f;

        private Vector3 _initPosition;
        private Vector3 _initRotation;
        private BotManager _botManager;
        private RoomManager _roomManager;
        private bool _isActive = false;
        private VariableJoystick _joystick;
        private Vector3 _velocity;
        private Camera _camera;

        public void Init(VariableJoystick joystick, BotManager botManager, RoomManager roomManager)
        {
            _camera = Camera.main;
            _initPosition = transform.position;
            _initRotation = transform.eulerAngles;
            _joystick = joystick;
            _botManager = botManager;
            _roomManager = roomManager;

            _arrow.Init(this, _followerController, _playerRoomInteractionHandler);
            _followerController.Init(_playerRoomInteractionHandler);
            _playerRoomInteractionHandler.Init(_followerController, _roomManager);
        }

        public void OnPrepare()
        {
            _model.Animator.PlayAnimation(EAnimStyle.Idle);
            transform.position = _initPosition;
            transform.eulerAngles = _initRotation;
            _followerController.OnPrepare(_botManager.FollowerBots);
        }

        public void OnStart()
        {
            _isActive = true;
            _followerController.OnStart();
            _arrow.SetActive(true);
        }

        public IEnumerator CROnWinRound()
        {
            _isActive = false;
            _arrow.SetActive(false);
            transform.DOMove(_playerRoomInteractionHandler.PlayerRoom.RandomPositionInRoom(), 0.2f).OnComplete(() =>
            {
                _model.Animator.PlayAnimation(EAnimStyle.Idle);
            });
            yield return _playerRoomInteractionHandler.CRReleaseFollowerToRoom();
        }

        public IEnumerator CROnLoseRound()
        {
            _isActive = false;
            _model.Animator.PlayAnimation(EAnimStyle.Die);
            _arrow.SetActive(false);
            _followerController.Stop();
            yield return null;
        }

        private void Update()
        {
            if (!_isActive) return;

            HandleUserInput();
        }

        private void HandleUserInput()
        {
            var joystickDirection = _joystick.Direction;

            // Handle rotation
            if (joystickDirection.magnitude > 0.1f)
            {
                var targetAngle = Mathf.Atan2(joystickDirection.x, joystickDirection.y) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, targetAngle, 0);
            }

            var move = transform.forward * joystickDirection.magnitude;

            // Move character
            _characterController.Move(_speed * Time.deltaTime * move);

            // Handle animations
            var movementSpeed = move.magnitude;
            _model.Animator.PlayAnimation(movementSpeed > 0.1f ? EAnimStyle.Run : EAnimStyle.Idle);

            // Handle gravity
            if (_characterController.isGrounded)
            {
                _velocity.y = 0;
            }

            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }
}
