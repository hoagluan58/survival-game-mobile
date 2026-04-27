using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame2
{
    public class GlassFracture : MonoBehaviour
    {
        [SerializeField] private List<Rigidbody> _fractures;
        [SerializeField] private float _valueForce;

        private void Start()
        {
            foreach (var fracture in _fractures)
            {
                var rnd = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f));
                rnd.Normalize();
                fracture.AddForce(rnd * _valueForce);
            }
        }
    }
}
