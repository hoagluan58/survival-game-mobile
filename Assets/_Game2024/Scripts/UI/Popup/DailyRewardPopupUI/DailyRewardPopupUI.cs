using NFramework;
using SquidGame.Config;
using SquidGame.Core;
using SquidGame.LandScape;
using SquidGame.SaveData;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class DailyRewardPopupUI : BaseUIView
    {
        [SerializeField] private RectTransform _popupRect;
        [SerializeField] private Button _closeBTN;
        [SerializeField] private DailyNormalItemUI[] _normalItems;
        [SerializeField] private DailySpecialItemUI _specialItem;

        [SerializeField] private GameObject _claimButtonPNL;
        [SerializeField] private GameObject _donePNL;
        [SerializeField] private Button _claimNormalBTN;
        [SerializeField] private Button _claimAdsBTN;
        [SerializeField] private TextMeshProUGUI _claimAdsTMP;

        public override void OnOpen()
        {
            base.OnOpen();
            _closeBTN.onClick.AddListener(OnCloseButtonClicked);
            _claimNormalBTN.onClick.AddListener(OnClaimButtonClicked);
            _claimAdsBTN.onClick.AddListener(OnClaimAdsButtonClicked);
            _popupRect.DOPunchScalePopup();
            SetData();
        }

        public override void OnClose()
        {
            base.OnClose();
            _closeBTN.onClick.RemoveListener(OnCloseButtonClicked);
            _claimNormalBTN.onClick.RemoveListener(OnClaimButtonClicked);
            _claimAdsBTN.onClick.RemoveListener(OnClaimAdsButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            CloseSelf();
        }

        private void OnClaimButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            var rewards = DailyRewardSaveData.I.Claim();
            UIManager.I.Open<RewardPopupUI>(Define.UIName.REWARD_POPUP).SetData(rewards);
            SetData();
        }

        private void OnClaimAdsButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            var rewards = DailyRewardSaveData.I.Claim(2);
            UIManager.I.Open<RewardPopupUI>(Define.UIName.REWARD_POPUP).SetData(rewards);
            SetData();
        }

        // Future create a class RewardItemUI to handle RewardData
        public void SetData()
        {
            var configs = ConfigManager.I.DailyRewardConfig.Values.ToList();
            var isClaimAll = DailyRewardSaveData.I.IsClaimAll();
            var canClaim = DailyRewardSaveData.I.CanClaimDaily();

            _claimAdsTMP.text = GameLocalization.I.GetStringFromTable("STRING_CLAIM_X", 2);
            _claimAdsBTN.gameObject.DOScaleLoop(Vector3.one, Vector3.one * 1.1f, 0.5f);

            _claimButtonPNL.SetActive(!isClaimAll && canClaim);
            _donePNL.SetActive(!canClaim && !isClaimAll);


            SetNormalitem(configs);
            SetSpecialItem(configs.Last());
        }

        private void SetNormalitem(List<DailyRewardConfig> configs)
        {
            for (var i = 0; i < _normalItems.Length; i++)
            {
                var config = configs[i];
                var item = _normalItems[i];
                var state = DailyRewardSaveData.I.GetRewardState(config.Day);
                if (state == DailyRewardSaveData.EDailyState.Ready && !DailyRewardSaveData.I.CanClaimDaily())
                {
                    state = DailyRewardSaveData.EDailyState.Future;
                }
                switch (state)
                {
                    case DailyRewardSaveData.EDailyState.Ready:
                        item.SetNormal(config.Day, config.Rewards[0].Amount, true);
                        break;
                    case DailyRewardSaveData.EDailyState.Claimed:
                        item.SetComplete(config.Day);
                        break;
                    case DailyRewardSaveData.EDailyState.Future:
                        item.SetNormal(config.Day, config.Rewards[0].Amount, false);
                        break;
                }
            }

        }

        private void SetSpecialItem(DailyRewardConfig config)
        {
            var state = DailyRewardSaveData.I.GetRewardState(config.Day);
            switch (state)
            {
                case DailyRewardSaveData.EDailyState.Ready:
                    _specialItem.SetNormal(config.Day, config.Rewards[1].Amount, config.Rewards[0].Icon, true);
                    break;
                case DailyRewardSaveData.EDailyState.Claimed:
                    _specialItem.SetComplete(config.Day);
                    break;
                case DailyRewardSaveData.EDailyState.Future:
                    _specialItem.SetNormal(config.Day, config.Rewards[1].Amount, config.Rewards[0].Icon, false);
                    break;
            }
        }
    }
}
