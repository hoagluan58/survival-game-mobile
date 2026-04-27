using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class EnterCombat : MonoBehaviour
    {
        [SerializeField] private GameObject _gateBlockObject;
        [SerializeField] private EnemyUnit _enemyUnit;
        // [SerializeField] private PlayerController _playerController;

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController _playerController))
            {
                gameObject.SetActive(false);
                _gateBlockObject.SetActive(true);
                _enemyUnit.StartCombat(_playerController.transform);
            }
        }
    }
}
