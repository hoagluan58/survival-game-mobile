using NFramework;
using SquidGame.LandScape.Config;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Data;
using SquidGame.LandScape.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.UI
{
    public class MinigameItemUI : MonoBehaviour
    {
        [SerializeField] private GameObject _container;

        [Header("UNLOCK")]
        [SerializeField] private GameObject _unlockGroup;
        [SerializeField] private TextMeshProUGUI _unlockTitleTMP;
        [SerializeField] private TextMeshProUGUI _unlockTitleShadow;
        [SerializeField] private Image _unlockThumbnailIMG;
        [SerializeField] private Button _playBTN;

        [Header("ADS")]
        [SerializeField] private GameObject _adsGroup;
        [SerializeField] private TextMeshProUGUI _adsTitleTMP;
        [SerializeField] private TextMeshProUGUI _adsTitleShadow;
        [SerializeField] private Image _adsThumbnailIMG;
        [SerializeField] private Button _adsBTN;

        [Header("COMING SOON")]
        [SerializeField] private GameObject _comingSoonGroup;

        private MinigameConfig _config;

        private void OnEnable()
        {
            _playBTN.onClick.AddListener(OnPlayButtonClicked);
            _adsBTN.onClick.AddListener(OnAdsButtonClicked);
        }

        private void OnDisable()
        {
            _playBTN.onClick.RemoveListener(OnPlayButtonClicked);
            _adsBTN.onClick.RemoveListener(OnAdsButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            GameManager.I.PlayMinigameMode(_config.Id, () =>
            {
                UIManager.I.Close(Define.UIName.HOME_MENU);
                UIManager.I.Close(Define.UIName.MINIGAMES_MENU);
            });
        }

        private void OnAdsButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            // Reward video removed — unlock is free locally
            var userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
            userData.TryAddPlayedMinigame(_config.Id);
            _adsGroup.SetActive(false);
            GameManager.I.PlayMinigameMode(_config.Id, () =>
            {
                UIManager.I.Close(Define.UIName.HOME_MENU);
                UIManager.I.Close(Define.UIName.MINIGAMES_MENU);
            });
        }

        public void SetData(MinigameConfig config, bool isAds)
        {
            _config = config;
            _container.SetActive(true);

            _unlockGroup.SetActive(_config != null);
            _adsGroup.SetActive(_config != null);
            _comingSoonGroup.SetActive(config == null);

            if (_config != null)
            {
                SetTitle($"{_config.Name}");
                SetThumbnail(_config.Thumbnail);
                CheckUnlockedMinigame(_config.Id);
            }
        }

        private void CheckUnlockedMinigame(int minigameId)
        {
            var userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
            _adsGroup.SetActive(!userData.IsMinigamePlayed(minigameId));
        }


        private void SetTitle(string title)
        {
            _unlockTitleTMP.SetText(title);
            _unlockTitleShadow.SetText(title);
            _adsTitleTMP.SetText(title);
            _adsTitleShadow.SetText(title);
        }

        private void SetThumbnail(Sprite sprite)
        {
            _unlockThumbnailIMG.sprite = sprite;
            _adsThumbnailIMG.sprite = sprite;
        }
    }
}
