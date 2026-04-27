using System.Collections;
using Cinemachine;
using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Data;
using SquidGame.LandScape.Game;
using UnityEngine;

namespace SquidGame.LandScape.Lobby
{
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;

        //--- CAMERA ---
        [SerializeField] private CinemachineFreeLook _cinemachineFreeLook;
        [SerializeField] private Transform[] _introPositions;

        //--- COIN ---
        [SerializeField] private PiggyBreakCutScene _piggyBreakCutScene;
        [SerializeField] private GameObject _coinObject;
        [SerializeField] private Transform _coinSpawnPosition;
        [SerializeField] private MeshFilter _coinBaseFilter;
        [SerializeField] private MeshCollider _coinBaseCollider;
        [SerializeField] private Mesh[] _coinBaseMeshes;


        //--- BOT ---
        [SerializeField] private BotManager _botManager;

        //--- SEASON ---
        private UserData _userData;
        private SeasonProgressData _seasonProgressData;
        private int _seasonId;

        Transform _cameraTransform;
        Quaternion _rdCoinRotation;
        LobbyUI _lobbyUI;
        AudioSource _coinDropSound;

        void Awake()
        {
            Init();
            if (_seasonProgressData.IsSeasonMaxProgress)
                ShowCutSceneWhenCompleteSeason();
            else
            {
                StartIntro(_seasonProgressData.Progress);
                _botManager.Initialize(_seasonProgressData.Progress);
            }
        }

        void Init()
        {
            _cameraTransform = Camera.main.transform;
            _cinemachineFreeLook.gameObject.SetActive(false);

            _seasonId = GameManager.I.CurSeasonId;
            _userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
            _seasonProgressData = _userData.GetSeasonData(_seasonId);

            _lobbyUI = UIManager.I.Open<LobbyUI>(Define.UIName.LOBBY_MENU);
            _lobbyUI.Init(_playerController, _cinemachineFreeLook);
        }

        void StartIntro(int seasionMinigameIndex)
        {
            _cameraTransform.position = _introPositions[0].position;

            Sequence sq = DOTween.Sequence();
            // sq.AppendInterval(0.5f);
            sq.AppendCallback(() => StartCoroutine(SpawningCoin(seasionMinigameIndex)));
            sq.Append(_cameraTransform.DOMove(_introPositions[1].position, 1.8f).OnComplete(() =>
            {
                if (seasionMinigameIndex > 0)
                    _coinDropSound = GameSound.I.PlaySFX(Define.SoundPath.SFX_LOBBY_COIN_DROP_2);
            }))
            .Join(_cameraTransform.DORotate(_introPositions[1].rotation.eulerAngles, 1.8f));
            // sq.Append(_cameraTransform.DOMove(_introPositions[2].position, 1.2f).SetEase(Ease.Linear))
            //   .Join(_cameraTransform.DORotate(_introPositions[2].rotation.eulerAngles, 1.2f));
            sq.AppendInterval(seasionMinigameIndex > 0 ? 2.5f : 0.5f);
            sq.AppendCallback(() => _coinDropSound?.Stop());
            sq.Append(_cameraTransform.DOMove(_introPositions[2].position, 1.5f))
              .Join(_cameraTransform.DORotate(_introPositions[2].rotation.eulerAngles, 1.5f));
            sq.AppendCallback(() =>
            {
                _cinemachineFreeLook.gameObject.SetActive(true);
                _lobbyUI.gameObject.SetActive(true);
                _playerController.SetActive();
            });
        }

        void ShowCutSceneWhenCompleteSeason()
        {
            _userData.CompleteSeason(_seasonId);
            _lobbyUI.ShowVictoryScreen();
            //piggy go kabum
            _playerController.transform.eulerAngles = new Vector3(0, 180, 0);
            Sequence sq = DOTween.Sequence();

            sq.AppendCallback(() =>
            {
                StartCoroutine(SpawningCoin(_coinBaseMeshes.Length - 1));
                _coinBaseFilter.mesh = _coinBaseMeshes[^1];
                _coinBaseCollider.sharedMesh = _coinBaseMeshes[^1];
            });
            sq.Append(_cameraTransform.DOMove(_introPositions[1].position, 1.8f).OnComplete(() =>
            {
                _coinDropSound = GameSound.I.PlaySFX(Define.SoundPath.SFX_LOBBY_COIN_DROP_2);
            }))
            .Join(_cameraTransform.DORotate(_introPositions[1].rotation.eulerAngles, 1.8f));
            sq.AppendInterval(2f);
            sq.AppendCallback(() =>
            {
                _playerController.PlayAnimationDance();
                _piggyBreakCutScene.Show();
                GameSound.I.PlaySFX(Define.SoundPath.SFX_LOBBY_SMASH);
                _coinDropSound = GameSound.I.PlaySFX(Define.SoundPath.SFX_LOBBY_COIN_DROP);
            });

            sq.AppendInterval(0.5f);
            sq.Append(_cameraTransform.DOMove(_introPositions[2].position, 1.5f))
              .Join(_cameraTransform.DORotate(_introPositions[2].rotation.eulerAngles, 1.5f));
            sq.AppendInterval(5f);
            sq.AppendCallback(() => _lobbyUI.ShowOutro(_seasonId));
            sq.AppendInterval(3.5f);
            sq.AppendCallback(() =>
            {
                GameManager.I.Exit();
                _coinDropSound?.Stop();
            });
        }

        IEnumerator SpawningCoin(int seasionMinigameIndex)
        {
            ActiveCoinBaseSinceMinigame2();
            if (seasionMinigameIndex > 0)
            {
                yield return new WaitForSeconds(0.5f);
                if (!_seasonProgressData.IsSeasonMaxProgress)
                {
                    for (int i = 0; i < Random.Range(12, 20); i++)
                    {
                        _rdCoinRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                        Instantiate(_coinObject, _coinSpawnPosition.position, _rdCoinRotation, _coinSpawnPosition.transform);
                        yield return new WaitForSeconds(0.1f);
                    }
                }
            }

            void ActiveCoinBaseSinceMinigame2()
            {
                seasionMinigameIndex = Mathf.Clamp(seasionMinigameIndex, 0, _coinBaseMeshes.Length);

                if (seasionMinigameIndex >= 2)
                {
                    _coinBaseFilter.gameObject.SetActive(true);
                    _coinBaseFilter.mesh = _coinBaseMeshes[seasionMinigameIndex - 2];
                    _coinBaseCollider.sharedMesh = _coinBaseMeshes[seasionMinigameIndex - 2];
                }
            }
        }

        public void PlayNextMinigame()
        {
            var seasonConfig = GameConfig.I.SeasonConfigSO.GetSeasonConfig(_seasonId);
            var minigameConfig = seasonConfig.MinigameList[_seasonProgressData.Progress];
            UIManager.I.Close(Define.UIName.LOBBY_MENU);
            GameManager.I.LoadMinigame(minigameConfig.Id);
        }
    }
}
