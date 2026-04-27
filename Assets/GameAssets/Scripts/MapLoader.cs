using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game1
{
    public class MapLoader : MonoBehaviour
    {
        [SerializeField] private AssetReference[] _allMaps;

        private AsyncOperationHandle _asyncOperation;

        private void Awake()
        {
            int count = PlayerPrefs.GetInt("Count_Play", 0);

            AssetReference asset = _allMaps[count % _allMaps.Length];

            _asyncOperation = Addressables.LoadSceneAsync(asset, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            PlayerPrefs.SetInt("Count_Play", count + 1);
        }

        private void OnDisable()
        {
            Addressables.Release(_asyncOperation);
        }
    }
}