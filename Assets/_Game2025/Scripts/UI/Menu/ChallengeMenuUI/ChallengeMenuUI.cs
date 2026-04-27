using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Data;
using SquidGame.LandScape.Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace SquidGame.LandScape.UI
{
    public class ChallengeMenuUI : BaseUIView
    {
        [SerializeField] private Button _backBTN;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private ChallengeSeasonItemUI _seasonItemPf;

        private List<ChallengeSeasonItemUI> _seasonItemList = new List<ChallengeSeasonItemUI>();
        private ObjectPool<ChallengeSeasonItemUI> _seasonItemPool;

        private void Awake()
        {
            _seasonItemPool = new(
               () => Instantiate(_seasonItemPf, _scrollRect.content),
               item => item.gameObject.SetActive(true),
               item => item.gameObject.SetActive(false),
               item => Destroy(item.gameObject));
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _backBTN.onClick.AddListener(OnBackButtonClicked);
            SetData();
        }

        public override void OnClose()
        {
            base.OnClose();
            _backBTN.onClick.RemoveListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            CloseSelf();
            UIManager.I.Open(Define.UIName.HOME_MENU);
        }

        private void SetData()
        {
            ClearPool();

            var userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);

            // Spawn season config
            foreach (var config in GameConfig.I.SeasonConfigSO.Configs.Values)
            {
                var item = _seasonItemPool.Get();
                item.SetData(config.Season, config.Thumbnail, userData.IsSeasonUnlock(config.Season), OnStartSeason, OnLockClicked);
                item.transform.SetAsLastSibling();
                _seasonItemList.Add(item);

                void OnStartSeason()
                {
                    GameSound.I.PlaySFXButtonClick();
                    UIManager.I.Close(Define.UIName.HOME_MENU);
                    CloseSelf();
                    GameManager.I.PlayChallengeMode(config.Season);
                }

                void OnLockClicked()
                {
                    GameSound.I.PlaySFXButtonClick();
                    NotificationPanelUI.OnShowNotification?.Invoke();
                }
            }

            // Spawn coming soon item
            var comingSoonItem = _seasonItemPool.Get();
            comingSoonItem.SetComingSoon(OnComingSoon);
            comingSoonItem.transform.SetAsLastSibling();
            _seasonItemList.Add(comingSoonItem);

            void OnComingSoon()
            {
                GameSound.I.PlaySFXButtonClick();
                NotificationPanelUI.OnShowNotification?.Invoke();
            }

        }

        private void ClearPool()
        {
            _seasonItemList.ForEach(x => _seasonItemPool.Release(x));
            _seasonItemList.Clear();
        }
    }
}
