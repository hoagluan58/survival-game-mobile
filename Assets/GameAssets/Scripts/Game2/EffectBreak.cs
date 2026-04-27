using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game2
{
    public class EffectBreak : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _meshRenderers;

        public void Show(Material material)
        {
            gameObject.SetActive(true);

            Material[] materials = new Material[] { material, material };

            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                _meshRenderers[i].materials = materials;
            }

            Destroy(gameObject, 3f);
        }
    }
}
