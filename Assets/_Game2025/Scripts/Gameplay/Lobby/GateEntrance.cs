using System.Collections;
using UnityEngine;

namespace SquidGame.LandScape.Lobby
{
    public class GateEntrance : MonoBehaviour
    {
        [SerializeField] private LobbyManager _lobbyManager;
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _lobbyManager.PlayNextMinigame();
                gameObject.SetActive(false);
            }
        }

    }
}
