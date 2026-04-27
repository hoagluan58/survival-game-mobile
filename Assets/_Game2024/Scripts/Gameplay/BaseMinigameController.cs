using NFramework;
using SquidGame.Config;
using SquidGame.Core;
using SquidGame.UI;
using System;
using UnityEngine;

namespace SquidGame.Gameplay
{
    public abstract class BaseMinigameController : MonoBehaviour
    {
        public MinigameConfig Config;
        protected bool _canRevive;
        protected bool _isBoosterAvailable;

        private void Awake() => GameManager.I.MinigameInstance = this;

        public void Init(MinigameConfig config)
        {
            Config = config;
            _canRevive = Config.CanRevive;
            _isBoosterAvailable = Config.IsBooster;
        }

        public virtual void OnLoadMinigame()
        {
            UIManager.I.Close(Define.UIName.GAMEPLAY_POPUP);
            UIManager.I.Open(Define.UIName.GAMEPLAY_POPUP);
        }

        public virtual void OnStart() => UIManager.I.Close(Define.UIName.GAMEPLAY_POPUP);

        public virtual void OnWin() { }

        public virtual void OnLose() { }

        public virtual void OnRevive() { }

        public virtual void OnUseBooster() => _isBoosterAvailable = false;

        public void TryShowRevivePopup(Action onRevive = null, Action onNoThanks = null)
        {
            if (_canRevive)
            {
                _canRevive = false;
                UIManager.I.Open<RevivePopupUI>(Define.UIName.REVIVE_POPUP).SetData(onRevive, onNoThanks);
            }
            else
            {
                onNoThanks?.Invoke();
            }
        }
    }
}