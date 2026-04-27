using NFramework;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.Game
{
    public class GameManager : SingletonMono<GameManager>
    {
        [SerializeField] private TransitionUI _transitionUI;


        private Dictionary<EGameMode, IGameModeHandler> _gameModeHandlerDic = new Dictionary<EGameMode, IGameModeHandler>();

        private int _curSeasonId;
        private EGameState _curGameState;
        private BaseMinigameController _minigameInstance;
        private IGameModeHandler _curGameModeHandler;

        public IGameModeHandler CurGameModeHandler => _curGameModeHandler;
        public EGameState CurGameState => _curGameState;
        public int CurSeasonId => _curSeasonId;
        public BaseMinigameController MinigameInstance { get => _minigameInstance; set => _minigameInstance = value; }
        public TransitionUI TransitionUI => _transitionUI;
        public void Init()
        {
            _curGameState = EGameState.None;
            _gameModeHandlerDic.Add(EGameMode.Challenge, new ChallengeMode());
            _gameModeHandlerDic.Add(EGameMode.Minigame, new MinigameMode());
        }

        public void PlayChallengeMode(int seasonId)
        {
            _curSeasonId = seasonId;

            EnterGameMode(EGameMode.Challenge);
            StartCoroutine(CRLoadLobby());

            IEnumerator CRLoadLobby()
            {
                yield return _transitionUI.CRLoadingAnim(true);
                if (_minigameInstance != null)
                {
                    yield return SceneUtils.CRUnloadSceneAsync(_minigameInstance.Config.SceneName);
                }
                yield return SceneUtils.CRUnloadSceneAsync(Define.SceneName.HOME);
                yield return SceneUtils.CRUnloadSceneAsync(Define.SceneName.LOBBY);
                yield return SceneUtils.CRLoadSceneAsync(Define.SceneName.LOBBY, true, true);
                yield return _transitionUI.CRLoadingAnim(false);
            }
        }

        public void PlayMinigameMode(int minigameId)
        {
            EnterGameMode(EGameMode.Minigame);
            LoadMinigame(minigameId);
        }


        public void PlayMinigameMode(int minigameId, UnityAction onStartLoadAction)
        {
            EnterGameMode(EGameMode.Minigame);
            LoadMinigame(minigameId, onStartLoadAction, null);
        }



        private void EnterGameMode(EGameMode gameMode)
        {
            _curGameModeHandler = _gameModeHandlerDic[gameMode];
            _curGameModeHandler.OnEnter();
        }

        public void LoadMinigame(int minigameId)
        {
            StartCoroutine(CRLoadMinigame());

            IEnumerator CRLoadMinigame()
            {
                _curGameState = EGameState.None;
                yield return _transitionUI.CRLoadingAnim(true);
                if (_minigameInstance != null)
                {
                    yield return SceneUtils.CRUnloadSceneAsync(_minigameInstance.Config.SceneName);
                }
                if (_curGameModeHandler.GameMode == EGameMode.Challenge)
                {
                    yield return SceneUtils.CRUnloadSceneAsync(Define.SceneName.LOBBY);
                }
                else
                {
                    yield return SceneUtils.CRUnloadSceneAsync(Define.SceneName.HOME);
                }

                var config = GameConfig.I.MinigameConfigs[minigameId];

                yield return SceneUtils.CRLoadSceneAsync(config.SceneName, true, true);
                _minigameInstance.Init(config);
                _minigameInstance.OnLoadMinigame();
                yield return _transitionUI.CRLoadingAnim(false);
            }
        }

        public void LoadMinigame(int minigameId, UnityAction onStartLoadAction, UnityAction onEndLoadAction)
        {
            StartCoroutine(CRLoadMinigame());

            IEnumerator CRLoadMinigame()
            {
                _curGameState = EGameState.None;
                yield return _transitionUI.CRLoadingAnim(true);
                onStartLoadAction?.Invoke();
                if (_minigameInstance != null)
                {
                    yield return SceneUtils.CRUnloadSceneAsync(_minigameInstance.Config.SceneName);
                }
                if (_curGameModeHandler.GameMode == EGameMode.Challenge)
                {
                    yield return SceneUtils.CRUnloadSceneAsync(Define.SceneName.LOBBY);
                }
                else
                {
                    yield return SceneUtils.CRUnloadSceneAsync(Define.SceneName.HOME);
                }

                var config = GameConfig.I.MinigameConfigs[minigameId];

                yield return SceneUtils.CRLoadSceneAsync(config.SceneName, true, true);
                _minigameInstance.Init(config);
                _minigameInstance.OnLoadMinigame();
                yield return _transitionUI.CRLoadingAnim(false);
                onEndLoadAction?.Invoke();
            }
        }

        public void ReloadMinigame()
        {
            if (_minigameInstance == null)
            {
                NFramework.Logger.LogError("Can't reload minigame");
                return;
            }
            _minigameInstance.OnReload();
            LoadMinigame(_minigameInstance.Config.Id);
        }

        public void StartMinigame()
        {
            _curGameState = EGameState.Playing;
            _minigameInstance.OnStart();
        }

        [Button]
        public void Win()
        {
            if (_curGameState != EGameState.Playing)
                return;

            _curGameState = EGameState.Win;
            _minigameInstance.OnWin();
            _curGameModeHandler.OnHandleResult();
        }

        [Button]
        public void Lose()
        {
            if (_curGameState != EGameState.Playing)
                return;

            _curGameState = EGameState.Lose;
            _minigameInstance.OnLose();
            _curGameModeHandler.OnHandleResult();
        }

        public void Revive()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_REVIVE);
            _curGameState = EGameState.Playing;
            _minigameInstance.OnRevive();
        }

        public void Exit()
        {
            StartCoroutine(CRExitMinigame());

            IEnumerator CRExitMinigame()
            {
                yield return _transitionUI.CRLoadingAnim(true);
                if (_minigameInstance != null)
                {
                    yield return SceneUtils.CRUnloadSceneAsync(_minigameInstance.Config.SceneName);
                }
                if (_curGameModeHandler.GameMode == EGameMode.Challenge)
                {
                    yield return SceneUtils.CRUnloadSceneAsync(Define.SceneName.LOBBY);
                }

                yield return SceneUtils.CRLoadSceneAsync(Define.SceneName.HOME, true);
                UIManager.I.CloseAllInLayer(EUILayer.Menu);
                UIManager.I.CloseAllInLayer(EUILayer.Popup);
                UIManager.I.Open(Define.UIName.HOME_MENU);
                _curGameState = EGameState.None;
                _curSeasonId = -1;
                _curGameModeHandler.OnExit();
                _curGameModeHandler = null;
                _minigameInstance = null;

                yield return _transitionUI.CRLoadingAnim(false);
            }
        }


        public void ReturnToLobby()
        {
            UIManager.I.CloseAllInLayer(EUILayer.Menu);
            PlayChallengeMode(_curSeasonId);
        }


        public EGameMode GetGameMode() => _curGameModeHandler.GameMode;

        public void SetGameState(EGameState gameState) => _curGameState = gameState;

        public void ClearCurrentMinigame()
        {
            _minigameInstance = null;
        }
    }

    public enum EGameMode
    {
        Challenge,
        Minigame,
    }

    public enum EGameState
    {
        None,
        Playing,
        Win,
        Lose
    }
}
