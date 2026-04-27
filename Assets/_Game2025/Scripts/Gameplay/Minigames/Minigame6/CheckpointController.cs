using Cysharp.Threading.Tasks;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.Minigame6.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6
{
    public abstract class CheckpointController : MonoBehaviour
    {
        protected MinigameUI _ui;
        protected MinigameController _controller;
        protected List<Character> _charGroup;

        public void Init(MinigameController controller, List<Character> charGroup, MinigameUI ui)
        {
            _controller = controller;
            _charGroup = charGroup;
            _ui = ui;
        }

        public abstract void OnEnter();

        public abstract void OnExit();

        public virtual void OnRevive()
        {
            ToggleGreyScale(false);
            foreach (var character in _charGroup)
            {
                character.OnRevive();
            }
        }

        public async UniTask KillAllCharacters(BaseGuard guard)
        {
            var delay = 0.3f;

            _charGroup.Shuffle();
            VibrationManager.I.Haptic(VibrationManager.EHapticType.MediumImpact);

            foreach (var character in _charGroup)
            {
                var rndDeadSound = UnityEngine.Random.value > 0.5f ? Define.SoundPath.SFX_MG01_HOSTAGE_F_HIT_01 : Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01;
                GameSound.I.PlaySFX(rndDeadSound);
                GuardShoot();
                character.Model.GetCom<CharacterAnimator>().PlayAnimation(EAnimStyle.Die);
                character.Model.ToggleGreyScale(true);
                character.OnDie();
                await UniTask.Delay(TimeSpan.FromSeconds(delay));

                void GuardShoot()
                {
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
                    guard.PlayShootAnim();
                    guard.ShowLine(0f, character.HeadPos);
                    guard.ClearLine(delay);
                }
            }

            _controller.Lose();
        }

        public void ToggleGreyScale(bool value)
        {
            foreach (var character in _charGroup)
            {
                character.Model.ToggleGreyScale(value);
            }
        }
    }
}
