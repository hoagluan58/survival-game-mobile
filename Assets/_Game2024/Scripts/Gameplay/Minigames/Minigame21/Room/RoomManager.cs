using NFramework;
using SquidGame.Core;
using SquidGame.LandScape;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SquidGame.Minigame21
{
    public class RoomManager : MonoBehaviour
    {
        [Header("REF")]
        [SerializeField] private List<RoomController> _rooms;

        private MinigameController _controller;
        private RoundController _roundController;
        private BotManager _botManager;
        private List<RoomController> _unlockedRooms = new List<RoomController>();
        private RoomController _playerRoom;
        private List<RoomController> _validRoomsForPlayer = new List<RoomController>();
        public List<RoomController> Rooms => _rooms;
        public MinigameController Controller => _controller;
        public RoomController PlayerRoom => _playerRoom;

        public void Init(MinigameController controller, RoundController roundController, BotManager botManager)
        {
            _controller = controller;
            _roundController = roundController;
            _botManager = botManager;
            _validRoomsForPlayer = _rooms.FindAll(r => r.IsInfrontOfCamera).ToList();
        }

        public void OnPrepare()
        {
            _rooms.ForEach(r => r.Init(this));
            RandomRooms();
        }

        public void OnStart() => _rooms.ForEach(r => r.OnActive());

        public void OnWinRound()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG21_DOOR_CLOSE);
            _playerRoom.TryCloseDoor();
        }

        public RoomController GetValidRoom()
        {
            var validRooms = _unlockedRooms.FindAll(r => r.IsValid());

            if (validRooms.Count == 0) return null;
            return validRooms.RandomItem();
        }

        private void RandomRooms()
        {
            var botLeft = _botManager.BotLeft;
            var groupRequire = _roundController.GroupRequire;
            var maxRoomNeeded = (int)(Mathf.CeilToInt((botLeft - groupRequire) / groupRequire) * 0.8);

            // First room = player room
            HandleRoomForPlayer();

            // Unlocked room
            var rnd = new System.Random();
            var remainingRooms = _rooms.Where(r => !r.IsPlayerRoom).ToList();
            _unlockedRooms = remainingRooms.OrderBy(r => rnd.Next()).Take(maxRoomNeeded).ToList();
            var lockedRooms = remainingRooms.Except(_unlockedRooms).ToList();

            for (var i = 0; i < _unlockedRooms.Count; i++)
            {
                var room = _unlockedRooms[i];
                room.OnPrepare(groupRequire, false, false);
            }

            // Locked room
            for (var i = 0; i < lockedRooms.Count; i++)
            {
                var room = lockedRooms[i];
                room.OnPrepare(groupRequire, true, false);
            }

            void HandleRoomForPlayer()
            {
                _playerRoom = _validRoomsForPlayer.RandomItem();
                _playerRoom.OnPrepare(groupRequire, false, true);
            }
        }
    }
}
