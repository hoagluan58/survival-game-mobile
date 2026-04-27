using Cinemachine;
using Cysharp.Threading.Tasks;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class SurvivalManager : BaseMinigameController
    {
        [Header("CONTROLLER")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private BotController _botController;
        // [SerializeField] private PositionController _positionController;
        [SerializeField] private CinemachineFreeLookInput _cinemachineFreeLookInput;

        [Header("SAVE")]
        [SerializeField] private SaveLevel _saveLevel;

        private AudioSource _losingAudioSource;
        private List<IInfomation> _userList;
        private MinigameSurvivalUI _minigameSurvivalUI;

        private void OnDestroy()
        {
            PlayerController.OnPlayerDead -= OnPlayerDead;
            _minigameSurvivalUI.CloseSelf(true);
        }

        private void OnPlayerDead()
        {
            LoseLevel();
        }

        public List<IInfomation> Userlist
        {
            get
            {
                GetAllUser();
                return _userList;
            }
        }
        // public PositionController PositionController => _positionController;
        // public WeaponController WeaponController => _weaponController;
        int _maxBotAmount;


        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            _minigameSurvivalUI = UIManager.I.Open<MinigameSurvivalUI>(Define.UIName.MINIGAME_SURVIVAL_MENU);
            _minigameSurvivalUI.Init(_playerController, _cinemachineFreeLookInput);
            _botController.Init(this);
            // _weaponController.Init(this);
            // _playerController.Init(this);

            GameManager.I.StartMinigame();
            PlayerController.OnPlayerDead += OnPlayerDead;
        }

        public override void OnStart()
        {
            base.OnStart();
            _maxBotAmount = _botController.SpawnAllBot(_saveLevel.GetCurrentLevel()) + 1;
            _minigameSurvivalUI.UpdateLevelText(_saveLevel.GetCurrentLevel());
            _minigameSurvivalUI.ResetGame();

            StartCoroutine(CRDelaySpawnWeapon());

            IEnumerator CRDelaySpawnWeapon()
            {
                int _startTime = 5;
                while (_startTime > 0)
                {
                    _minigameSurvivalUI.UpdateStartText($"Game start in {_startTime} seconds");
                    _startTime--;
                    yield return new WaitForSeconds(1f);
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_TICK);
                }

                _minigameSurvivalUI.UpdateStartText($"Start!");
                UpdateAmountText();

                yield return new WaitForSeconds(1f);
                _minigameSurvivalUI.StartGame();
                WeaponController.I.SpawnAllWeapon(_saveLevel.GetCurrentLevel());
            }
        }

        public async void WinLevel()
        {
            _minigameSurvivalUI.CloseSelf();
            await UniTask.Delay(1000, cancellationToken: destroyCancellationToken);
            _playerController.Win();
            GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
            _saveLevel.InscreaseLevel();
            await UniTask.Delay(2000, cancellationToken: destroyCancellationToken);
            GameManager.I.Win();
        }

        public void LoseLevel()
        {
            _minigameSurvivalUI.CloseSelf();
            UpdateAmountText();
            _losingAudioSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_LOSING);
            GameManager.I.Lose();
        }

        public override void OnRevive()
        {
            Replay();
        }

        public void Replay()
        {
            _minigameSurvivalUI = UIManager.I.Open<MinigameSurvivalUI>(Define.UIName.MINIGAME_SURVIVAL_MENU);
            _losingAudioSource?.Stop();
            _playerController.Revive();
            UpdateAmountText();
        }

        public void GetAllUser()
        {
            _userList = new List<IInfomation>(_botController.GetAllInformation());
            if (_playerController.IsDead()) return;
            _userList.Add(_playerController);
        }

        public void UpdateAmountText()
        {
            _minigameSurvivalUI.UpdateAmountText($"{GetCurrent()}/{_maxBotAmount}");

            int GetCurrent()
            {
                return _botController.GetBotCount() + (_playerController.IsDead() ? 0 : 1);
            }
        }

        public void DoHurtEffect()
        {
            _minigameSurvivalUI?.DoHurtEffect();
        }
    }
}
