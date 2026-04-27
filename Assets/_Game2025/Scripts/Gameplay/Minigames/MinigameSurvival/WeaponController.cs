using NFramework;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Survival;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class WeaponController : SingletonMono<WeaponController>
    {
        [SerializeField] private SerializableDictionary<WeaponType, WeaponInfor> _weaponDic = new SerializableDictionary<WeaponType, WeaponInfor>();
        [SerializeField] private Transform _weaponPickupHolder;
        [SerializeField] private LayerMask _obstacleLayer;
        private List<SavePosition> _spawnedPositionList = new List<SavePosition>();

        // private SurvivalManager _survivalManager;

        // public void Init(SurvivalManager survivalManager)
        // {
        //     _survivalManager = survivalManager;
        // }

        [Button]
        public void SpawnAllWeapon(int level = 1)
        {
            WeaponAmountConfig weaponAmountConfig = ConfigController.I.WeaponAmountSO.GetConfig(level);

            foreach (var weaponDetail in weaponAmountConfig.WeaponAmountList)
            {
                for (int i = 0; i < weaponDetail.Amount; i++)
                {
                    SpawnWeapon(weaponDetail.WeaponType);
                }
            }
        }

        public void SpawnWeapon(WeaponType weaponType)
        {
            Vector3 position = PositionController.I.GetRandomPosition(_obstacleLayer, _spawnedPositionList);
            SavePosition weaponSpawnPosition = new SavePosition(position);
            WeaponPickup weaponPickup = Instantiate(_weaponDic[weaponType].WeaponPickUp, position, Quaternion.identity, _weaponPickupHolder);
            weaponPickup.Init(this, weaponSpawnPosition);
            _spawnedPositionList.Add(weaponSpawnPosition);
        }

        public void SpawnWeapon(WeaponType weaponType, Transform spawnTransform)
        {
            WeaponPickup weaponPickup = Instantiate(_weaponDic[weaponType].WeaponPickUp, spawnTransform.position, Quaternion.identity, _weaponPickupHolder);
            weaponPickup.Init(this, null);
        }


        public WeaponEquipment PickupWeapon(WeaponType weaponType, SavePosition position)
        {
            WeaponEquipment weaponEquipment = Instantiate(_weaponDic[weaponType].WeaponEquipment);
            WeaponStatsConfig statsConfig = ConfigController.I.WeaponStatsSO.GetConfig(weaponType);
            weaponEquipment.LoadStats(statsConfig.Damage, statsConfig.Range, statsConfig.DelayTime, statsConfig.ExistTime);
            if (position != null)
            {
                position.IsRemoved = true;
                _spawnedPositionList.Remove(position);
            }
            return weaponEquipment;
        }

        public bool TryGetNeareastSavePosition(Transform transform, out SavePosition currentPosition)
        {
            float minDistance = Mathf.Infinity;
            currentPosition = null;
            foreach (var savePosition in _spawnedPositionList)
            {
                float currentDistance = Vector3.Distance(savePosition.Position, transform.position);
                if (currentDistance >= minDistance) continue;
                minDistance = currentDistance;
                currentPosition = savePosition;
            }
            return currentPosition == null ? false : true;
        }

        public WeaponEquipment GetRandomWeapon()
        {
            WeaponType randomWeaponType = (WeaponType)UnityEngine.Random.Range(1, Enum.GetValues(typeof(WeaponType)).Length);
            return PickupWeapon(randomWeaponType, null);
        }

    }

    public enum WeaponType
    {
        Default,
        Axe,
        Baseball,
        Sword,
        Katana
    }

    [Serializable]
    public class WeaponInfor
    {
        public WeaponEquipment WeaponEquipment;
        public WeaponPickup WeaponPickUp;
    }
}
