using Cysharp.Threading.Tasks;
using DG.Tweening;
using NFramework;
using Redcode.Extensions;
using SquidGame.Core;
using SquidGame.LandScape;
using SquidGame.Minigame01;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game1
{
    public class HunterController : MonoBehaviour
    {
        public static event Action<bool> OnEnemySinging;
        public static event Action<bool> OnWarning;

        [SerializeField] protected List<MG01Enemy> _enemies;

        [Header("CONFIG")]
        [SerializeField] protected float _startPitch = 1f;
        [SerializeField] protected Transform _headTf;
        public bool IsSilent => _isSilent;
        public bool IsRotateToBot => _isRotateToBot;

        protected bool _isSilent;
        protected float _curPitch = 1;
        protected int _pitchIncreaseTimeCount = 0;
        protected bool _isStopGame;
        protected bool _isRotateToBot;
        protected PlayerController _playerController;
        protected Game1Control _controller;
        protected BotManager _botManager;

        public void Init(Game1Control controller, BotManager botManager)
        {
            _controller = controller;
            _botManager = botManager;
            _playerController = _controller.PlayerController;
            _pitchIncreaseTimeCount = 0;
            _curPitch = 1;
            _headTf.eulerAngles = Vector3.zero;
        }

        public void Rotate()
        {
            _headTf.DOKill();
            _headTf.DOLocalRotate(new Vector3(0, -180f, 0), 2f);
        }

        public void StartGame()
        {
            StartCoroutine(CRSingBehaviour());
        }

        public virtual IEnumerator CRSingBehaviour()
        {
            while (_playerController.IsPlaying)
            {
                _isSilent = false;
                _isRotateToBot = false;

                OnEnemySinging?.Invoke(true);

                // Pitch when singing
                if (_pitchIncreaseTimeCount < 5) _pitchIncreaseTimeCount++;

                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_SING, pitch: _curPitch);
                if (_curPitch < 2)
                {
                    _curPitch = _startPitch + (0.2f * _pitchIncreaseTimeCount);
                }

                // Time singing
                var timeSinging = 3.1f / _curPitch;

                while (timeSinging > 0f)
                {
                    timeSinging -= Time.deltaTime;
                    yield return null;
                }

                // Time over (Rotate and scan for moving bot)
                yield return _headTf.DOLocalRotate(Vector3.zero, 0.25f).WaitForCompletion();
                yield return CRScanning();
                yield return _headTf.DOLocalRotate(new Vector3(0, -180, 0), 0.25f).WaitForCompletion();
            }
        }

        public void SetActive(bool isActive)
        {
            _isStopGame = !isActive;
            OnWarning?.Invoke(_isStopGame);

            if (_isStopGame)
            {
                _isSilent = false;
            }
        }


        private IEnumerator CRScanning()
        {
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Warning);
            _isSilent = true;
            _isRotateToBot = true;
            OnEnemySinging?.Invoke(false);

            if (!_isStopGame)
            {
                OnWarning?.Invoke(true);
            }

            yield return CRKillBots();
        }


        public virtual IEnumerator CRKillBots()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_SEARCHING);
            var waiter = new WaitUntil(() => _isRotateToBot);
            yield return waiter;

            var randomBot = UnityEngine.Random.Range(1, 4);
            var bots = _botManager.PickRandomeAliveBots(randomBot);
            var enemies = _enemies.PickRandom(bots.Count);
            enemies.ForEach(x => x.PlayShootAnim());
            yield return new WaitForSeconds(0.3f);
            ShootLaser(bots, enemies);
            bots.ForEach(bot => bot.HandleDie());
            yield return new WaitForSeconds(0.3f);
            enemies.ForEach(enemy => enemy.ShootCompleted());
            yield return new WaitForSeconds(2f);
            _isRotateToBot = false;
        }


        private void ShootLaser(List<Bot> bots, List<MG01Enemy> enemies)
        {
            for (int i = 0; i < bots.Count; i++)
            {
                enemies[i].ShootBot(bots[i].HeadPos);
            }
        }


        private MG01Enemy GetFreeEnemy()
        {
            return _enemies.Find(x => !x.IsShooting);
        }


        public async UniTaskVoid ShootSingle(Transform headPos)
        {
            var enemy = GetFreeEnemy();
            if (enemy == null) return;
            enemy.PlayShootAnim();
            await UniTask.Delay(300);
            enemy.ShootBot(headPos);
            await UniTask.Delay(300);
            enemy.ShootCompleted();
        }
    }
}