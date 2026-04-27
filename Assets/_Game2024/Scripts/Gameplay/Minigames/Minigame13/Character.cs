using Animancer;
using DG.Tweening;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using System.Collections;
using UnityEngine;

namespace SquidGame.Minigame13
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private BaseCharacter _model;
        [SerializeField] private AnimancerComponent _animancer;

        [SerializeField] private ClipTransition _throwClip;
        [SerializeField] private ClipTransition[] _winClips;

        [SerializeField] private Transform _marbles;
        [SerializeField] private Transform _throwMarblesPos;
        [SerializeField] private ParticleSystem _fxBlood;

        public void Init()
        {
            _model.Animator.PlayAnimation(EAnimStyle.Idle);
            _marbles.gameObject.SetActive(false);
        }

        public IEnumerator CRThrowMarbles()
        {
            _marbles.gameObject.SetActive(false);
            var state = _animancer.Play(_throwClip);
            state.Events.OnEnd ??= () => _model.Animator.PlayAnimation(EAnimStyle.Idle);
            yield return new WaitForSeconds(0.8f);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG04_THROW_MARBLE);
            _marbles.gameObject.SetActive(true);
            _marbles.localEulerAngles = Vector3.zero;
            _marbles.SetParent(_throwMarblesPos);
            yield return _marbles.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear).WaitForCompletion();
        }

        public IEnumerator CRRotateY(float y)
        {
            var rot = new Vector3(transform.localEulerAngles.x, y, transform.localEulerAngles.z);
            yield return transform.DOLocalRotate(rot, 0.5f).WaitForCompletion();
        }

        public void PlayDeadAnim()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
            _fxBlood.Play();
            _model.Animator.PlayAnimation(EAnimStyle.Die);
        }

        public void PlayWinAnim()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
            _animancer.Play(_winClips.RandomItem());
        }
    }
}
