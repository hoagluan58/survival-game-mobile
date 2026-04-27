using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game9
{
    public class PanelInGame : PanelBase
    {
        [SerializeField] private Image _fillProgress;
        [SerializeField] private TextMeshProUGUI _tmpProgressValue;

        [SerializeField] private TextMeshProUGUI _tmpTimeCounter;

        public void UpdateUIProgress(int current,int total)
        {
            _fillProgress.fillAmount = (float)current / total;
            _tmpProgressValue.text = current + "/" + total;
        }

        public void SetTimeCounter(string str)
        {
            _tmpTimeCounter.text = str;
        }
    }
}