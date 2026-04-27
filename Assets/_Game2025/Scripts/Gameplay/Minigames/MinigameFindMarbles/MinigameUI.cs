using NFramework;
using SquidGame.LandScape.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.MinigameFindMarbles
{
    public class MinigameUI : BaseUIView
    {
        [SerializeField] private TextMeshProUGUI _roundTMP;
        [SerializeField] private TextMeshProUGUI _levelTMP;
        [SerializeField] private Button _settingBTN;

        private void Awake() => _settingBTN.onClick.AddListener(OnSettingButtonClicked);

        private void OnSettingButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
        }

        public void SetData(int level)
        {
            UpdateLevelText(level);
            UpdateRoundText(1);
        }

        public void UpdateRoundText(int round) => _roundTMP.SetText($"Round {round}");

        public void UpdateLevelText(int level) => _levelTMP.SetText($"Level {level}");
    }
}
