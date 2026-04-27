using Animancer;
using DG.Tweening;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using System.Collections;
using UnityEngine;

namespace SquidGame.Minigame17
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _fxBlood;

        [Header("ANIMATION")]
        [SerializeField] private BaseCharacter _model;
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private ClipTransition _runClip;
        [SerializeField] private ClipTransition[] _winClips;

        private void OnEnable()
        {
            _model.Animator.PlayAnimation(EAnimStyle.Idle);
        }

        public IEnumerator MoveTo(Vector3 destination, float duration)
        {
            yield return transform.DOLocalMove(destination, duration).OnUpdate(() =>
            {
                _animancer.Play(_runClip);
            }).WaitForCompletion();
            _model.Animator.PlayAnimation(EAnimStyle.Idle);
        }

        public IEnumerator CRWin()
        {
            var animWin = _winClips.RandomItem();
            transform?.DOKill();
            yield return DORotateY(-180f);
            _animancer.Play(animWin);
        }

        public void Die()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
            _model.Animator.PlayAnimation(EAnimStyle.Die);
            _fxBlood.Play();
        }

        public IEnumerator DORotateY(float y)
        {
            transform?.DOKill();
            yield return transform.DORotate(new Vector3(0, y, 0), 0.5f);
        }
    }
}
