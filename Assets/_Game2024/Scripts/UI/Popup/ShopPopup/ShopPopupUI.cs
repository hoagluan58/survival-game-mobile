using System.Collections.Generic;
using System.Linq;
using Animancer;
using NFramework;
using SquidGame.Config;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.SaveData;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace SquidGame.UI
{
    public class ShopPopupUI : BaseUIView
    {
        [Header("SCENE REFERENCES")]
        [SerializeField] private Button _closeButton;

        [Header("CHARACTER PANEL")]
        [SerializeField] private BaseCharacter _baseCharacter;
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private AnimationClip _idleAnim;

        [Header("SHOP PANEL")]
        [SerializeField] private HorizontalScrollSnap _horizontalScrollSnap;
        [SerializeField] private Transform _shopItemGroupContainer;
        [SerializeField] private List<GameObject> _pageNodes;

        [Header("BUTTONS PANEL")]
        [SerializeField] private Button _equipButton;
        [SerializeField] private Button _equippedButton;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _watchAdsButton;

        [Header("ASSETS REFERENCES")]
        [SerializeField] private GroupShopItemUI _shopItemGroupPrefab;
        [SerializeField] private ShopItemUI _shopItemUIPrefab;

        // References
        private HatConfigSO _playerSkinConfig;

        // Internal
        private ShopItemUI _shopItemSelect;
        private readonly List<ShopItemUI> _shopItems = new();
        private readonly List<GroupShopItemUI> _groupItems = new();

        private bool _isFirstOpen;

        private void OnEnable()
        {
            ShopItemUI.Clicked += OnShopItemUIClick;
            _closeButton.onClick.AddListener(OnCloseButtonClick);
            _equipButton.onClick.AddListener(OnEquipButtonClick);
            _buyButton.onClick.AddListener(OnBuyButtonClick);
            _watchAdsButton.onClick.AddListener(OnWatchAdsButtonClick);
        }

        private void OnDisable()
        {
            ShopItemUI.Clicked -= OnShopItemUIClick;
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);
            _equipButton.onClick.RemoveListener(OnEquipButtonClick);
            _buyButton.onClick.RemoveListener(OnBuyButtonClick);
            _watchAdsButton.onClick.RemoveListener(OnWatchAdsButtonClick);
        }

        private void SpawnGroup(int skinCount)
        {
            var groupCount = Mathf.CeilToInt(skinCount / 6f);
            for (var i = 0; i < groupCount; i++)
            {
                var newGroup = Instantiate(_shopItemGroupPrefab, _shopItemGroupContainer);
                _groupItems.Add(newGroup);
            }
            for (var i = 0; i < _groupItems.Count; i++)
            {
                _pageNodes[i].gameObject.SetActive(true);
            }
        }

        private void SpawnShopItems(HatConfig[] skinConfigs)
        {
            var groupIndex = 0;
            var itemCountInGroup = 0;

            foreach (var cfg in skinConfigs)
            {
                var currentGroup = _groupItems[groupIndex];

                var shopItem = Instantiate(_shopItemUIPrefab, currentGroup.transform);
                shopItem.Init(cfg.Id, cfg.Icon);
                _shopItems.Add(shopItem);

                itemCountInGroup++;
                if (itemCountInGroup >= 6)
                {
                    groupIndex++;
                    itemCountInGroup = 0;
                }
            }
        }

        public override void OnOpen()
        {
            if (!_isFirstOpen)
            {
                _isFirstOpen = true;
                var skinConfigs = ConfigManager.I.PlayerSkinConfig.Config.Values.ToArray();
                SpawnGroup(skinConfigs.Length);
                SpawnShopItems(skinConfigs);
            }

            _animancer.Play(_idleAnim);
            _shopItemSelect = _shopItems.FirstOrDefault(item => item.ItemId == UserData.I.CurrentHatId);
            UpdateItemsState();
            this.InvokeDelay(0.1f, ScrollToCurrentItem);
        }

        private void ScrollToCurrentItem()
        {
            var currentItemIndex = _shopItems.IndexOf(_shopItemSelect);
            var page = currentItemIndex / 6;
            _horizontalScrollSnap.GoToScreen(page, true);
        }

        private void UpdateButtons(ShopItemUI selectItem)
        {
            if (selectItem == null) return;

            var isEquip = UserData.I.CurrentHatId == selectItem.ItemId;
            var isUnlocked = UserData.I.IsSkinUnlocked(selectItem.ItemId);
            var canNotBuy = ConfigManager.I.PlayerSkinConfig.Config[selectItem.ItemId].CanNotBuy;

            _buyButton.gameObject.SetActive(!isUnlocked && !canNotBuy);
            _watchAdsButton.gameObject.SetActive(!isUnlocked && !canNotBuy);
            _equipButton.gameObject.SetActive(!isEquip && isUnlocked);
            _equippedButton.gameObject.SetActive(isEquip && isUnlocked);
        }

        private void OnShopItemUIClick(ShopItemUI shopItemUIClicked)
        {
            GameSound.I.PlaySFXButtonClick();
            _shopItemSelect = shopItemUIClicked;
            UpdateItemsState();
            PreviewItem(shopItemUIClicked.ItemId);
        }

        private void UpdateItemsState()
        {
            foreach (var shopitem in _shopItems)
            {
                var isEquip = UserData.I.CurrentHatId == shopitem.ItemId;
                var isUnlocked = UserData.I.IsSkinUnlocked(shopitem.ItemId);
                var isSelected = shopitem == _shopItemSelect;
                shopitem.ChangeState(isSelected, isEquip, isUnlocked);
            }
            UpdateButtons(_shopItemSelect);
        }

        private void PreviewItem(int itemId) => _baseCharacter.Hat.SetHat(itemId);

        private void OnBuyButtonClick()
        {
            if (UserData.I.Coin >= 3000)
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_BUY_ITEM);
                UserData.I.Coin -= 3000;
                UserData.I.UnlockSkin(_shopItemSelect.ItemId);
                UpdateItemsState();
            }
            else
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG14_WRONG);
            }
        }

        private void OnWatchAdsButtonClick()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_BUY_ITEM);
            UserData.I.UnlockSkin(_shopItemSelect.ItemId);
            UpdateItemsState();
        }

        private void OnEquipButtonClick()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_EQUIP_ITEM);
            UserData.I.ChangeHat(_shopItemSelect.ItemId);
            UpdateItemsState();
        }

        private void OnCloseButtonClick()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Close(this);
        }
    }
}