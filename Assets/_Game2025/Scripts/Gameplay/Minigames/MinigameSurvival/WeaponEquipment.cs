using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class WeaponEquipment : MonoBehaviour
    {
        private (int Damage, float Range, float DelayTime, float Duration) _stats;
        public (int Damage, float Range, float DelayTime, float Duration) Stats => _stats;

        [SerializeField] private WeaponType _weaponType;
        public WeaponType WeaponType => _weaponType;

        [SerializeField] private AnimancerComponent _animancer;
        public AnimancerComponent Animancer => _animancer;
        [SerializeField] private CombatHandler _combatHandler;
        public CombatHandler CombatHandler => _combatHandler;

        public void LoadStats(int damage, float range, float delayTime, float duration)
        {
            _stats.Damage = damage;
            _stats.Range = range;
            _stats.DelayTime = delayTime;
            _stats.Duration = duration;
        }
    }
}
