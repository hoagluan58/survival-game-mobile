using NFramework;
using Redcode.Extensions;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.Minigame21.UI;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

namespace SquidGame.Minigame21
{
    public class MinigameController : BaseMinigameController
    {
        public enum EGameState
        {
            Idle,
            Playing,
            Win,
            Lose,
        }

        private const int MAX_ROUND = 4;

        [Header("REF")]
        [SerializeField] private IntroStartGame _introStart;
        [SerializeField] private NavMeshSurface _navMeshSurface;

        [SerializeField] private Light _light;
        [SerializeField] private Color _normalLightColor;
        [SerializeField] private Color _gameLightColor;
        [SerializeField] private BaseGuard[] _guards;

        [Header("MANAGER")]
        [SerializeField] private RoomManager _roomManager;
        [SerializeField] private BotManager _botManager;

        [Header("CONTROLLER")]
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private RoundController _roundController;

        private Minigame21MenuUI _ui;

        private EGameState _state;

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();

            UIManager.I.Close(Define.UIName.MINIGAME_21_MENU);
            _ui = UIManager.I.Open<Minigame21MenuUI>(Define.UIName.MINIGAME_21_MENU);

            // Init Manager
            _botManager.Init(this, _roomManager, _roundController);
            _roomManager.Init(this, _roundController, _botManager);

            // Init Controller
            _playerController.Init(_ui.Joystick, _botManager, _roomManager);
            _roundController.Init(this, _ui);
            _cameraController.Init(CameraController.ECameraType.Init);

            // Init UI
            _ui.SetData(this);

            OnPrepare();
        }

        public override void OnStart()
        {
            base.OnStart();

            StartCoroutine(CRStartGame());

            IEnumerator CRStartGame()
            {
                yield return _introStart.CRIntroGame();
                yield return _cameraController.CRSwitchCamera(CameraController.ECameraType.Follow, 0f);
                _roundController.OnStart();
                _roomManager.OnStart();
                BuildNavMesh();
                _botManager.OnStart();
                _playerController.OnStart();
                _ui.OnPlaying(_roundController.GroupRequire, _roundController.CurrentRound);
                _light.color = _gameLightColor;
                _state = EGameState.Playing;
                GameSound.I.PlayBGM(Define.SoundPath.BGM_MG21_INGAME, true);
            }
        }

        public override void OnWin()
        {
            base.OnWin();

            StartCoroutine(CRWinGame());

            IEnumerator CRWinGame()
            {
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);
                _ui.CloseSelf();
                yield return null;
            }
        }

        public override void OnLose()
        {
            base.OnLose();
            StartCoroutine(CRLoseGame());

            IEnumerator CRLoseGame()
            {
                if (_state != EGameState.Playing) yield break;

                GameSound.I.StopBGM();
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Failure);
                _ui.CloseSelf();
                _roundController.OnLoseRound();
                yield return _playerController.CROnLoseRound();
                _botManager.OnEndRound();
                PlayGuardShootAnim();

                GameManager.I.HandleResult();
            }
        }

        public void OnPrepare()
        {
            StartCoroutine(CRPrepare());

            IEnumerator CRPrepare()
            {
                _state = EGameState.Idle;
                _light.color = _normalLightColor;
                _ui.OnPrepare();
                yield return _cameraController.CRSwitchCamera(CameraController.ECameraType.Init, 0);
                _roundController.OnPrepare();
                _roomManager.OnPrepare();
                _botManager.OnPrepare();
                _playerController.OnPrepare();
            }
        }

        public void WinRound()
        {
            StartCoroutine(CRWinRound());

            IEnumerator CRWinRound()
            {
                if (_state != EGameState.Playing) yield break;

                GameSound.I.StopBGM();
                _state = EGameState.Win;
                _roundController.OnWinRound();
                yield return _playerController.CROnWinRound();
                _roomManager.OnWinRound();
                yield return _cameraController.CRSwitchCamera(CameraController.ECameraType.Overview, 0f);

                yield return new WaitUntil(() => _roundController.TimeLeft == 0);

                _botManager.OnEndRound();
                PlayGuardShootAnim();

                // Move to next round or end game
                if (_roundController.CurrentRound == MAX_ROUND)
                {
                    GameManager.I.Win();
                }
                else
                {
                    yield return new WaitForSeconds(2f);
                    OnPrepare();
                }
            }
        }

        public void BuildNavMesh() => _navMeshSurface.BuildNavMesh();

        private void PlayGuardShootAnim()
        {
            _guards.ForEach(x =>
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
                GameSound.I.PlaySFX(Random.value > 0.5f ? Define.SoundPath.SFX_MG01_HOSTAGE_F_HIT_01 : Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
                x.PlayShootAnim();
            });
        }
    }
}
