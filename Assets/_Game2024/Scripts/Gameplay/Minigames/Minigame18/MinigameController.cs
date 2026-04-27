using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using UnityEngine;

namespace SquidGame.Minigame18
{
    public class MinigameController : BaseMinigameController
    {
        [Header("CONFIGS")]
        [SerializeField] private float _obstacleSpeed = 80f;

        [Header("OBJECTS")]
        [SerializeField] private ParticleSystem _winFx;
        [SerializeField] private PlayerController _player;
        [SerializeField] private Obstacle _obstacle;
        [SerializeField] private List<BotController> _bots;
        [SerializeField] private List<Vector2> _jumpPatterns;

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            UIManager.I.Open(Define.UIName.MINIGAME_18_MENU);

            // Init obstacle
            _obstacle.Init(_obstacleSpeed);

            // Init bots
            var intervalObstacleSpin180 = 180f / _obstacleSpeed;
            foreach (var bot in _bots)
            {
                var randomItem = _jumpPatterns.RandomItem();
                _jumpPatterns.Remove(randomItem);

                var randomJumpTime = (int)Random.Range(randomItem.x, randomItem.y);
                var direction = (_obstacle.transform.position - bot.transform.position).normalized;
                var angle = Vector3.SignedAngle(_obstacle.transform.right, direction, Vector3.up);
                if (angle < 0) angle += 360f;
                if (angle > 180) angle -= 180;

                var firstJumpTime = angle / _obstacleSpeed - 0.2f;
                bot.Init(randomJumpTime, firstJumpTime, intervalObstacleSpin180);
                bot.DieEvent += OnBotDie;
            }
        }

        public override void OnStart()
        {
            base.OnStart();
            this.InvokeDelay(3f, () =>
            {
                _obstacle.SetActive(true);
                _player.SetActive(true);
                _bots.ForEach(b => b.SetActive(true));
            });
        }

        public override void OnLose()
        {
            base.OnLose();
            StartCoroutine(LoseCoroutine());
        }

        private void OnBotDie()
        {
            var isWin = _bots.All(b => !b.IsAlive);
            if (isWin) StartCoroutine(WinCoroutine());
        }

        private IEnumerator WinCoroutine()
        {
            // Disable objects
            _obstacle.SetActive(false);
            _player.SetActive(false);
            UIManager.I.Close(Define.UIName.MINIGAME_18_MENU);

            // Move cam
            Game4.CameraControl.I.MoveToPosCamWinLose(1f);
            yield return new WaitForSeconds(1f);

            // Play win animation
            _winFx.gameObject.SetActive(true);
            _winFx.Play();
            _player.OnWin();
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);
            var winSoundSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);

            yield return new WaitForSeconds(4f);

            winSoundSource.Stop();
            GameManager.I.Win();
        }

        private IEnumerator LoseCoroutine()
        {
            _obstacle.SetActive(false);
            _player.SetActive(false);
            _bots.ForEach(b => b.SetActive(false));
            UIManager.I.Close(Define.UIName.MINIGAME_18_MENU);
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Failure);
            yield return new WaitForSeconds(2f);
            GameManager.I.HandleResult();
        }
    }
}