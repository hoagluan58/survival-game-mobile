using SquidGame.Core;
using System.Collections;
using UnityEngine;

namespace SquidGame.Minigame21
{
    public class RoomHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _blockPlayer;

        private MinigameController _controller;
        private RoomController _roomController;
        private int _curCapacity = 0;

        public int CurCapacity
        {
            get => _curCapacity;
            set
            {
                if (value < 0) return;

                _curCapacity = value;
                _roomController.UpdateText(_curCapacity);
            }
        }

        public void Init(MinigameController minigameController, RoomController roomController)
        {
            _controller = minigameController;
            _roomController = roomController;
        }

        public void OnPrepare()
        {
            CurCapacity = 0;
            BlockPlayer(true);
        }

        public void BlockPlayer(bool value)
        {
            _blockPlayer.SetActive(value);
            if (!value)
            {
                _roomController.ToggleOutline(true);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Define.TagName.PLAYER))
            {
                if (other.TryGetComponent<PlayerRoomInteractionHandler>(out var component))
                {
                    HandlePlayerEnterRoom(component);
                }
            }

            if (other.CompareTag(Define.TagName.BOT))
            {
                if (other.TryGetComponent<Bot>(out var component))
                {
                    HandleBotEnterRoom(component);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Define.TagName.PLAYER))
            {
                if (other.TryGetComponent<PlayerRoomInteractionHandler>(out var component))
                {
                    HandlePlayerExitRoom(component);
                }
            }

            if (other.CompareTag(Define.TagName.BOT))
            {
                if (other.TryGetComponent<Bot>(out var component))
                {
                    HandleBotExitRoom(component);
                }
            }
        }

        private void HandlePlayerEnterRoom(PlayerRoomInteractionHandler handler)
        {
            CurCapacity = handler.FollowerController.FollowedBots.Count + 1; // All queue bot + player
            _controller.WinRound();
        }

        private void HandleBotEnterRoom(Bot bot)
        {
            if (_roomController.IsPlayerRoom) return;

            StartCoroutine(CRHandleBotEnterRoom());

            IEnumerator CRHandleBotEnterRoom()
            {
                if (!_roomController.QueueBots.Contains(bot))
                {
                    yield break;
                }
                if (bot.IsInRoom) yield break;

                CurCapacity++;
                bot.AddBotToRoom();
                _roomController.TryCloseDoor();
            }
        }

        private void HandlePlayerExitRoom(PlayerRoomInteractionHandler handler)
        {
            CurCapacity = 0;
        }

        private void HandleBotExitRoom(Bot bot)
        {
            CurCapacity--;
            bot.ExitRoom();
        }
    }
}
