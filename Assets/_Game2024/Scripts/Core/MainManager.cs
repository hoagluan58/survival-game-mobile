using NFramework;
using SquidGame.Gameplay;
using SquidGame.SaveData;
using System.Collections;
using UnityEngine;

namespace SquidGame.Core
{
    public class MainManager : SingletonMono<MainManager>
    {
        private bool _internetStatus;

        private void Start() => StartCoroutine(CRInitGame());

        private IEnumerator CRInitGame()
        {
            yield return null;

            Application.targetFrameRate = 60;

            ConfigManager.I.Init();
            RegisterSaveData();
            InitManager();

            HandleFirstTimeUserPlayed();

            StartCoroutine(CRInternetChecker());

            void RegisterSaveData()
            {
                SaveManager.I.RegisterSaveData(GameLocalization.I);
                SaveManager.I.RegisterSaveData(SoundManager.I);
                SaveManager.I.RegisterSaveData(VibrationManager.I);
                SaveManager.I.RegisterSaveData(UserData.I);
                SaveManager.I.RegisterSaveData(DailyRewardSaveData.I);
                SaveManager.I.Load();
            }

            void InitManager()
            {
                GameManager.I.Init();
            }

            void HandleFirstTimeUserPlayed()
            {
                UIManager.I.Open(Define.UIName.HOME_MENU);
                if (UserData.I.IsFirstTimePlayed)
                {
                    UIManager.I.Open(Define.UIName.SELECT_LANGUAGE_POPUP);
                }
            }
        }

        private IEnumerator CRInternetChecker()
        {
            var wait = new WaitForSecondsRealtime(1f);
            while (true)
            {
                _internetStatus = DeviceInfo.HasInternet();

                if (_internetStatus == false)
                {
                    if (!UIManager.I.IsSpecificViewShown(Define.UIName.NO_INTERNET_POPUP, out var view))
                    {
                        UIManager.I.Open(Define.UIName.NO_INTERNET_POPUP);
                    }
                }
                yield return wait;
            }
        }
    }
}
