using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.Gameplay
{
    public class CharacterRagdoller : MonoBehaviour
    {
        [SerializeField] private List<Rigidbody> _rigidbodies;

        public void ToggleRagdoll(bool value)
        {
            _rigidbodies.ForEach(x =>
            {
                x.useGravity = value;
                x.isKinematic = !value;
            });
        }

        public void ApplyForce(Vector3 direction, float forceMagnitude)
        {
            foreach (var rb in _rigidbodies)
            {
                rb.AddForce(direction.normalized * forceMagnitude, ForceMode.Impulse);
            }
        }
    }
}
