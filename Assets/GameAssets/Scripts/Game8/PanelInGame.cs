using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Game8
{
    public class PanelInGame : PanelBase
    {
        [SerializeField] private TextMeshProUGUI _tmpTimeCounter;

        public void SetTimeCounter(string str)
        {
            _tmpTimeCounter.text = str;
        }
    }
}