using System.Collections;
using Animancer;
using DG.Tweening;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using UnityEngine;

namespace SquidGame.Minigame14
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private bool _isPlayer;
        [SerializeField] private Transform _marbles;

        [Header("ANIMATOR")]
        [SerializeField] private BaseCharacter _model;
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private ClipTransition _throwAnimClip;
        [SerializeField] private AnimationClip[] _winAnimClips;

        [Header("FX")]
        [SerializeField] private ParticleSystem _bloodFx;

        public Transform Marbles => _marbles;

        private void Start()
        {
            _model.Animator.PlayAnimation(EAnimStyle.Idle);
        }

        public IEnumerator ThrowMarblesCoroutine()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG04_THROW_MARBLE);
            _animancer.Play(_throwAnimClip);
            yield return new WaitForSeconds(0.7f);
            _marbles.gameObject.SetActive(true);
        }

        public void PlayLoseAnimation()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
            _bloodFx.Play();
            _model.Animator.PlayAnimation(EAnimStyle.Die);
        }

        public void PlayWinAnimation()
        {
            if (_isPlayer)
            {
                transform.DORotate(Vector3.up * 180f, 0.2f)
                    .OnComplete(() => _animancer.Play(_winAnimClips.RandomItem()));
            }
            else
            {
                _animancer.Play(_winAnimClips.RandomItem());
            }
        }
    }
}