using System;
using System.Collections.Generic;
using System.Linq;
using NFramework;
using UnityEngine;

namespace SquidGame.Minigame20
{
    public class MoneyManager : MonoBehaviour, IMoneyGetter
    {
        [SerializeField] private List<Money> _moneyList;

        public void OnLoadMinigame()
        {
            UnitController.CarHit += SpawnMoney;
            UnitController.PutMoneyToBase += SpawnMoney;
        }

        private void SpawnMoney(int amount)
        {
            if (amount == 0) return;
            var count = 0;
            foreach (var money in _moneyList.Where(money => !money.gameObject.activeSelf))
            {
                money.OnSpawn();
                count++;
                if (count >= amount)
                {
                    break;
                }
            }
        }

        public Money GetRandomMoney()
        {
            var activeMoney = _moneyList.Where(money => !money.gameObject.activeSelf).ToArray();
            return activeMoney.RandomItem();
        }
    }

    public interface IMoneyGetter
    {
        Money GetRandomMoney();
    }
}