using Sirenix.OdinInspector;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Data;
using System.Collections;
using UnityEngine;

namespace SquidGame.LandScape.Game
{
    public class LobbyController : MonoBehaviour
    {
        private UserData _userData;
        private SeasonProgressData _seasonProgressData;
        private int _seasonId;

        private void Start() => Init();

        private void OnDisable()
        {
            Debug.Log($"Disable");
        }

        private void Init()
        {
            _seasonId = GameManager.I.CurSeasonId;
            _userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
            _seasonProgressData = _userData.GetSeasonData(_seasonId);

            if (_seasonProgressData.IsSeasonMaxProgress)
            {
                StartCoroutine(CRBreakPiggyBank());
            }
        }

        private IEnumerator CRBreakPiggyBank()
        {
            Debug.Log($"Break piggy bank, delay anim then back to home screen");
            _userData.CompleteSeason(_seasonId);
            yield return new WaitForSeconds(2f);
            GameManager.I.Exit();
        }

        [Button]
        public void PlayNextMinigame()
        {
            var seasonConfig = GameConfig.I.SeasonConfigSO.GetSeasonConfig(_seasonId);
            var minigameConfig = seasonConfig.MinigameList[_seasonProgressData.Progress];
            GameManager.I.LoadMinigame(minigameConfig.Id);
        }
    }
}
