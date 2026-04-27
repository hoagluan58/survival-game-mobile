using Sirenix.OdinInspector;
using UnityEngine;

namespace SquidGame.Minigame12
{
    public class MinigameController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private BoxCollider _boxCollider;

        [Button]
        public void Test(Vector3 direction, float knockbackForce)
        {
            _rigidbody.velocity = Vector2.zero; // Reset velocity before applying knockback
            _rigidbody.AddForce(direction.normalized * knockbackForce, ForceMode.Impulse); // Apply force
        }
    }
}
