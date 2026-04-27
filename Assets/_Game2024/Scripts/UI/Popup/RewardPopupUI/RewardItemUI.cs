using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class RewardItemUI : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _amountTMP;

        public void SetData(Sprite icon, int amount)
        {
            _icon.sprite = icon;
            _amountTMP.gameObject.SetActive(amount > 0);
            if (amount > 0)
            {
                _amountTMP.text = $"+{amount}";
            }
        }
    }
}
