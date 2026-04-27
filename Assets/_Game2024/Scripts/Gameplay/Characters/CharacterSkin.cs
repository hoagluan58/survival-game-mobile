using NFramework;
using SquidGame.Core;
using System.Linq;
using UnityEngine;

namespace SquidGame.Gameplay
{
    public class CharacterSkin : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _bodyMeshRenderer;
        [SerializeField] private SerializableDictionary<ESkinName, Material> _materials;

        private Material _material;
        private bool _isPlayer;

        public void Init(bool isPlayer)
        {
            _isPlayer = isPlayer;
            _material = GetMaterial();
            SetMaterial(_material);
        }

        public void SetMaterial(Material material)
        {
            _bodyMeshRenderer.material = material;
        }

        public void SetGreyScale(bool value)
        {
            _bodyMeshRenderer.material.SetFloat(Define.MaterialPropertyName.SATURATION, value ? 0f : 1f);
        }

        private Material GetMaterial()
        {
            if (_isPlayer) return _materials[ESkinName.Base];
            else
            {
                var rndSkin = _materials.Keys.ToList();
                rndSkin.Remove(ESkinName.Base);
                return _materials[rndSkin.RandomItem()];
            }
        }

        public enum ESkinName
        {
            Base,
            Blue,
            DeepOrange,
            Ocean,
            Orange,
            Red,
        }
    }
}
