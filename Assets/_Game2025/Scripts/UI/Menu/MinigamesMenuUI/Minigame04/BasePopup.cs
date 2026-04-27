using SquidGame.LandScape.Game;
using UnityEngine;

namespace SquidGame.LandScape.Minigame4
{
    public abstract class BasePopup : MonoBehaviour
    {
        [SerializeField] protected EGameMode _gameMode;
        public virtual BasePopup SetGameMode(EGameMode gameMode)
        {
            _gameMode = gameMode;
            return this;
        }
        public virtual void OnInitialize() { }
        public virtual void OnShow() { }
        public virtual void OnHide() { }

    }
}
