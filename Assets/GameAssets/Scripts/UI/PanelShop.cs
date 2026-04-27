using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SquidGame.Core;

public class PanelShop : PanelBase
{
    /*[SerializeField] private TextMeshProUGUI _tmpBtnBuyEquip;
    [SerializeField] private GameObject _iconCoinBuyEquip;

    private UIItemPlayerSkin[] _uIItemPlayerSkins;
    private TypeBuyEquip _typeBuyEquip;

    private PlayerSkinDefine _skinSelect;
    public UIItemPlayerSkin ItemSkinSelected { get; set; }
    public UIItemPlayerSkin ItemSkinEquipped { get; set; }

    private void Awake()
    {
        _uIItemPlayerSkins = GetComponentsInChildren<UIItemPlayerSkin>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < _uIItemPlayerSkins.Length; i++)
        {
            if (i < PlayerSkinConfig.I.PlayerSkinDefines.Length)
                _uIItemPlayerSkins[i].LoadUI(this, PlayerSkinConfig.I.PlayerSkinDefines[i]);
            else _uIItemPlayerSkins[i].LoadUI(this, null);
        }

        if (ItemSkinSelected == null)
            ItemSkinSelected = _uIItemPlayerSkins[0];
        ItemSkinSelected.Select(true);
        SelectItemSkin(ItemSkinSelected.SkinDefine);
    }

    private void OnDisable()
    {
        EventManager.CloseShop();
    }

    public void SelectItemSkin(PlayerSkinDefine skinDefine)
    {
        _skinSelect = skinDefine;

        if (DataManager.I.GameData.IDSkinPlayerSelected == skinDefine.ID)
        {
            _typeBuyEquip = TypeBuyEquip.Equipped;
            _tmpBtnBuyEquip.text = "Equipped";
            _tmpBtnBuyEquip.margin = new Vector4();

            _iconCoinBuyEquip.SetActive(false);
            //EventManager.PlayerChangeHat(skinDefine.ID);
        }
        else if (DataManager.I.GameData.PlayerSkinUnlocks[skinDefine.ID])
        {
            _typeBuyEquip = TypeBuyEquip.Equip;
            _tmpBtnBuyEquip.text = "Equip";
            _iconCoinBuyEquip.SetActive(false);
            _tmpBtnBuyEquip.margin = new Vector4();
        }
        else
        {
            _tmpBtnBuyEquip.text = Constrains.CoinBuySkinInShop.ToString();
            _typeBuyEquip = TypeBuyEquip.Buy;
            _iconCoinBuyEquip.SetActive(true);
            _tmpBtnBuyEquip.margin = new Vector4(75f, 0f);
        }
    }

    public void Buy_Equip_OnClick()
    {
        if (_typeBuyEquip == TypeBuyEquip.Buy)
        {
            if (GameMaster.I.COIN >= Constrains.CoinBuySkinInShop)
            {
                GameMaster.I.COIN -= Constrains.CoinBuySkinInShop;
                DataManager.I.GameData.PlayerSkinUnlocks[_skinSelect.ID] = true;
                SelectItemSkin(_skinSelect);
            }
        }
        else if (_typeBuyEquip == TypeBuyEquip.Equip)
        {
            DataManager.I.GameData.IDSkinPlayerSelected = _skinSelect.ID;
            DataManager.I.SaveData();

            SelectItemSkin(_skinSelect);

            ItemSkinEquipped?.Equip(false);
            ItemSkinEquipped = ItemSkinSelected;
            ItemSkinEquipped.Equip(true);
        }
        GameSound.I.PlaySFXButtonClick();
    }

    public void WatchAds_OnClick()
    {
        GameMaster.I.COIN += Constrains.CoinWatchAdsInShop;
        GameSound.I.PlaySFXButtonClick();
    }

    public void Close_OnClick()
    {
        gameObject.SetActive(false);
        GameSound.I.PlaySFXButtonClick();
    }*/
}

public enum TypeBuyEquip
{
    Buy,
    Equip,
    Equipped
}
