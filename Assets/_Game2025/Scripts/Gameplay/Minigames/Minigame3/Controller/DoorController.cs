using NFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private List<Door> _doorList;
        private MinigameController _controller;

        public void Init(MinigameController minigameController)
        {
            _controller = minigameController;
            foreach (Door door in _doorList)
            {
                door.Init(minigameController);
            }
        }

        public Door GetRandomDoor()
        {
            Door door;
            do
            {
                door = _doorList.RandomItem();
            } while (!door.CanHaveChar());
            door.SetContain();
            return door;
        }
    }
}
