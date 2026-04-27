using SquidGame.Config;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.SaveData;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class TrainingItemUI : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private LocalizedString _nameTMP;
        [SerializeField] private TextMeshProUGUI _levelTMP;
        [SerializeField] private GameObject _hotIcon;
        [SerializeField] private GameObject _newIcon;

        [Header("Normal")]
        [SerializeField] private GameObject _normalGroup;
        [SerializeField] private Image _thumbIMG;
        [SerializeField] private Button _normalBTN;

        [Header("Ads")]
        [SerializeField] private GameObject _adsGroup;
        [SerializeField] private Image _thumbAdsIMG;
        [SerializeField] private Button _adsBTN;

        private MinigameConfig _config;

        private void OnEnable()
        {
            _normalBTN.onClick.AddListener(OnButtonClicked);
            _adsBTN.onClick.AddListener(OnAdsButtonClicked);
            _nameTMP.StringChanged += OnLanguageChanged;
        }

        private void OnDisable()
        {
            _normalBTN.onClick.RemoveListener(OnButtonClicked);
            _adsBTN.onClick.RemoveListener(OnAdsButtonClicked);
            _nameTMP.StringChanged -= OnLanguageChanged;
        }

        private void OnButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            GameManager.I.PlayGameMode(EGameMode.Training, _config.Id);
        }

        private void OnAdsButtonClicked()
        {
            // Call ads and call back 
            UserData.I.UnlockNewTrainingMinigame(_config.Id);
            OnButtonClicked();
        }

        private void OnLanguageChanged(string value) => UpdateNameText();


        public void SetData(MinigameConfig config)
        {
            _container.SetActive(config != null);
            if (config != null)
            {
                _config = config;
                var isAds = IsAds();
                _normalGroup.SetActive(!isAds);
                _adsGroup.SetActive(isAds);
                _thumbIMG.sprite = _config.Thumbnail;
                _thumbAdsIMG.sprite = _config.Thumbnail;
                _hotIcon.SetActive(_config.IsHot);
                _newIcon.SetActive(_config.IsNew);
                UpdateNameText();
            }
        }

        private void UpdateNameText()
        {
            if (_config != null)
            {
                var key = $"STRING_MINIGAME_{_config.Id}";
                _nameTMP.TableEntryReference = key;
                _levelTMP.text = GameLocalization.I.GetStringFromTable(key);
            }
        }

        private bool IsAds() => _config.IsAds && !UserData.I.TrainingMinigameUnlockedByAds.Contains(_config.Id);
    }
}
