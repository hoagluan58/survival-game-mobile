using System;
using System.Collections;
using Animancer;
using NFramework;
using Redcode.Extensions;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SquidGame.Minigame15
{
    public class Opponent : MonoBehaviour
    {
        public event Action EndTurnEvent;

        [SerializeField] private Transform _throwPoint;

        [Header("ANIMATOR")]
        [SerializeField] private BaseCharacter _model;
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private AnimationClip _throwAnimClip;
        [SerializeField] private AnimationClip _winAnimClip;

        [Header("FX")]
        [SerializeField] private ParticleSystem _bloodFx;

        private IMarbleThrowHandler _marbleThrowHandler;
        private Minigame15MenuUI _menuUI;

        private float _currentTurnScore;

        public float CurrentTurnScore => _currentTurnScore;

        public void Init(IMarbleThrowHandler marbleThrowHandler, Minigame15MenuUI minigame15MenuUI)
        {
            _model.Animator.PlayAnimation(EAnimStyle.Idle);
            _marbleThrowHandler = marbleThrowHandler;
            _menuUI = minigame15MenuUI;
        }

        public void OnStartTurn()
        {
            StartCoroutine(ThrowMarbleCoroutine());

            IEnumerator ThrowMarbleCoroutine()
            {
                _menuUI.PlayTextMessageAnimation(GameLocalization.I.GetStringFromTable("STRING_OPPONENT_TURN"));
                yield return new WaitForSeconds(1.5f);
                _menuUI.AnnouncerTMP.gameObject.SetActive(false);
                var throwDirection = new Vector3(Random.Range(-1f, 1f), 0.25f, 1f);
                throwDirection = throwDirection.normalized;
                var throwForce = Random.Range(5f, 15f);
                var state = _animancer.Play(_throwAnimClip);
                state.Events.OnEnd += () => _model.Animator.PlayAnimation(EAnimStyle.Idle);
                yield return new WaitForSeconds(0.7f);
                VibrationManager.I.Haptic(VibrationManager.EHapticType.Selection);
                var marbleThrow = _marbleThrowHandler.ThrowMarble(Side.Player, _throwPoint.transform.position, throwDirection, throwForce);
                yield return new WaitForSeconds(6f);
                _currentTurnScore = GetMarbleScore(marbleThrow);
                _menuUI.PlayShowScoreAnimation(_currentTurnScore);
                yield return new WaitForSeconds(2f);
                _menuUI.AnnouncerTMP.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.1f);
                EndTurn();
            }
        }

        private float GetMarbleScore(Marble marble)
        {
            var marblePosZ = marble.transform.position.z;
            var score = (marblePosZ - 10) / 0.45f;
            return score;
        }

        private void EndTurn()
        {
            EndTurnEvent?.Invoke();
        }

        public void PlayWinAnimation()
        {
            transform.SetEulerAnglesY(180);
            _animancer.Play(_winAnimClip);
        }

        public void PlayLoseAnimation()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
            _bloodFx.Play();
            _model.Animator.PlayAnimation(EAnimStyle.Die);
        }
    }
}