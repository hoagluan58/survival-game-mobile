using UnityEngine;

namespace SquidGame.Minigame20
{
    public class UnitBase : MonoBehaviour
    {
        [SerializeField] private string _key;

        private int _currentMoney;

        public bool PutMoneyIn(int amount, string key)
        {
            if (key != _key) return false;

            _currentMoney += amount * 1000;
            return true;
        }
    }
}