using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.Minigame21
{
    public class FollowerController : MonoBehaviour
    {
        private List<Bot> _remainingFollowers;
        private List<Bot> _followedBots;
        private bool _isActive;
        private PlayerRoomInteractionHandler _roomInteractionHandler;

        public List<Bot> FollowedBots => _followedBots;
        public Bot FirstRemainingFollower => _remainingFollowers.FirstOrDefault();

        public void Init(PlayerRoomInteractionHandler roomInteractionHandler)
        {
            _roomInteractionHandler = roomInteractionHandler;
        }

        public void OnPrepare(List<Bot> followerBots)
        {
            _isActive = false;
            _followedBots = new List<Bot>();
            _remainingFollowers = followerBots;
            _roomInteractionHandler.OnPrepare();
        }

        public void OnStart() => _isActive = true;

        public void Stop() => _isActive = false;

        private void Update()
        {
            if (!_isActive) return;

            ScanForFollowers();
            HandleFollowers();
        }

        private void ScanForFollowers()
        {
            for (var i = _remainingFollowers.Count - 1; i >= 0; i--)
            {
                var bot = _remainingFollowers[i];
                var distance = Vector3.Distance(transform.position, bot.transform.position);

                if (distance <= 0.8f)
                {
                    bot.Follower.StartFollowing();
                    _followedBots.Add(bot);
                    _remainingFollowers.RemoveAt(i);
                    if (_remainingFollowers.Count == 0)
                    {
                        _roomInteractionHandler.PlayerRoom.RoomHandler.BlockPlayer(false);
                    }
                }
            }
        }

        private void HandleFollowers()
        {
            foreach (var bot in _followedBots)
            {
                bot.Agent.SetDestination(transform.position + transform.TransformDirection(bot.Follower.Offset));
                bot.Model.Animator.PlayAnimation(bot.Agent.velocity != Vector3.zero ? Gameplay.EAnimStyle.Run : Gameplay.EAnimStyle.Idle);
            }
        }
    }
}
