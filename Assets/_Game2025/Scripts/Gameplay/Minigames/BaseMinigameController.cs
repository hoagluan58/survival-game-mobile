using SquidGame.LandScape.Config;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Data;
using UnityEngine;

namespace SquidGame.LandScape.Game
{
    public abstract class BaseMinigameController : MonoBehaviour
    {
        private MinigameConfig _config;

        public MinigameConfig Config => _config;

        private void Awake() => GameManager.I.MinigameInstance = this;

        public void Init(MinigameConfig config)
        {
            _config = config;
        }

        public virtual void OnLoadMinigame() { }

        public virtual void OnStart()
        {
            var userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
            userData.TryAddPlayedMinigame(_config.Id);
        }

        public virtual void OnWin()
        {

        }

        public virtual void OnLose() { }

        public virtual void OnRevive() { }

        public virtual void OnReload() { }

    }
}
