using SquidGame.Core;
using SquidGame.SaveData;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.Gameplay
{
    public class CharacterHat : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _hats;

        private GameObject _currentHat;
        private bool _isPlayer;

        public void Init(bool isPlayer)
        {
            _isPlayer = isPlayer;
            if (_isPlayer)
            {
                // SetHat(UserData.I.CurrentHatId);
                UserData.OnSkinChanged += OnUserSkinChanged;
            }
            else
            {
                var index = Random.Range(0, _hats.Count);
                SetHat(index);
                return;
            }
        }

        private void OnDisable()
        {
            if (_isPlayer)
            {
                UserData.OnSkinChanged -= OnUserSkinChanged;
            }
        }

        public void SetHat(int itemId)
        {
            _hats.ForEach(h => h.gameObject.SetActive(false));
            if (itemId != 0 && itemId <= _hats.Count)
            {
                _currentHat = _hats[itemId - 1];
                _hats[itemId - 1].gameObject.SetActive(true);
            }
        }

        public void SetGreyScale(bool value)
        {
            if (_currentHat != null)
            {
                _currentHat.GetComponent<SkinnedMeshRenderer>().material.SetFloat(Define.MaterialPropertyName.SATURATION, value ? 0f : 1f);
            }
        }

        private void OnUserSkinChanged() => SetHat(UserData.I.CurrentHatId);
    }
}