using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.MinigameRockPaperScissors
{
    [CreateAssetMenu(fileName = "GameAsset", menuName = "Game/GameAsset")]
    public class GameAsset : ScriptableObject
    {
        public List<GameContent> GameContents;

        public Sprite Get(GameResult result) => GameContents.Find(x => x.Result == result).Sprite;
    }

    [System.Serializable]
    public class GameContent
    {
        public GameResult Result;
        public Sprite Sprite;
    }

}
