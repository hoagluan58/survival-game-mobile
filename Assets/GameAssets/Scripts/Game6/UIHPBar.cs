using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game6
{
    public class UIHPBar : MonoBehaviour
    {
        [SerializeField] private Image _fill;

        public void UpdateUI(float val)
        {
            _fill.fillAmount = val;
            if (val <= 0.25f)
                _fill.color = Color.red;
            else _fill.color = Color.green;
        }
    }
}
