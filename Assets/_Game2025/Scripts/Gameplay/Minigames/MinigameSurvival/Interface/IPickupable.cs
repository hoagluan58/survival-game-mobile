using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape
{
    public interface IPickupable
    {
        public bool IsPickedUp();

        public void EquipWeapon(WeaponEquipment weaponEquipment);

        public void ReplaceWeapon(Func<WeaponType, WeaponEquipment> callback);

    }

    public interface IDamageable
    {
        public bool IsDead();
        public void Damaged(int damageValue);
    }

    public interface IInfomation
    {
        public Transform GetTransform();

        public bool IsPlayer();
    }
}
