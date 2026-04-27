using Cinemachine;
using NFramework;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.Utils;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

namespace SquidGame.LandScape.Minigame2
{
    public class MinigameController : BaseMinigameController
    {
        public enum EGameState
        {
            Playing,
            Win,
            Lose,
        }

        private const float GET_READY_TIME = 5f;

        [Header("REF")]
        [SerializeField] private NavMeshSurface _navMeshSurface;
        [SerializeField] private GameObject _startMapCollider;
        [SerializeField] private BaseCharacter _baseCharacter;
        [SerializeField] private CinemachineFreeLook _cinemachineFreeLook;

        [Header("CONTROLLER")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private NPCController _npcController;
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private Timer _timer;

        private MinigameUI _ui;
        private EGameState _state;
        private LevelSaveData _saveData;
        private LevelData _levelData;
        private IGameModeHandler _handler;

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            Init();
        }

        public override void OnRevive()
        {
            var hasValidGlassPanel = _playerController.HasValidGlassPanel;

            if (hasValidGlassPanel)
            {
                HandleOnRevive();
                return;
            }
            else
            {
                GameManager.I.ReloadMinigame();
            }

            void HandleOnRevive()
            {
                GameManager.I.StartMinigame();
                if (!_ui.gameObject.activeSelf)
                {
                    _ui = UIManager.I.Open<MinigameUI>(Define.UIName.MINIGAME_02_MENU);
                }
                _npcController.ToggleUpdate(true);
                _playerController.OnRevive();
                _timer.PauseTimer(false);
                _state = EGameState.Playing;
            }
        }

        [Button]
        public void Win()
        {
            if (_state != EGameState.Playing) return;

            StartCoroutine(CRWin());

            IEnumerator CRWin()
            {
                var waiter = new WaitForSeconds(2f);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
                _state = EGameState.Win;
                _ui.CloseSelf();
                _timer.StopTimer();
                _saveData.Save();
                yield return waiter;
                GameManager.I.Win();
            }
        }

        [Button]

        public void Lose()
        {
            if (_state != EGameState.Playing) return;

            CheckRevive();
            _state = EGameState.Lose;
            _playerController.SetActive(false);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_LOSING);
            _npcController.ToggleUpdate(false);
            _ui.CloseSelf();
            _timer.PauseTimer(true);
            GameManager.I.Lose();
        }

        private void Init()
        {
            _handler = GameManager.I.CurGameModeHandler;
            _ui = UIManager.I.Open<MinigameUI>(Define.UIName.MINIGAME_02_MENU);
            _ui.Init(_playerController, _cinemachineFreeLook, this);
            _playerController.Init(_ui.Joystick, this);

            _saveData = new LevelSaveData(_handler.GameMode);
            _levelGenerator.SpawnLevel(_saveData.Level);
            _levelData = _levelGenerator.LevelData;
            _navMeshSurface.BuildNavMesh();
            _ui.UpdateLevelText(_saveData.Level);

            _npcController.Init(_levelGenerator.GlassPanels);
            var maxIntervalTime = GET_READY_TIME / _levelData.BotNumber;
            StartCoroutine(_npcController.CRSpawnInterval(_levelData.BotNumber, maxIntervalTime));

            GetReady();
        }

        private void GetReady()
        {
            // Get ready 5 seconds
            _startMapCollider.SetActive(true);
            _ui.SetActiveTutorialPanel(true);
            _timer.Init(GET_READY_TIME, OnTimerChanged, OnTimerEnd);
            _timer.StartTimer();
            _playerController.OnStartGame();
            _state = EGameState.Playing;

            void OnTimerChanged(float value)
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_TICK);
                _ui.UpdateTutorialText($"Game will start in {value}s");
            }

            void OnTimerEnd()
            {
                _ui.SetActiveTutorialPanel(false);
                StartGame();
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_WHISTLE);
            }
        }

        public void StartGame()
        {
            _startMapCollider.SetActive(false);
            _ui.SetActiveTimerPanel(true);
            _timer.Init(_levelData.PlayTime, OnTimerChanged, OnTimerEnd);
            _timer.StartTimer();
            _npcController.StartJumpingBehaviour();
            GameManager.I.StartMinigame();

            void OnTimerChanged(float value) => _ui.UpdateTimeText(value);

            void OnTimerEnd()
            {
                foreach (var glass in _levelGenerator.GlassPanels)
                {
                    glass.Break();
                }
                _npcController.KillNPCNotReachDestination();
                if (_playerController.IsWin) Win();
                else
                {
                    _playerController.Die();
                }
            }
        }

        private void CheckRevive()
        {
            var isChallengeMode = _handler.GameMode == EGameMode.Challenge;
            if (!isChallengeMode) return;

            var handler = _handler as ChallengeMode;

            handler.CanRevive = _timer.TimeLeft > 0;
        }
    }
}
