using NFramework;
using Redcode.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6.CardGame
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] private Transform _cardHolder;
        [SerializeField] private CardConfigSO _configSO;

        private CardGameController _controller;
        private Card[,] _cards;

        private Dictionary<ECardType, List<Card>> _pools = new Dictionary<ECardType, List<Card>>();

        public void OnEnter(CardGameController controller) => _controller = controller;

        public void OnLevelGen(int rows, int columns)
        {
            TryReturnCardIntoPool();
            _cards = new Card[rows, columns];
        }

        public Card SpawnCard(ECardType cardType, int row, int column)
        {
            var config = _configSO.GetConfig(cardType);
            Card card = null;
            if (_pools.ContainsKey(cardType) && _pools[cardType].Count > 0)
            {
                card = _pools[cardType][0];
                _pools[cardType].RemoveAt(0);
                card.gameObject.SetActive(true);
            }
            else
            {
                card = Instantiate(config.View, _cardHolder);
            }
            _cards[row, column] = card;
            return card;

        }

        public void FlipAllCard(bool isFlipUp)
        {
            foreach (var card in _cards)
            {
                card.Flip(isFlipUp);
            }
        }

        public void CheckWin()
        {
            foreach (var card in _cards)
            {
                if (!card.Data.IsCompleted) return;
            }

            _controller.Win().Forget();
        }

        private void TryReturnCardIntoPool()
        {
            _cardHolder.GetChilds().ForEach(x => x.gameObject.SetActive(false));

            if (_cards == null) return;

            foreach (var card in _cards)
            {
                var type = card.Data.Type;
                if (_pools.ContainsKey(type))
                {
                    _pools[type].Add(card);
                }
                else
                {
                    _pools.Add(type, new List<Card> { card });
                }
                card.gameObject.SetActive(false);
            }
        }
    }
}
