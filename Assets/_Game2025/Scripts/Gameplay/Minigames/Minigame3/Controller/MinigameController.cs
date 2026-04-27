using Cysharp.Threading.Tasks;
using NFramework;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.Minigame2;
using System.Collections;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class MinigameController : BaseMinigameController
    {
        public const string CHALLENGE_LEVEL_KEY = "Level_Minigame3_Challenge";
        public const string MINIGAME_LEVEL_KEY = "Level_Minigame3_Minigame";

        [Header("UI")]
        private Minigame03MenuUI _minigameUI;

        [Header("CONTROLLER")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private DalgonaController _dalgonaController;
        [SerializeField] private NeedleController _needleController;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private BreakDalgonaController _breakDalgonaController;
        [SerializeField] private OutroController _outroController;
        [SerializeField] private TimeController _timeController;
        [SerializeField] private DoorController _doorController;
        [SerializeField] private BotController _botController;
        [SerializeField] private Guard _guard;

        [Header("TRANSFORM")]
        [SerializeField] private Transform _tfPlayerInStartStep;
        [SerializeField] private Transform _tfPlayerInDalgonaStep;

        [Header("CONFIG")]
        [SerializeField] private Minigame03SO _minigame03SO;
        [SerializeField] private SerializableDictionary<MapType, GameObject> _mapDic = new SerializableDictionary<MapType, GameObject>();

        private StepState _currentStepState;
        private MapType _currentMapType = MapType.Map_1;
        private EGameMode _gameMode;
        private AudioSource _losingAudioSource;
        public CameraController CameraController => _cameraController;
        public DalgonaController DalgonaController => _dalgonaController;
        public NeedleController NeedleController => _needleController;
        public DoorController DoorController => _doorController;
        public PlayerController PlayerController => _playerController;
        public BreakDalgonaController BreakDalgonaController => _breakDalgonaController;
        public TimeController TimeController => _timeController;
        public EGameMode GameMode => _gameMode;


        private void Start()
        {
            GameManager.I.StartMinigame();
        }

        private void OnDestroy()
        {
            Input.multiTouchEnabled = true;
            if (_minigameUI == null) return;
            _minigameUI.CloseSelf();
        }

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            _minigameUI = UIManager.I.Open<Minigame03MenuUI>(Core.Define.UIName.MINIGAME_03_MENU);
            _minigameUI.PlayerPanelUI.Joystick.ResetJoystick();
            _gameMode = GameManager.I.CurGameModeHandler.GameMode;
            Init();

        }

        public override void OnStart()
        {
            base.OnStart();
            StartPlayerStep();
        }

        public override void OnRevive()
        {
            Replay();
        }

        private void Init()
        {
            _minigameUI.Init(this);
            _cameraController.Init(this);
            _needleController.Init(this, _minigameUI.CutDalgonaPanelUI);
            _dalgonaController.Init(this);
            _breakDalgonaController.Init(this, _minigameUI.BreakDalgonaPanelUI, _minigame03SO);
            _outroController.Init(this);
            _timeController.Init(this);
            _doorController.Init(this);
            _botController.Init(this);
            _minigame03SO.Init();
            if (!PlayerPrefs.HasKey(CHALLENGE_LEVEL_KEY))
            {
                PlayerPrefs.SetInt(CHALLENGE_LEVEL_KEY, 1);
            }
            if (!PlayerPrefs.HasKey(MINIGAME_LEVEL_KEY))
            {
                PlayerPrefs.SetInt(MINIGAME_LEVEL_KEY, 1);
            }
            _minigameUI.SetTextLevel(PlayerPrefs.GetInt(GetKeySaveLevel()));
        }

        // step 1
        private void StartPlayerStep()
        {
            Input.multiTouchEnabled = true;
            _currentStepState = StepState.Player;
            _minigameUI.SetActivePanel(PanelType.Player);
            _currentMapType = MapType.Map_1;
            ChangeMap();
            _cameraController.UseFreeLook();
            _playerController.transform.position = _tfPlayerInStartStep.position;
            _playerController.transform.localRotation = Quaternion.Euler(_tfPlayerInStartStep.localEulerAngles);
        }

        // step 2
        public void StartCutPagonalStep(float delayTime = 0, float changeScreenTime = 0)
        {
            Input.multiTouchEnabled = false;
            _currentStepState = StepState.CutDalgona;
            _dalgonaController.LoadDalgona(delayTime, () =>
            {
                SetupDalgonaStep();
            });

            void SetupDalgonaStep()
            {
                _currentMapType = MapType.Map_2;
                ChangeMap();
                _minigameUI.SetActiveAllPanel(false);
                _minigameUI.DoFadingBlackScreen(changeScreenTime,
                    onStartFadeOut: () =>
                    {
                        StartCoroutine(_cameraController.CRChangeVirtualCam(VirtualCamType.DalgonaCut));
                        _botController.SetActiveAllBot(false);
                        _playerController.transform.position = _tfPlayerInDalgonaStep.position;
                        _playerController.transform.localRotation = Quaternion.Euler(_tfPlayerInDalgonaStep.localEulerAngles);
                    },
                    onComplete: () =>
                    {
                        _minigameUI.SetActivePanel(PanelType.DalgonaCut);
                        _timeController.SetMaxTime();
                        _timeController.StartCountDown(TimerType.CutStep);
                        _minigameUI.CutDalgonaPanelUI.StartTutorial();
                        _needleController.Active(_dalgonaController.GetCurrentDalgona());
                        _breakDalgonaController.SetDalgona(_dalgonaController.GetCurrentDalgona());
                    });

            }

        }
        // step 3
        public void StartBreakDalgonaStep(float delayTime = 0)
        {
            Input.multiTouchEnabled = false;
            _botController.SetActiveAllBot(false);
            _timeController.StopCountDown(TimerType.CutStep);
            _currentStepState = StepState.BreakDalgona;
            _minigameUI.SetActiveAllPanel(false);
            _needleController.Deactivate();
            _breakDalgonaController.Active(delayTime);
        }

        public string GetKeySaveLevel()
        {
            string key = _gameMode == EGameMode.Challenge ? CHALLENGE_LEVEL_KEY : MINIGAME_LEVEL_KEY;
            return key;
        }

        private void ChangeMap()
        {
            foreach (var map in _mapDic)
            {
                map.Value.SetActive(map.Key == _currentMapType);
            }
        }

        public void WinLevel()
        {
            _minigameUI.SetActiveAllPanel(false);
            _minigameUI.CloseSelf();
            _playerController.OnWin();
            GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
            PlayerPrefs.SetInt(GetKeySaveLevel(), PlayerPrefs.GetInt(GetKeySaveLevel()) + 1);
            _outroController.DoOutro(2f, 3.2f, () =>
            {
                GameManager.I.Win();
            });
        }

        public void LoseLevel()
        {
            _minigameUI.CloseSelf();
            _minigameUI.SetActiveAllPanel(false);
            _outroController.DoOutro(1f, 1.6f, async () =>
            {
                _guard.LookAtTarget(_playerController.transform).BaseGuard.PlayShootAnim().ShowLine(0, _playerController.TfHead).ClearLine(0.1f);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_M4_SHOOT);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
                _playerController.OnLose();
                await UniTask.Delay(500, cancellationToken: destroyCancellationToken);
                _losingAudioSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_LOSING);
                GameManager.I.Lose();
            });
        }


        [Button]
        public void StartDebug(int level = 1)
        {
            _minigameUI = UIManager.I.Open<Minigame03MenuUI>(Core.Define.UIName.MINIGAME_03_MENU);
            Init();
            PlayerPrefs.SetInt(GetKeySaveLevel(), level);
            StartPlayerStep();
        }

        [Button]
        public void Replay()
        {
            _losingAudioSource?.Stop();
            _minigameUI = UIManager.I.Open<Minigame03MenuUI>(Core.Define.UIName.MINIGAME_03_MENU);
            _playerController.SwitchPlayerAnimation(EAnimStyle.Idle);
            _playerController.OnReplay();
            _cameraController.ResetAllVirtualCamPosition();
            ResetMinigame();
            switch (_currentStepState)
            {
                case StepState.Player:
                    break;
                case StepState.CutDalgona:
                    StartCutPagonalStep();
                    break;
                case StepState.BreakDalgona:
                    _dalgonaController.SpawnDalgona();
                    _breakDalgonaController.SetDalgona(_dalgonaController.GetCurrentDalgona());
                    _dalgonaController.GetCurrentDalgona().gameObject.SetActive(true);
                    _needleController.CompleteLine();
                    StartBreakDalgonaStep();
                    break;
            }
        }

        private void ResetMinigame()
        {
            _dalgonaController.GetCurrentDalgona().gameObject.SetActive(false);
            _needleController.ResetNeedle();
            _breakDalgonaController.ResetBreak();
        }
    }

    public enum MapType
    {
        Map_1 = 0,
        Map_2 = 1
    }

    public enum StepState
    {
        Player,
        CutDalgona,
        BreakDalgona,
    }
}
