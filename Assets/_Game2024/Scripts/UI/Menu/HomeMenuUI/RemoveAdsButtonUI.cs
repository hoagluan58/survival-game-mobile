using SquidGame.LandScape;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    [RequireComponent(typeof(Button))]
    public class RemoveAdsButtonUI : MonoBehaviour
    {
        private Button _button;

        private void Awake() => _button = GetComponent<Button>();

        private void OnEnable() => _button.onClick.AddListener(OnButtonClicked);

        private void OnDisable() => _button.onClick.RemoveListener(OnButtonClicked);

        private void OnButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
        }
    }
}
