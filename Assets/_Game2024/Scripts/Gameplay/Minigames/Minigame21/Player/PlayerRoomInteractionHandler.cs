using System.Collections;
using System.Linq;
using UnityEngine;

namespace SquidGame.Minigame21
{
    public class PlayerRoomInteractionHandler : MonoBehaviour
    {
        private RoomManager _roomManager;
        private FollowerController _followerController;
        private RoomController _playerRoom;

        public RoomController PlayerRoom => _playerRoom;
        public FollowerController FollowerController => _followerController;

        public void Init(FollowerController followerController, RoomManager roomManager)
        {
            _followerController = followerController;
            _roomManager = roomManager;
        }

        public void OnPrepare() => _playerRoom = _roomManager.PlayerRoom;

        public void OnEndRound()
        {
            _followerController.Stop();
        }

        public IEnumerator CRReleaseFollowerToRoom()
        {
            var followers = _followerController.FollowedBots;
            _followerController.Stop();
            foreach (var follower in followers)
            {
                MoveToRoomPosition(follower);
            }

            yield return new WaitUntil(() => AllBotReachDestination());


            bool AllBotReachDestination() => followers.All(f => f.Agent.IsAgentReachDestination());
        }

        private void MoveToRoomPosition(Bot follower)
        {
            follower.Follower.StopFollowing();
            var position = _playerRoom.RandomPositionInRoom();
            follower.Agent.SetDestination(position);
            follower.AddBotToRoom();
        }
    }
}
