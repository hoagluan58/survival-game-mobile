using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SquidGame.Core;

public class UIItemPlayerSkin : MonoBehaviour
{
    /*[SerializeField] private GameObject _selected;
    [SerializeField] private GameObject _equipped;
    [SerializeField] private GameObject _unavailable;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _tmpName;

    private PlayerSkinDefine _skinDefine;
    private PanelShop _panelShop;

    public PlayerSkinDefine SkinDefine { get { return _skinDefine; } }

    public void LoadUI(PanelShop panelShop, PlayerSkinDefine define)
    {
        _panelShop = panelShop;
        if (define == null)
        {
            _selected.SetActive(false);
            _equipped.SetActive(false);
            _unavailable.SetActive(true);
            _icon.gameObject.SetActive(false);
            _tmpName.text = "Locked";
            return;
        }
        _unavailable.SetActive(false);

        _skinDefine = define;

        _tmpName.text = _skinDefine.Name;
        _icon.sprite = define.Icon;
        _icon.gameObject.SetActive(true);

        if (DataManager.I.GameData.IDSkinPlayerSelected == _skinDefine.ID)
        {
            _equipped.SetActive(true);
            _selected.SetActive(true);
            _panelShop.ItemSkinSelected = this;
            _panelShop.ItemSkinEquipped = this;
        }
        else
        {
            _equipped.SetActive(false);
            _selected.SetActive(false);
        }
    }

    public void Select(bool b)
    {
        _selected.SetActive(b);
        EventManager.PlayerChangeHat(_skinDefine.ID);
    }

    public void Equip(bool b)
    {
        _equipped.SetActive(b);
    }

    public void OnClick()
    {
        if (_skinDefine != null)
        {
            _panelShop.ItemSkinSelected?.Select(false);
            _panelShop.ItemSkinSelected = this;
            _panelShop.ItemSkinSelected.Select(true);
            _panelShop.SelectItemSkin(_skinDefine);
        }
        GameSound.I.PlaySFXButtonClick();
    }*/
}
