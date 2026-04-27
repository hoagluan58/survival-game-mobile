using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Game11
{
    public class PanelInGame : PanelBase
    {
        [SerializeField] private GameObject _warning;
        [SerializeField] private TextMeshProUGUI _tmpTimeCounter;

        public void ShowWarning()
        {
            _warning.gameObject.SetActive(true);
        }

        public void SetTimeCounter(string str)
        {
            _tmpTimeCounter.text = str;
        }
    }
}