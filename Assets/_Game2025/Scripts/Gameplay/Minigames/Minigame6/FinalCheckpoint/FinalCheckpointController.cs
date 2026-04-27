using Cinemachine;
using Cysharp.Threading.Tasks;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6.FinalCheckpoint
{
    public class FinalCheckpointController : CheckpointController
    {
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private GameObject _fxGroup;

        public override void OnEnter()
        {
            Win().Forget();
        }

        public override void OnExit()
        {
        }

        public override void OnRevive()
        {
        }

        public async UniTaskVoid Win()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_DESTINATION);

            _camera.gameObject.SetActive(true);
            await UniTask.WaitUntil(() => !_cinemachineBrain.IsBlending);

            _fxGroup.SetActive(true);
            foreach (var character in _charGroup)
            {
                var animator = character.Model.GetCom<CharacterAnimator>();
                animator.PlayAnimation(EAnimStyle.Victory_1, EAnimStyle.Victory_2, EAnimStyle.Victory_3);
            }

            var delay = 2f;
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            _controller.Win();
        }
    }
}
