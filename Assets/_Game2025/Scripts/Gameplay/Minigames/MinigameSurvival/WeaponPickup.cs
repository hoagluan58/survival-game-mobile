using SquidGame.LandScape.Core;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private bool _needRespawnAfterPickup = true;
        public WeaponType WeaponType => _weaponType;

        private SavePosition _weaponPosition;
        [SerializeField] private WeaponController _weaponController;

        public void Init(WeaponController weaponController, SavePosition position)
        {
            _weaponController = weaponController;
            _weaponPosition = position;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.TryGetComponent(out IPickupable component))
            {
                IInfomation infomation = collision.GetComponent<IInfomation>();
                WeaponEquipment weaponEquipment = null;
                if (component.IsPickedUp())
                {
                    component.ReplaceWeapon((weaponType) =>
                    {
                        if (infomation.IsPlayer())
                            GameSound.I.PlaySFX(Define.SoundPath.SFX_PICKUP_WEAPON);
                        gameObject.SetActive(false);

                        if (_needRespawnAfterPickup)
                            _weaponController.SpawnWeapon(weaponType);

                        return _weaponController.PickupWeapon(_weaponType, _weaponPosition);
                    });
                }
                else
                {
                    if (infomation.IsPlayer())
                        GameSound.I.PlaySFX(Define.SoundPath.SFX_PICKUP_WEAPON);
                    gameObject.SetActive(false);
                    weaponEquipment = _weaponController.PickupWeapon(_weaponType, _weaponPosition);
                    component.EquipWeapon(weaponEquipment);
                }
            }
        }
    }
}
