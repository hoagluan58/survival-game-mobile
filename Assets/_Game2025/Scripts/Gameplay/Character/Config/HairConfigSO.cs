using NFramework;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SquidGame.LandScape.Config
{
    public class HairConfigSO : GoogleSheetConfigSO<HairConfig>
    {
        private Dictionary<int, HairConfig> _configs;
        public Dictionary<int, HairConfig> Configs => _configs;

        public void Init()
        {
            _configs = new Dictionary<int, HairConfig>();
            foreach (var config in _datas)
            {
                _configs.Add(config.Id, config);
            }
        }

        public HairConfig GetDefaultHair() => _datas.FirstOrDefault(x => x.IsDefault);

        public HairConfig GetConfig(int id) => _datas.Find(x => x.Id == id);

#if UNITY_EDITOR

        [SerializeField] private string _renderTexturePath;

        protected override void OnSynced(List<HairConfig> googleSheetData)
        {
            base.OnSynced(googleSheetData);

            foreach (var data in _datas)
            {
                data.Model = FileUtils.LoadFirstAssetWithName<GameObject>(data.Name);
                data.ShopRenderTexture = CreateRenderTexture($"SHOP_TEXTURE_{data.Id}");
            }

            EditorUtility.SetDirty(this);
        }

        private RenderTexture CreateRenderTexture(string name)
        {
            var renderTexture = new RenderTexture(512, 512, 0)
            {
                name = name,
                dimension = UnityEngine.Rendering.TextureDimension.Tex2D,
            };
            renderTexture.Create();

            var path = $"{_renderTexturePath}/{name}.renderTexture";
            AssetDatabase.CreateAsset(renderTexture, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return renderTexture;
        }
#endif
    }

    [System.Serializable]
    public class HairConfig
    {
        public int Id;
        public GameObject Model;
        public bool IsDefault;
        public RenderTexture ShopRenderTexture;

        [HideInInspector] public string Name;
    }
}
