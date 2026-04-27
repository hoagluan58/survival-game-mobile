using System;
using DG.Tweening;
using UnityEngine;

namespace SquidGame.Minigame20
{
    public class UnitController : MonoBehaviour
    {
        public static event Action<int> CarHit;
        public static event Action<int> PutMoneyToBase;
        
        [SerializeField] private string _key;

        private int _currentMoneyHolding;
        private bool _isInvincible;

        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Dead_Zone")) OnHitCar();
            if (other.CompareTag("Player_Collect")) OnHitMoney(other);
            if (other.CompareTag("Check_Point")) OnHitBase(other);
        }

        protected virtual void OnHitCar()
        {
            if (_isInvincible) return;
            SetInvincible(5);
            CarHit?.Invoke(_currentMoneyHolding);
            _currentMoneyHolding = 0;
        }

        protected virtual void OnHitMoney(Collider moneyCollider)
        {
            if (_currentMoneyHolding >= 3) return;
            var money = moneyCollider.GetComponent<Money>();
            if (money == null) return;
            money.Collect();
            _currentMoneyHolding++;
        }

        protected virtual void OnHitBase(Collider baseCollider)
        {
            var unitBase = baseCollider.GetComponent<UnitBase>();
            if (unitBase == null) return;
            if (unitBase.PutMoneyIn(_currentMoneyHolding, _key))
            {
                PutMoneyToBase?.Invoke(_currentMoneyHolding);
                _currentMoneyHolding = 0;
            }
        }

        protected virtual void SetInvincible(float seconds)
        {
            _isInvincible = true;
            DOVirtual.DelayedCall(seconds, RemoveInvincible);
        }

        protected virtual void RemoveInvincible()
        {
            _isInvincible = false;
        }
    }
}