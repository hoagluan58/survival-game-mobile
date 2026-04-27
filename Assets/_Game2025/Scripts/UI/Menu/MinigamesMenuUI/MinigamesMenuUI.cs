using NFramework;
using SquidGame.LandScape.Config;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.UI
{
    public class MinigamesMenuUI : BaseUIView
    {
        [SerializeField] private Button _backBTN;
        [SerializeField] private MinigamesScrollerUI _scroller;

        public override void OnOpen()
        {
            base.OnOpen();
            _backBTN.onClick.AddListener(OnBackButtonClicked);
            _scroller.SetData();// DELETE TEMP USE THIS
            //LoadTempView();
        }

        public override void OnClose()
        {
            base.OnClose();
            _backBTN.onClick.RemoveListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            CloseSelf();
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.HOME_MENU);
        }

        // Delete this and use scroller later on
        #region TEMP

        [SerializeField] private List<MinigameItemUI> _items;

        private List<MinigameConfig> _datas;
        private bool _isInit = false;
        private const int _comingSoonItemCount = 2;

        private void LoadTempView()
        {
            if (!_isInit)
            {
                _datas = GameConfig.I.MinigameConfigs.Values.ToList();
                for (var i = 0; i < _comingSoonItemCount; i++)
                {
                    _datas.Add(null);
                }
            }

            var userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
            for (var i = 0; i < _items.Count; i++)
            {
                var index = i;
                if (!_datas.IsIndexOutOfList(index))
                {
                    var data = _datas[index];
                    var isAds = data == null || !userData.IsMinigamePlayed(data.Id);
                    _items[index].SetData(data, isAds);
                }
            }
            _isInit = true;
        }
        #endregion
    }
}
