using System;
using System.Collections;
using System.Collections.Generic;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using UnityEngine;

namespace SquidGame.Minigame19
{
    public class MinigameController : BaseMinigameController
    {
        [Header("CONFIGS")]
        [SerializeField] private float _time;
        [SerializeField] private MapLimit _mapLimit;

        [Header("OBJECTS")]
        [SerializeField] private Timer _timer;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private List<BotController> _botControllerList;
        [SerializeField] private ParticleSystem _winFx;
        [SerializeField] private CannonManager _cannonManager;  

        private Minigame19MenuUI _minigame19MenuUI;

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();

            _minigame19MenuUI = UIManager.I.Open(Define.UIName.MINIGAME_19_MENU) as Minigame19MenuUI;

            // Init objects
            _cannonManager.Init();
            _playerController.Init(_mapLimit, _minigame19MenuUI.Joystick);
            _botControllerList.ForEach(b => b.Init(_mapLimit));
            _timer.Init(_minigame19MenuUI);
            _minigame19MenuUI.UpdateTimer(_time);

            // Register events
            _timer.OnTimeEnd += OnTimeOut;
        }

        public override void OnStart()
        {
            base.OnStart();

            _timer.StartTimer(_time);
            _playerController.SetActive(true);
            _botControllerList.ForEach(b => b.SetActive(true));
            _cannonManager.SetActive(true);
        }

        public void Win()
        {
            UIManager.I.Close(Define.UIName.MINIGAME_18_MENU);

            _cannonManager.SetActive(false);
            _timer.PauseTimer(true);
            _playerController.SetActive(false);
            _botControllerList.ForEach(b => b.SetActive(false));

            _playerController.OnWin();
            
            _winFx.gameObject.SetActive(true);
            _winFx.Play();

            StartCoroutine(WinCoroutine());
            return;

            IEnumerator WinCoroutine()
            {
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Success);
                var winSoundSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
                yield return new WaitForSeconds(4f);
                winSoundSource.Stop();
                GameManager.I.Win();
            }
        }

        public override void OnLose()
        {
            _timer.PauseTimer(true);
            _playerController.SetActive(false);
            _botControllerList.ForEach(b => b.SetActive(false));
            _cannonManager.SetActive(false);

            StartCoroutine(LoseCoroutine());
            return;

            IEnumerator LoseCoroutine()
            {
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Failure);
                yield return new WaitForSeconds(1f);
                TryShowRevivePopup(GameManager.I.Revive, () =>
                {
                    UIManager.I.Close(Define.UIName.MINIGAME_19_MENU);
                    GameManager.I.HandleResult();
                });
            }
        }

        public override void OnRevive()
        {
            base.OnRevive();
            _cannonManager.SetActive(true);
            _timer.PauseTimer(false);
            _playerController.SetActive(true);
            _botControllerList.ForEach(b => b.SetActive(true));
        }


        private void OnTimeOut() => Win();
    }

    [Serializable]
    public struct MapLimit
    {
        public Vector3 MapCenterPosition;
        public float MaxDistance;
    }
}