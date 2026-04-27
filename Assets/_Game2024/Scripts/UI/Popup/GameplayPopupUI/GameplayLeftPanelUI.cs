using DG.Tweening;
using SquidGame.SaveData;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using NFramework;
using SquidGame.Gameplay;

namespace SquidGame.UI
{
    public class GameplayLeftPanelUI : MonoBehaviour
    {
        [SerializeField] private GameObject _dayTrackPNL;
        [SerializeField] private TextMeshProUGUI _dayTrackTMP;
        [SerializeField] private LocalizedString _dayLocalizedString;

        private void OnEnable()
        {
            _dayLocalizedString.StringChanged += UpdateText;
            SetData();
        }

        private void OnDisable() => _dayLocalizedString.StringChanged -= UpdateText;

        private void SetData() => UpdateDayTrackPanel();

        private void UpdateDayTrackPanel()
        {
            if (GameManager.I.CurModeHandler.GameMode == EGameMode.Training)
            {
                _dayTrackPNL.SetActive(false);
                return;
            }
            _dayTrackPNL.SetActive(true);
            _dayTrackPNL.SetActiveChilds(false);
            _dayTrackPNL.transform.localScale = Vector3.one;
            _dayTrackPNL.transform.DOScale(new Vector3(0, 1, 1f), 0.5f).From().OnComplete(() =>
            {
                _dayTrackPNL.SetActiveChilds(true);
            });
        }

        private void UpdateText(string value)
        {
            _dayTrackTMP.text = string.Format(value, new object[] { UserData.I.Day }) + $"/{UserData.I.MaxChallengeDay}";
        }
    }
}
