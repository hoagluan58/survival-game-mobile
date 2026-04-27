using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System;
using System.Collections;
using UnityEngine;

namespace SquidGame.LandScape.MinigameRockPaperScissors
{
    public class Character : MonoBehaviour
    {
        [SerializeField] protected ParticleSystem _bloodFx;
        [SerializeField] protected CharacterAnimator _characterAnimator;

        private BaseCharacter _baseCharacter; 
        private Coroutine _coroutine;

        public virtual void Init()
        {
            PlayAnimation(EAnimStyle.Idle,0f);
            _baseCharacter = _characterAnimator.GetComponent<BaseCharacter>();
        }


        public IEnumerator CRPlayAnimation(params EAnimStyle[] eAnimStyle)
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            var anim = eAnimStyle.RandomItem();
            _characterAnimator.PlayAnimation(anim,fadeDuration : 0.2f);
            yield return new WaitForSeconds(_characterAnimator.GetLength(anim));
        }


        public void PlayAnimation(EAnimStyle eAnimStyle, float fadeDuration)
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            _characterAnimator.PlayAnimation(eAnimStyle, fadeDuration);
        }

        public void PlayAnimationOneTime(EAnimStyle startAnimation, EAnimStyle finishAnimation)
        {
            _characterAnimator.PlayAnimation(startAnimation, fadeDuration: 0.5f);
            var delay = _characterAnimator.GetLength(startAnimation);
            _coroutine = this.InvokeDelay(delay, () =>
            {
                _characterAnimator.PlayAnimation(finishAnimation,fadeDuration : 0.5f);
            });
        }

        public virtual void OnRevive() {
            PlayAnimation(EAnimStyle.Idle, 0f);
            _bloodFx.Stop();
            _bloodFx.gameObject.SetActive(false);
            _baseCharacter.ToggleGreyScale(false);
        }

        public virtual void Dead() {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
            PlayAnimation(EAnimStyle.Die,0f);
            _bloodFx.gameObject.SetActive(true);
            _bloodFx.Play();
            _baseCharacter.ToggleGreyScale(true);
        }
    }
}
