using NFramework;
using RotaryHeart.Lib.SerializableDictionary;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Game
{
    public class CharacterBodySkin : CharacterComponent
    {
        [Header("BODY")]
        [SerializeField] private List<SkinnedMeshRenderer> _meshes;
        [SerializeField] private SerializableDictionaryBase<ESkinName, Material> _skins;

        [Header("FACE")]
        [SerializeField] private SkinnedMeshRenderer _faceMesh;
        [SerializeField] private SerializableDictionaryBase<EFaceName, Material> _faceMats;
        public override void Init(BaseCharacter character)
        {
            base.Init(character);
            if (_baseCharacter.IsPlayer)
            {
                ChangeSkin(ESkinName.Green);
            }
            else
            {
                var skinList = new List<ESkinName>(_skins.Keys);
                skinList.Remove(ESkinName.Green);
                ChangeSkin(skinList.RandomItem());
            }
        }

        public void ChangeSkin(ESkinName skinName)
        {
            if (_skins.TryGetValue(skinName, out var material))
            {
                foreach (var mesh in _meshes)
                {
                    mesh.material = material;
                }
            }
        }

        public void ChangeFace(EFaceName faceName)
        {
            if (_faceMats.TryGetValue(faceName, out var material))
            {
                _faceMesh.material = material;
            }
        }

        public void SetGreyScale(bool value)
        {
            _meshes.ForEach(x => x.material.SetFloat(Define.MaterialPropertyName.SATURATION, value ? 0f : 1.001f));
            _faceMesh.material.SetFloat(Define.MaterialPropertyName.SATURATION, value ? 0f : 1.001f);
        }

#if UNITY_EDITOR
        [Button]
        public void Sync()
        {
            _skins = new();
            foreach (ESkinName skinName in Enum.GetValues(typeof(ESkinName)))
            {
                _skins[skinName] = FileUtils.LoadFirstAssetWithName<Material>($"M_Skin_M_{skinName}");
            }

            _faceMats = new();
            foreach (EFaceName faceName in Enum.GetValues(typeof(EFaceName)))
            {
                _faceMats[faceName] = FileUtils.LoadFirstAssetWithName<Material>($"M_Face_{faceName}");
            }
        }
#endif

        public enum ESkinName
        {
            Blue,
            Dark_Blue,
            Green,
            Orange,
            Red,
        }

        public enum EFaceName
        {
            Normal,
            Happy,
            Sad
        }
    }
}
