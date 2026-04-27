using NFramework;
using SquidGame.LandScape.Data;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.UI;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace SquidGame.LandScape.Core
{
    public class MainManager : MonoBehaviour
    {
        private void Start() => StartCoroutine(CRInitGame());

        private IEnumerator CRInitGame()
        {
            yield return null;

            Application.targetFrameRate = 60;

            GameConfig.I.Init();
            GameData.I.Init();
            GameManager.I.Init();

            UIManager.I.Open<LoadingUI>(Define.UIName.LOADING_UI).SetData(GetLoadingTime(), OnLoadingCompleted);

            void OnLoadingCompleted()
            {
                StartCoroutine(CRInternetCheck());

                var userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
                if (userData.IsFirstTimePlayed)
                {
                    userData.IsFirstTimePlayed = false;
                    var firstSeasonId = GameConfig.I.SeasonConfigSO.Configs.Keys.FirstOrDefault();
                    GameManager.I.PlayChallengeMode(firstSeasonId);
                }
                else
                {
                    StartCoroutine(CRLoadMinigame());

                    IEnumerator CRLoadMinigame()
                    {
                        yield return SceneUtils.CRLoadSceneAsync(Define.SceneName.HOME, true);
                        UIManager.I.Open(Define.UIName.HOME_MENU);
                    }
                }
            }
        }

        private float GetLoadingTime()
        {
#if UNITY_EDITOR
            return 1f;
#endif

            return 8f;
        }

        private IEnumerator CRInternetCheck()
        {
            var wait = new WaitForSecondsRealtime(1f);
            while (true)
            {
                var internetStatus = DeviceInfo.HasInternet();
                if (internetStatus == false)
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
