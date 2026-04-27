using NFramework;
using SquidGame.Core;
using SquidGame.LandScape;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.UI
{
    public class RewardPopupUI : BaseUIView
    {
        [Header("SINGLE")]
        [SerializeField] private GameObject _singleRewardPNL;
        [SerializeField] private RewardItemUI _singleRewardItemUI;

        [Header("MULTIPLE")]
        [SerializeField] private GameObject _multiRewardPNL;
        [SerializeField] private List<RewardItemUI> _multiRewardItemUIList;

        public override void OnOpen()
        {
            base.OnOpen();
            _singleRewardPNL.SetActive(false);
            _multiRewardPNL.SetActive(false);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_CLAIM_REWARD);
            this.InvokeDelay(2f, () => CloseSelf());
        }

        public void SetData(RewardData data)
        {
            _singleRewardPNL.SetActive(true);
            _singleRewardItemUI.SetData(data.Icon, data.Amount);
        }

        public void SetData(List<RewardData> datas)
        {
            if (datas.Count == 1)
            {
                SetData(datas[0]);
                return;
            }

            _multiRewardPNL.SetActive(true);
            for (int i = 0; i < datas.Count; i++)
            {
                var data = datas[i];
                if (_multiRewardItemUIList.IsIndexOutOfList(i))
                {
                    break;
                }
                _multiRewardItemUIList[i].SetData(data.Icon, data.Amount);
            }
        }
    }
}
