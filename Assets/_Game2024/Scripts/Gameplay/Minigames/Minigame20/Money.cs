using UnityEngine;

namespace SquidGame.Minigame20
{
    public class Money : MonoBehaviour
    {
        private bool _isCollected;
        
        public bool IsCollected;

        public void OnSpawn()
        {
            gameObject.SetActive(true);
            _isCollected = false;
        }
        
        
        public void Collect()
        {
            _isCollected = true;
            gameObject.SetActive(false);
        }
    }
}