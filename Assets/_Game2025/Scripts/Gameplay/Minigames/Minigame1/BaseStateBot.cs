using UnityEngine;

namespace SquidGame.LandScape.Minigame1
{
    public class BaseStateBot : MonoBehaviour
    {
        protected Bot _bot;

        public virtual void Init(Bot bot)
        {
            _bot = bot;
        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnEnter()
        {

        }

        public virtual void OnExit()
        {

        }

        public virtual void OnContinuing()
        {}
    }

}