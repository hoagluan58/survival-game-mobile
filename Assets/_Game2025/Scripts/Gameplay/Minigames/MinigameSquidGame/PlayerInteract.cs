using System.Collections;
using System.Collections.Generic;
using SquidGame.LandScape.Core;
using UnityEngine;

namespace SquidGame.LandScape.SquidGame
{
    public class PlayerInteract : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _healFx;
        private PlayerController _playerController;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (_playerController.IsDead()) return;

            if (other.transform.CompareTag("Dead_Zone"))
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_WEAPON_DAMAGE);
                _playerController.Damaged(0);
                _playerController.Die();
                return;
            }

            if (other.transform.CompareTag("Player_Collect"))
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_PICKUP_WEAPON);
                _playerController.Heal(5);
                other.gameObject.SetActive(false);
                _healFx.Play();
            }
        }
    }
}
