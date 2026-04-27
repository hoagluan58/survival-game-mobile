using SquidGame.Gameplay;
using SquidGame.Minigame13.UI;
using System.Collections;
using UnityEngine;

namespace SquidGame.Minigame13
{
    public class MinigameController : BaseMinigameController
    {
        [SerializeField] private Character _player;
        [SerializeField] private Character _enemy;
        [SerializeField] private BaseGuard _guard;

        [Header("CONTROLLER")]
        [SerializeField] private BattleController _battleController;
        [SerializeField] private CameraController _cameraController;

        [Header("UI")]
        [SerializeField] private Minigame13MenuUI _ui;
        [SerializeField] private ParticleSystem _winFx;

        public BattleController BattleController => _battleController;

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            _cameraController.Init(CameraController.ECameraType.Player);
            _battleController.Init(this, _ui);
            _ui.SetData(this);
            _player.Init();
            _enemy.Init();
        }

        public override void OnStart()
        {
            base.OnStart();
            StartCoroutine(CRStartGame());

            IEnumerator CRStartGame()
            {
                _cameraController.SwitchCamera(CameraController.ECameraType.Fight);
                yield return _player.CRRotateY(0);
                yield return _player.CRThrowMarbles();
                yield return _enemy.CRThrowMarbles();
                _ui.ShowPlayingPNL(true);
                _battleController.StartBattle();
            }
        }

        public override void OnWin()
        {
            base.OnWin();
            StartCoroutine(CRWinGame());

            IEnumerator CRWinGame()
            {
                _ui.gameObject.SetActive(false);
                _cameraController.SwitchCamera(CameraController.ECameraType.Player);
                yield return _player.CRRotateY(-180f);
                _enemy.PlayDeadAnim();
                _player.PlayWinAnim();
                _winFx.gameObject.SetActive(true);
                _winFx.Play();
                _guard.PlayShootAnim();
            }
        }

        public override void OnLose()
        {
            base.OnLose();
            StartCoroutine(CRLoseGame());

            IEnumerator CRLoseGame()
            {
                _ui.gameObject.SetActive(false);
                _cameraController.SwitchCamera(CameraController.ECameraType.Player);
                yield return _player.CRRotateY(-180f);
                _enemy.PlayWinAnim();
                _player.PlayDeadAnim();
                _guard.PlayShootAnim();
                yield return new WaitForSeconds(3f);
                GameManager.I.HandleResult();
            }
        }
    }
}
