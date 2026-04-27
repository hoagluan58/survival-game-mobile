using DG.Tweening;
using DinoFractureDemo;
using NFramework;
using Redcode.Extensions;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.UI;
using SquidGame.Minigame21;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    public abstract class GameBase : MonoBehaviour
    {
        public virtual void OnLoadMinigame() { }
        public virtual void OnLose() { }
        public virtual void OnReload() { }
        public virtual void OnStart() { }
        public virtual void OnRevive() { }
        public virtual void OnWin() { }
    }


    public class MinigameController : BaseMinigameController
    {
        [Header("config")]
        [SerializeField] private int _stepIndex;
        [SerializeField][Range(0, 1)] private float _removeNpcInRoundPercent;

        [Header("references")]
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private PlayerController _playerController;

        [SerializeField] private NpcManager _npcManager;
        [SerializeField] private PodiumController _podiumController;
        [SerializeField] private RoomManager _roomManager;
        [SerializeField] private GuardManager _guardManager;
        [SerializeField] private RingAreaSpawner _ringAreaSpawner;

        private LevelContent _currentLevelContent;
        private Coroutine _currentRoundCoroutine;
        private MinigameMingleMenuUI _ui;
        private PlaySoundFx _rotateSound;
        private PlaySoundFx _tensionSound;
        private DontPlayIfPlaying _countdownSound;

        private int _currentPeopleLimit;

        #region OVERRIDE
        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            _cameraController.OnInitialized();
            _ui = UIManager.I.Open<MinigameMingleMenuUI>(Define.UIName.MINIGAME_MINGLE_MENU);
            _playerController.Init(_ui.VariableJoystick);
            _ui.InitFreeLookController(_cameraController.CinemachineFreeLook).InitJumpButton(_playerController.Jump).Init();
            _npcManager.Init();
            _roomManager.Init();
            _stepIndex = 0;
            _currentLevelContent = _levelConfig.GetLevelContent(_stepIndex);
            _currentPeopleLimit = Random.Range(_currentLevelContent.PeopleCountMin, _currentLevelContent.PeopleCountMax);
            _rotateSound = new PlaySoundFx(Define.SoundPath.SFX_MINGLE_RORATE);
            _tensionSound = new PlaySoundFx(Define.SoundPath.SFX_MINGLE_TENSION);
            _countdownSound = new DontPlayIfPlaying(Define.SoundPath.SFX_COUNT_DOWN_8s);
            GameManager.I.StartMinigame();
            
        }

 

        public override void OnLose()
        {
            base.OnLose();
        }

        public override void OnReload()
        {
            base.OnReload();
        }

        public override void OnStart()
        {
            base.OnStart();
            _rotateSound.PlaySound(false);
            _tensionSound.PlaySound(true);
            _npcManager.GoToPlatform();
            _cameraController.PlayIntro(() =>
            {
                _cameraController.SwitchCamera(CameraType.Player);
                _ui.ShowScene(true, 1f);
                _ui.ShowInput(false, 0f);
                StartCoroutine(CRWaitStartgame());
            });
        }


        private void OnZoomInCompleted()
        {
            StartRound();
        }


        private void StartRound()
        {
            if (_currentRoundCoroutine != null)
                StopCoroutine(_currentRoundCoroutine);
            _currentRoundCoroutine = StartCoroutine(CRStartRound());
        }


        private void StopRound()
        {
            if (_currentRoundCoroutine != null)
            {
                StopCoroutine(_currentRoundCoroutine);
                _currentRoundCoroutine = null;
            }

            _tensionSound.PlaySound(false);
            _rotateSound.PlaySound(false);
        }


        private void NextRound()
        {
            _stepIndex = Mathf.Min(_stepIndex + 1, _levelConfig.LevelContents.Count - 1);
            _currentLevelContent = _levelConfig.GetLevelContent(_stepIndex);
            _currentPeopleLimit = Random.Range(_currentLevelContent.PeopleCountMin, _currentLevelContent.PeopleCountMax);
            StartCoroutine(CRSpin());
        }




        private void OnDestroy()
        {
            _rotateSound.PlaySound(false);
            _tensionSound.PlaySound(false);
            _ui?.CloseSelf(true);
        }

        [SerializeField] private Transform _winPos;

        private bool CheckWinGame()
        {
            var nextStep = _stepIndex + 1;
            var win = nextStep >= _levelConfig.LevelContents.Count;
            if (win)
            {
                StopRound();

                _ui?.CloseSelf(true);
                _npcManager.gameObject.SetActive(false);
                _playerController.SetActive(false);
                var clone = _playerController.GetInstantiateCharacter();
                _playerController.gameObject.SetActive(false);
                clone.transform.position = _winPos.position;
                clone.transform.localScale = Vector3.one * 2;
                clone.transform.LookAt(Vector3.zero);

                DOVirtual.DelayedCall(0.1f, () =>
                {
                    clone.PlayAnimation(EAnimStyle.Victory_1,EAnimStyle.Victory_2,EAnimStyle.Victory_3);
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
                });
                DOVirtual.DelayedCall(4, () => GameManager.I.Win());
                _cameraController.PlayWinCamera();
            }
            return win;
        }


        [Button]
        public override void OnRevive()
        {
            base.OnRevive();

            StartCoroutine(CRRevive());
        }


        private IEnumerator CRRevive()
        {
            UIManager.I.Close(Define.UIName.LOSE_CHALLENGE_POPUP);
            GameManager.I.TransitionUI.SetActiveLoadingIMG(true).FadeScreenDuration(1f, 1f);
            yield return new WaitForSeconds(1f);
            _playerController.Revive();
            _playerController.transform.position = _ringAreaSpawner.GetRandomPointPodium().WithY(0.1f);
            _npcManager.ReviveAllNpcsInRound(_stepIndex);

            GameManager.I.TransitionUI.SetActiveLoadingIMG(true).FadeScreenDuration(0f, 1f);
            yield return new WaitForSeconds(2f);
            GameManager.I.TransitionUI.SetActiveLoadingIMG(false);
            _ui.ShowScene(true, 0f);
            StartRound();
        }


        public override void OnWin()
        {
            base.OnWin();
        }
        #endregion

        #region CROUTINES

        private IEnumerator CRWaitStartgame()
        {
            while (!_npcManager.AllNpcInPodium() || !_playerController.IsInPodium())
            {
                yield return null;
            }
            yield return CRSpin();
        }

        private IEnumerator CRSpin()
        {
            _tensionSound.PlaySound(false);
            _rotateSound.PlaySound(true);
            _roomManager.OpenRooms(false, 0.5f);
            _roomManager.SetActiveRooms(_currentLevelContent.RoomCount);
            _podiumController.SetActiveBlock(true);

            yield return new WaitForSeconds(1.5f);

            _playerController.SetParent(_podiumController.GetPodium(), true);
            _podiumController.Rotate(8f);
            _playerController.SetActive(true);
            _npcManager.SetContainer(_podiumController.GetPodium());

            _ui.ShowInput(true, 1f);
            _ui.Panel.Show(1f);

            yield return CRStartgameCountdown(8);

            _npcManager.SetContainer(_npcManager.transform);
            _playerController.SetParent(transform, false);
            _ui.ShowInput(false, 0.5f);
            _playerController.SetActive(false);

            _roomManager.ZoomInRoom(_currentPeopleLimit, OnZoomInCompleted);
        }

        private IEnumerator CRStartgameCountdown(int time)
        {
            while (time > 0)
            {
                _ui.Panel.SetText($"Round start in {time} {(time != 1 ? "seconds" : "second")}");
                yield return new WaitForSeconds(1);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MINGLE_TICK);
                time--;
            }
            _ui.Panel.Hide(1f);
            _ui.ShowInput(false, 0f);
        }

        private IEnumerator CRStartRound()
        {
            _rotateSound.PlaySound(false);
            _tensionSound.PlaySound(true);
            _ui.ShowInput(true, 0.5f);
            _playerController.SetActive(true);
            _npcManager.StartStep(_currentPeopleLimit);
            _ui.Panel.SetText($"{_currentPeopleLimit} players !").Show(1f);
            _roomManager.StartStep(_currentPeopleLimit);
            _roomManager.OpenRooms(true, 0.5f);
            _roomManager.SetActiveBlocks(false);
            yield return CRCountdownMoveInRoom(_currentLevelContent.Time);
            yield return CRCheckPlayerOutsideRoom();
            yield return CRPlaySoundShoot();
            yield return CRCheckPlayerInsideRoom();
            yield return CRWaitBackToPodium();
            yield return CRCheckPlayerOutsidePodium();
            NextRound();

        }


        private IEnumerator CRPlaySoundShoot()
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 3; i++)
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
                var delay = Random.Range(0.1f, 0.3f);
                yield return new WaitForSeconds(delay);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
            }
            _npcManager.KillAllWanderNpc(_stepIndex);
            yield return new WaitForSeconds(1f);
        }

        private IEnumerator CRCountdownMoveInRoom(int time)
        {
            yield return new WaitForSeconds(2);
            while (time > 0)
            {
                _ui.Panel.SetText($"{time} seconds left").Show(1f);
                if (time == 8) _countdownSound.PlaySound(true);
                yield return new WaitForSeconds(1);
                time--;
            }
            _countdownSound.PlaySound(false);
            _ui.Panel.Hide(0.5f);
            _tensionSound.PlaySound(false);
            _roomManager.OpenRooms(false, 0.5f);
            _roomManager.SetActiveBlocks(true);
            yield return new WaitForSeconds(0.6f);
        }

        private IEnumerator CRCheckPlayerOutsideRoom()
        {
            var room = _roomManager.GetPlayerRoom();
            if (!room)
            {
                //player out of room
                _ui.ShowScene(false, 0);
                _guardManager.KillPlayer(_playerController.Head);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
                _playerController.Dead();
                DOVirtual.DelayedCall(2f, () => GameManager.I.Lose());
                StopRound();
                yield break;
            }
        }

        private IEnumerator CRCheckPlayerInsideRoom()
        {
            var room = _roomManager.GetPlayerRoom();
            _ui.ShowScene(false, 0.5f);
            _playerController.SetActive(false);
            _playerController.Stanstill();
            var isOk = room.CheckRoom();
            var guard = _guardManager.GetGuard();
            var point = room.OutPoint;
            guard.PlayWalkAnimation();
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MINGLE_OPEN_DOOR);
            room.OpenDoor(true, 0.5f);

            var moveTime = 2f;
            guard.transform.SetParent(point);
            guard.transform.localPosition = Vector3.forward * -10f;
            guard.transform.LookAt(point);
            guard.transform.DOLocalMoveZ(0, moveTime);
            yield return new WaitForSeconds(moveTime);
            guard.PlayIdleAnimation();
            guard.transform.DOLocalRotate(new Vector3(0, -100, 0), 0.5f);
            yield return new WaitForSeconds(1f);
            if (!isOk)
            {
                guard.LookAt(_playerController.Head).PlayShootAnim().ShowLine(0.25f, _playerController.Head).ClearLine(0.45f);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
                _playerController.Dead();
                DOVirtual.DelayedCall(2f, () => GameManager.I.Lose());

                yield return new WaitForSeconds(1f);
                guard.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
                yield return new WaitForSeconds(0.5f);
                guard.PlayWalkAnimation();
                guard.transform.DOLocalMoveZ(10, 4).SetEase(Ease.Linear);
                yield return new WaitForSeconds(4);
                Destroy(guard.gameObject);
                StopRound();
                yield break;
            }
            yield return new WaitForSeconds(1f);
            guard.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
            yield return new WaitForSeconds(0.5f);
            guard.PlayWalkAnimation();
            guard.transform.DOLocalMoveZ(10, 4).SetEase(Ease.Linear);
            yield return new WaitForSeconds(4);
            Destroy(guard.gameObject);
            CheckWinGame();
        }

        private IEnumerator CRWaitBackToPodium()
        {
            _roomManager.SetActiveBlocks(false);
            _tensionSound.PlaySound(true);
            var time = 12;
            _npcManager.RemoveLiveNpcs(_removeNpcInRoundPercent, _stepIndex);
            _npcManager.ClearDieNpcs();
            _roomManager.OpenRooms(true, 0.5f);
            _roomManager.ResetTextRoom();
            _ui.ShowScene(true, 0.5f);
            _playerController.SetActive(true);
            yield return new WaitForSeconds(1);
            _ui.Panel.SetText($"Return to platform !").Show(1f);
            _npcManager.GoToPlatform();
            yield return new WaitForSeconds(1);
            while (time > 0)
            {
                _ui.Panel.SetText($"{time} seconds to return !").Show(1f);
                yield return new WaitForSeconds(1);
                time--;
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MINGLE_TICK);
            }
            _ui.Panel.Hide(0.5f);
        }

        private IEnumerator CRCheckPlayerOutsidePodium()
        {
            var position = _playerController.transform.position;
            var area = _ringAreaSpawner.GetAreaType(position);
            if (area == AreaType.Ring)
            {
                //player out of room
                _guardManager.KillPlayer(_playerController.Head);
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
                _ui.ShowScene(false, 0);
                _playerController.Dead();
                DOVirtual.DelayedCall(2f, () => GameManager.I.Lose());
                StopRound();
                yield break;
            }
            _npcManager.KillAllNpcOutsidePodium();
        }
        #endregion
    }
}
