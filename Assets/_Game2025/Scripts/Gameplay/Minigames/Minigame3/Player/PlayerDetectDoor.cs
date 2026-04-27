using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class PlayerDetectDoor : MonoBehaviour
    {
        private PlayerController _playerController;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Door>(out Door door))
            {
                door.TriggerDoor();
                _playerController.PlayerMovement.DisableComponent();
            }
        }
    }
}
