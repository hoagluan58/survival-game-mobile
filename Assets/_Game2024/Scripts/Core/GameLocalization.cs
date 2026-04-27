using NFramework;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;

namespace SquidGame.Core
{
    public class GameLocalization : SingletonMono<GameLocalization>, ISaveable
    {
        [SerializeField] private SerializedDictionary<string, Sprite> _flagSpriteDic;
        [SerializeField] private SaveData _saveData;

#if UNITY_EDITOR
        [Button]
        private void LoadFlagSprite()
        {
            _flagSpriteDic = new SerializedDictionary<string, Sprite>();
            foreach (var locale in GetAvailableLocales())
            {
                var sprite = FileUtils.LoadFirstAssetWithName<Sprite>($"{locale.Identifier.Code}_flag");
                _flagSpriteDic.Add(locale.Identifier.Code, sprite);
            }
        }
#endif

        public Sprite GetFlagSprite(string key)
        {
            Sprite sprite = null;
            _flagSpriteDic.TryGetValue(key, out sprite);
            return sprite;
        }

        public Locale UserLocale => LocalizationSettings.SelectedLocale;

        public void ChangeLanguage(Locale locale)
        {
            _saveData.IdentifierCode = locale.Identifier.Code;
            LocalizationSettings.SelectedLocale = locale;
            DataChanged = true;
        }

        public List<Locale> GetAvailableLocales() => LocalizationSettings.AvailableLocales.Locales;

        public string GetStringFromTable(string key, params object[] args) => LocalizationSettings.StringDatabase.GetLocalizedString(key, arguments: args);

        #region ISaveable

        [Serializable]
        public class SaveData
        {
            public string IdentifierCode;
        }

        public string SaveKey => "GameLocalization";

        public bool DataChanged { get; set; }

        public object GetData() => _saveData;

        public void SetData(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                _saveData = new SaveData()
                {
                    IdentifierCode = LocalizationSettings.ProjectLocale.Identifier.Code
                };
                DataChanged = true;
            }
            else
            {
                _saveData = JsonUtility.FromJson<SaveData>(data);
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(_saveData.IdentifierCode);
            }
        }

        public void OnAllDataLoaded()
        {
        }

        #endregion
    }
}
