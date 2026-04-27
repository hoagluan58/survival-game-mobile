using System.Collections;
using System.Collections.Generic;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Minigame4;
using UnityEngine;

namespace SquidGame.LandScape.SquidGame
{
    public class MapElementHandler : MonoBehaviour
    {
        [SerializeField] private Transform _startPoint, _endPoint, _leftWeaponSpawnTransorm, _rightWeaponSpawnTransform;
        [SerializeField] private SpikeRow _spikeRow;
        [SerializeField] private List<Guard> _guards;

        public void OnLoadMinigame(int spikeRowAmount)
        {
            float moveDuration = 5f;
            for (int i = 0; i < spikeRowAmount; i++)
            {
                if (i >= 4)
                    moveDuration -= 0.5f;

                Vector3 spawnPosition = Vector3.Lerp(_startPoint.position, _endPoint.position, i / (float)spikeRowAmount);
                Instantiate(_spikeRow, spawnPosition, Quaternion.identity, transform).InitMoveDuration(moveDuration);
            }

            WeaponController.I.SpawnWeapon((WeaponType)Random.Range((int)WeaponType.Axe, (int)WeaponType.Katana + 1), _leftWeaponSpawnTransorm);
            WeaponController.I.SpawnWeapon((WeaponType)Random.Range((int)WeaponType.Axe, (int)WeaponType.Katana + 1), _rightWeaponSpawnTransform);
        }

        public void OnTimeout(Transform playerHead)
        {
            Guard closestGuard = null;
            float closestDistance = float.MaxValue;

            foreach (Guard guard in _guards)
            {
                float distance = Vector3.Distance(playerHead.position, guard.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestGuard = guard;
                }
            }

            closestGuard.LookAtTarget(playerHead)
                .PlayAnimationFire(playerHead, () => GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT))
                .OnShootCompleted(() =>
                {
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
                });

        }
    }
}
