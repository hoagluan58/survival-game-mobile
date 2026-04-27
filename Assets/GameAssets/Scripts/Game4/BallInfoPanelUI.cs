using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game4
{
    public class BallInfoPanelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _successBallCountTMP;
        [SerializeField] private Image[] _enableImage;

        public void UpdateSuccessBallCount(int amount)
        {
            _successBallCountTMP.text = amount.ToString();
        }

        public void UpdateCurrentBallAmount(int amount)
        {
            foreach (var t in _enableImage)
            {
                t.gameObject.SetActive(false);
            }
            for (var i = 0; i < amount; i++)
            {
                _enableImage[i].gameObject.SetActive(true);
            }
        }
    }
}