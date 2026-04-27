using TMPro;
using UnityEngine;

namespace SquidGame.LandScape.UI
{
    public class MinigameTutorialPanelUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectPanel;
        [SerializeField] private TextMeshProUGUI _contentTMP;

        public void Show() => Show(string.Empty);

        public void Show(string content)
        {
            _rectPanel.gameObject.SetActive(true);
            UpdateText(content);
        }

        public void UpdateText(string text)
        {
            _contentTMP.SetText(text);
        }

        public void Hide()
        {
            _rectPanel.gameObject.SetActive(false);
        }
    }
}
