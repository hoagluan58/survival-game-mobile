using NFramework;
using RotaryHeart.Lib.SerializableDictionary;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6.CardGame
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header("CONFIG")]
        [SerializeField] private Vector3 _firstCardPos;
        [SerializeField] private float _distanceBetweenCol;
        [SerializeField] private float _distanceBetweenRow;
        [SerializeField] private LevelConfig _levelConfig;

        public LevelConfig LevelConfig => _levelConfig;

        private CardManager _cardManager;

        [Button]
        public void GenLevel(CardManager cardManager)
        {
            _cardManager = cardManager;

            var rows = _levelConfig.Rows;
            var columns = _levelConfig.Columns;
            var cardPool = new List<ECardType>();

            _cardManager.OnLevelGen(rows, columns);

            // Random cards 
            foreach (var pair in _levelConfig.CardDic)
            {
                var type = pair.Key;
                var amount = pair.Value;
                for (var i = 0; i < amount; i++)
                {
                    cardPool.Add(type);
                }
            }

            cardPool.Shuffle();

            // Spawn card
            var cardIndex = 0;
            for (int i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    var type = cardPool[cardIndex];

                    var card = _cardManager.SpawnCard(type, i, j);
                    card.Init(new CardData(type));
                    card.transform.localPosition = GetCardPos();
                    cardIndex++;

                    Vector3 GetCardPos()
                    {
                        var xPos = _firstCardPos.x + j * _distanceBetweenCol;
                        var yPos = _firstCardPos.y;
                        var zPos = _firstCardPos.z + i * _distanceBetweenRow;
                        return new Vector3(xPos, yPos, zPos);
                    }
                }
            }
        }
    }

    [Serializable]
    public class LevelConfig
    {
        public int Columns;
        public int Rows;
        public float Time;
        public SerializableDictionaryBase<ECardType, int> CardDic;
    }
}
