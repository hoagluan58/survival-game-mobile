using NFramework;
using SquidGame.Config;
using SquidGame.Core;
using SquidGame.LandScape;
using SquidGame.SaveData;
using SquidGame.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.Gameplay
{
    public class GameManager : SingletonMono<GameManager>
    {
        [SerializeField] private TransitionUI _transitionUI;

        public BaseMinigameController MinigameInstance { get => _minigameInstance; set => _minigameInstance = value; }
        public EGameState CurGameState = EGameState.None;
        public IGameModeHandler CurModeHandler => _curModeHandler;

        private Dictionary<EGameMode, IGameModeHandler> _gameModeHandlerDic = new Dictionary<EGameMode, IGameModeHandler>();
        private IGameModeHandler _curModeHandler;
        private MinigameConfig _curMinigameConfig;
        private BaseMinigameController _minigameInstance;

        public void Init()
        {
            _gameModeHandlerDic.Add(EGameMode.Challenge, new ChallengeMode());
            _gameModeHandlerDic.Add(EGameMode.Training, new TrainingMode());
        }

        public void PlayGameMode(EGameMode gameMode, int minigameId)
        {
            if (_gameModeHandlerDic.TryGetValue(gameMode, out _curModeHandler))
            {
                GameSound.I.StopBGM();
                _curModeHandler.OnStart();
                this.InvokeDelay(TransitionUI.DELAY_TIME, () => UIManager.I.CloseAllInLayer(EUILayer.Menu));
                LoadMinigame(minigameId);
            }
        }

        public void ReloadMinigame()
        {
            if (_curMinigameConfig == null)
            {
                NFramework.Logger.LogError("Can't reload minigame");
                return;
            }
            LoadMinigame(_curMinigameConfig.Id);
        }

        public void LoadMinigame(int minigameId)
        {
            StartCoroutine(CRLoadMinigame());

            IEnumerator CRLoadMinigame()
            {
                CurGameState = EGameState.None;

                yield return _transitionUI.CRLoadingAnim(true);
                if (_curMinigameConfig != null)
                {
                    yield return SceneUtils.CRUnloadSceneAsync(_curMinigameConfig.SceneName);
                }
                _curMinigameConfig = ConfigManager.I.MinigameConfig[minigameId];
                yield return SceneUtils.CRLoadSceneAsync(_curMinigameConfig.SceneName, true, true);

                _minigameInstance?.Init(_curMinigameConfig);

                _minigameInstance?.OnLoadMinigame();

                yield return _transitionUI.CRLoadingAnim(false);
            }
        }

        public void StartMinigame()
        {
            CurGameState = EGameState.Playing;
            _minigameInstance?.OnStart();
        }

        public void Win(float delay = 0f)
        {
            if (CurGameState != EGameState.Playing)
                return;

            CurGameState = EGameState.Win;
            _minigameInstance?.OnWin();

            StartCoroutine(CRLoadResult());

            IEnumerator CRLoadResult()
            {
                yield return new WaitForSeconds(delay);
                HandleResult();
            }
        }

        public void Lose()
        {
            if (CurGameState != EGameState.Playing)
                return;
            CurGameState = EGameState.Lose;
            _minigameInstance?.OnLose();
        }

        public void Revive()
        {
            CurGameState = EGameState.Playing;
            _minigameInstance?.OnRevive();
        }

        public void UseBooster() => _minigameInstance?.OnUseBooster();

        public void HandleResult()
        {
            StartCoroutine(CRLoadResult());

            IEnumerator CRLoadResult()
            {
                GameSound.I.StopBGM();
                if (_curMinigameConfig != null)
                {
                    yield return SceneUtils.CRUnloadSceneAsync(_curMinigameConfig.SceneName);
                }
                _curModeHandler.OnHandleResult();
            }
        }

        public void GoNextDay(bool backToHomeMenu = false)
        {
            UserData.I.Day++;
            if (backToHomeMenu)
            {
                Exit();
            }
            else
            {
                var minigameId = UserData.I.CurMinigameId;
                LoadMinigame(ConfigManager.I.MinigameConfig[minigameId].Id);
            }
        }

        public void Retry() => _curModeHandler.OnRetry();

        public void Exit()
        {
            UIManager.I.CloseAllInLayer(EUILayer.Menu);
            StartCoroutine(CRExitMinigame());

            IEnumerator CRExitMinigame()
            {
                yield return _transitionUI.CRLoadingAnim(true);

                if (_curMinigameConfig != null)
                {
                    yield return SceneUtils.CRUnloadSceneAsync(_curMinigameConfig.SceneName);
                }

                GameSound.I.StopBGM();
                UIManager.I.Open(Define.UIName.HOME_MENU);
                _curModeHandler.OnExit();
                _curMinigameConfig = null;
                _curModeHandler = null;
                _minigameInstance = null;

                yield return _transitionUI.CRLoadingAnim(false);
            }
        }
    }

    public enum EGameState
    {
        None,
        Playing,
        Win,
        Lose,
    }

    public enum EGameMode
    {
        None,
        Training,
        Challenge,
    }
}
