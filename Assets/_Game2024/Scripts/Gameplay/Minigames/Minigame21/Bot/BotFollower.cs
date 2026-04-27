using UnityEngine;

namespace SquidGame.Minigame21
{
    public class BotFollower : MonoBehaviour
    {
        private bool _isFollower;
        private bool _isFollowing;
        private Bot _bot;
        private Vector3 _offset;

        public Vector3 Offset => _offset;
        public bool IsFollower => _isFollower;

        public void Init(Bot bot)
        {
            _bot = bot;
            _isFollowing = false;
        }

        public void ToggleFollower(bool value) => _isFollower = value;

        public void OnStart() => _bot.Model.ToggleOutline(_isFollower);

        public void StartFollowing()
        {
            _isFollowing = true;
            _bot.Model.ToggleOutline(false);
            _offset = RandomOffset();
        }

        public void StopFollowing()
        {
            _isFollowing = false;
            _bot.Agent.ResetPath();
        }

        private Vector3 RandomOffset()
        {
            var rndDistance = Random.Range(0.8f, 2.2f);
            var rndSideOffset = Random.Range(-1.2f, 1.2f);

            var offset = new Vector3(rndSideOffset, 0, -rndDistance);
            return offset;
        }
    }
}
