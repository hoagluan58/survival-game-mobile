using System;
using Redcode.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class ShopItemUI : MonoBehaviour
    {
        public static event Action<ShopItemUI> Clicked;

        [SerializeField] private Button _button;
        [SerializeField] private Image _itemImage;
        [SerializeField] private Image _noneImage;
        [SerializeField] private Image[] _lockImages;
        [SerializeField] private Image _selectedImage;
        [SerializeField] private Image _equipImage;

        private int _itemId;

        public int ItemId => _itemId;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnItemClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnItemClick);
        }

        public void Init(int id, Sprite skinIcon)
        {
            _itemId = id;
            _noneImage.enabled = skinIcon == null;
            _itemImage.enabled = skinIcon != null;
            _itemImage.sprite = skinIcon;
        }

        public void ChangeState(bool isSelect, bool isEquip, bool isUnlocked)
        {
            _selectedImage.gameObject.SetActive(isSelect);
            _lockImages.ForEach(i => i.gameObject.SetActive(!isUnlocked));
            _equipImage.gameObject.SetActive(isEquip);
        }

        private void OnItemClick()
        {
            Clicked?.Invoke(this);
        }
    }
}