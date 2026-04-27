using System;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6.CardGame
{
    public class CardConfigSO : ScriptableObject
    {
        public List<CardConfig> Configs;

        public CardConfig GetConfig(ECardType Type) => Configs.Find(x => x.Type == Type);
    }

    [Serializable]
    public class CardConfig
    {
        public ECardType Type;
        public Card View;
    }

    public enum ECardType
    {
        Red,
        Green,
        Blue,
        Yellow,
    }
}
