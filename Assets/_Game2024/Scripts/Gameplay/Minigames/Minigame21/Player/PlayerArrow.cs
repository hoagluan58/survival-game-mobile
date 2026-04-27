using UnityEngine;

namespace SquidGame.Minigame21
{
    public class PlayerArrow : MonoBehaviour
    {
        [SerializeField] private GameObject _arrow;

        private bool _isActive;
        private Bot _currentBot;
        private PlayerController _player;
        private FollowerController _followerController;
        private PlayerRoomInteractionHandler _playerRoomHandler;

        public void Init(PlayerController player, FollowerController followerController, PlayerRoomInteractionHandler playerRoomHandler)
        {
            _player = player;
            _followerController = followerController;
            _playerRoomHandler = playerRoomHandler;

            FollowCharacter();
            SetActive(false);
        }

        public void SetActive(bool value)
        {
            _isActive = value;
            _arrow.SetActive(value);
        }

        private void Update()
        {
            if (!_isActive) return;

            FollowCharacter();

            if (!TryRotateToBot())
            {
                RotateToRoom();
            }
        }

        private void FollowCharacter() => transform.position = _player.transform.position;

        private void RotateArrow(Vector3 targetPosition)
        {
            var direction = targetPosition - transform.position;
            var angleY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, angleY, 0);
        }

        private bool TryRotateToBot()
        {
            _currentBot = _followerController.FirstRemainingFollower;
            if (_currentBot == null) return false;

            RotateArrow(_currentBot.transform.position);
            return true;
        }

        private void RotateToRoom() => RotateArrow(_playerRoomHandler.PlayerRoom.transform.position);
    }
}
