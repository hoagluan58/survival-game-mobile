using UnityEngine;

namespace SquidGame.LandScape.Minigame2
{
    public class CollidableType : MonoBehaviour
    {
        public enum EObjectType
        {
            GlassPanel,
            TriggerDeath,
            DanceZone,
            TriggerFalling,
            TriggerWin,
        }

        public EObjectType Type;
    }
}
