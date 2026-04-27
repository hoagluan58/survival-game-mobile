using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.UI;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private List<Character> _charGroup;

        public List<Character> CharGroup => _charGroup;

        private AudioSource _audioSource;
        private List<CharacterAnimator> _characterAnimators = new List<CharacterAnimator>();
        private int _curCheckPoint;
        private CheckpointManager _checkpointManager;
        private bool _isMovingToCheckpoint = false;

        private void OnEnable()
        {
            SettingsPopupUI.OnPopupVisible += SettingsPopupUI_OnPopupVisible;
        }

        private void OnDisable()
        {
            SettingsPopupUI.OnPopupVisible -= SettingsPopupUI_OnPopupVisible;
            _audioSource?.Stop();
        }

        private void SettingsPopupUI_OnPopupVisible(bool value)
        {
            if (_isMovingToCheckpoint)
            {
                if (value)
                {
                    _audioSource?.Pause();
                }
                else
                {
                    _audioSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_CHEER, true);
                }
            }
        }

        public void Init(CheckpointManager checkpointManager)
        {
            _checkpointManager = checkpointManager;
            _curCheckPoint = -1;
            _charGroup.ForEach(c => _characterAnimators.Add(c.Model.GetCom<CharacterAnimator>()));
            PlayGroupAnim(EAnimStyle.Idle);
        }

        public void MoveToNextCheckpoint()
        {
            _isMovingToCheckpoint = true;
            _audioSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_CHEER, true);
            PlayGroupAnim(EAnimStyle.Running);
            _curCheckPoint++;
            _checkpointManager.MoveToCheckPoint(_curCheckPoint, OnReachNextCheckpoint);

            void OnReachNextCheckpoint()
            {
                PlayGroupAnim(EAnimStyle.Idle);
                _audioSource?.Stop();
                _isMovingToCheckpoint = false;
            }
        }

        private void PlayGroupAnim(EAnimStyle animStyle)
        {
            _characterAnimators.ForEach(c => c.PlayAnimation(animStyle));
        }

        public void OnRevive()
        {
            PlayGroupAnim(EAnimStyle.Idle);
        }
    }
}
