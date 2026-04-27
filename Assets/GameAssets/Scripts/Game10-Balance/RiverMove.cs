using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game10
{
    public class RiverMove : MonoBehaviour
    {
        [SerializeField] private float _speedMove = 1f;

        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            _meshRenderer.material.SetTextureOffset("_BaseMap", Vector2.up * Time.time * _speedMove);
        }
    }
}