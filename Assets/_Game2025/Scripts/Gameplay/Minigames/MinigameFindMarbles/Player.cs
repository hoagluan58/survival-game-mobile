using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using UnityEngine;

namespace SquidGame.LandScape.MinigameFindMarbles
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private BaseCharacter _model;
        [SerializeField] private Transform _headPos;
        [SerializeField] private GameObject _fxBloodSplat;
        [SerializeField] private GameObject _fxBloodPool;

        public Transform HeadPos => _headPos;

        private CharacterAnimator _animator;

        private void Start()
        {
            _animator = _model.GetCom<CharacterAnimator>();
            _animator.PlayAnimation(EAnimStyle.Idle);
        }

        public void OnWin() => _animator.PlayAnimation(EAnimStyle.Victory_1, EAnimStyle.Victory_2, EAnimStyle.Victory_3);

        public void OnDie()
        {
            var rndDeadSound = Random.value > 0.5f ? Define.SoundPath.SFX_MG01_HOSTAGE_F_HIT_01 : Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01;

            GameSound.I.PlaySFX(rndDeadSound);
            _animator.PlayAnimation(EAnimStyle.Die);
            _model.ToggleGreyScale(true);
            _fxBloodSplat.SetActive(true);
            _fxBloodPool.SetActive(true);
        }

        public void OnRevive()
        {
            _animator.PlayAnimation(EAnimStyle.Idle);
            _model.ToggleGreyScale(false);
            _fxBloodSplat.SetActive(false);
            _fxBloodPool.SetActive(false);
        }
    }
}
