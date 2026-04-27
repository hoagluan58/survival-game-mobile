using Old;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHat : MonoBehaviour
{
    [SerializeField] private GameObject[] _allHats;

    private void Start()
    {
        if (DataManager.I)
        {
            int id = DataManager.I.GameData.IDSkinPlayerSelected;
            if (id >= 0 && id < _allHats.Length)
            {
                _allHats[id].gameObject.SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        EventManager.OnChangeHat += OnChangeHat;
        EventManager.OnCloseShop += OnHideShop;
    }

    private void OnDisable()
    {
        EventManager.OnChangeHat -= OnChangeHat;
        EventManager.OnCloseShop -= OnHideShop;
    }

    private void OnChangeHat(int id)
    {
        HideAll();
        if (id >= 0 && id < _allHats.Length)
        {
            _allHats[id].gameObject.SetActive(true);
        }
    }

    private void OnHideShop()
    {
        HideAll();
        Start();
    }

    private void HideAll()
    {
        for (int i = 0; i < _allHats.Length; i++)
        {
            _allHats[i].gameObject.SetActive(false);
        }
    }
}
