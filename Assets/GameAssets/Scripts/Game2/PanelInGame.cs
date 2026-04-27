using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game2
{
    public class PanelInGame : PanelBase
    {
        [SerializeField] private TextMeshProUGUI _tmpCountDown;
        [SerializeField] private GameObject _tutObject;
        [SerializeField] private GameObject _midHandTut;

        public void SetTextCountDown(string str)
        {
            _tmpCountDown.text = str;
        }

        public void ShowTut(int lane)
        {
            if (lane == 3)
                _midHandTut.SetActive(true);
            else _midHandTut.SetActive(false);
            _tutObject.SetActive(true);
        }

        public void HideTut()
        {
            _tutObject.SetActive(false);
        }
    }
}
