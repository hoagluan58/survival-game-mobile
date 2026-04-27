using NFramework;
using Redcode.Extensions;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Config;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.LandScape.Game
{
    public class CharacterHair : CharacterComponent
    {
        [System.Serializable]
        public class HairObject
        {
            public int Id;
            public List<MeshRenderer> Meshes = new List<MeshRenderer>();
            public GameObject View;

            public HairObject(int id, List<MeshRenderer> meshes, GameObject view)
            {
                Id = id;
                Meshes = meshes;
                View = view;
            }
        }

        [SerializeField] private List<HairObject> _hairs;

        private HairObject _curHair;

        private void OnEnable() => UserData.OnHairChanged += UserData_OnHairChanged;

        private void OnDisable() => UserData.OnHairChanged -= UserData_OnHairChanged;

        private void UserData_OnHairChanged()
        {
            if (_baseCharacter.IsPlayer)
            {
                var userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
                ChangeHair(userData.UserHair);
            }
        }

        public override void Init(BaseCharacter character)
        {
            base.Init(character);
            UpdatePlayerHair();
        }

        public void ChangeHair(int id)
        {
            foreach (var hair in _hairs)
            {
                hair.View.SetActive(false);
            }

            var correctHair = _hairs.Find(x => x.Id == id);
            if (correctHair != null)
            {
                correctHair.View.SetActive(true);
                _curHair = correctHair;
            }
        }

        private void UpdatePlayerHair()
        {
            if (_baseCharacter.IsPlayer)
            {
                var userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
                ChangeHair(userData.UserHair);
            }

            if (!_baseCharacter.IsPlayer)
            {
                ChangeHair(GameConfig.I.HairConfigs.Keys.GetRandomElement());
            }
        }

        public void SetGreyScale(bool value)
        {
            _curHair.Meshes.ForEach(x =>
            {
                x.material.SetFloat(Define.MaterialPropertyName.SATURATION, value ? 0f : 1.001f);
            });
        }

#if UNITY_EDITOR
        [Header("EDITOR")]
        [SerializeField] private HairConfigSO _hairConfigSO;
        [SerializeField] private LayerMask _layer;
        [Button]
        public void Create()
        {
            transform.DestroyAllChildren();
            _hairConfigSO.Init();
            _hairs.Clear();

            foreach (var config in _hairConfigSO.Configs.Values)
            {
                var obj = Instantiate(config.Model, transform);
                obj.transform.SetAsLastSibling();
                var meshes = obj.GetComponentsInChildren<MeshRenderer>();
                if (meshes != null)
                {
                    var data = new HairObject(config.Id, meshes.ToList(), obj);
                    meshes.ForEach(x => x.gameObject.layer = (int)Mathf.Log(_layer.value, 2));
                    obj.SetActive(false);
                    _hairs.Add(data);
                }
            }
        }
#endif
    }
}
