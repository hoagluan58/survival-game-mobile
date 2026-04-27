using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SquidGame.LandScape.Minigame5
{
    public class PanelInGame : PanelBase
    {
        [SerializeField] private UIPowerBar _uIPowerBar;
        [SerializeField] private GameObject _warningObject;
        [SerializeField] private TextMeshProUGUI _tmpTimeCounter;

        public UIPowerBar UIPowerBar { get { return _uIPowerBar; } }


        public void SetActiveWarning(bool b)
        {
            _warningObject.SetActive(b);
        }

        public void SetTimeCounter(string str)
        {
            _tmpTimeCounter.text = str;
        }
    }
}