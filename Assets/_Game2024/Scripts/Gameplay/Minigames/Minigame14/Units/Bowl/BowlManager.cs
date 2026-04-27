using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NFramework;
using Redcode.Extensions;
using SquidGame.Core;
using SquidGame.LandScape;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SquidGame.Minigame14
{
    public class BowlManager : MonoBehaviour
    {
        public event Action ClickedCorrectBowl;
        public event Action ClickedWrongBowl;

        [Header("OBJECTS")]
        [SerializeField] private List<Bowl> _bowls;

        [Header("CONFIGS")]
        [SerializeField] private float _suffleTime = 10f;
        [SerializeField] private float _suffleInterval = 1.2f;
        [SerializeField] private float _difficultyLevel = 1;

        private Bowl _correctBowl;

        private void OnEnable()
        {
            _bowls.ForEach(b => b.Clicked += OnBowlClicked);
        }

        private void OnDisable()
        {
            _bowls.ForEach(b => b.Clicked += OnBowlClicked);
        }

        public void Init()
        {
            SetClickableBowls(false);
        }

        public IEnumerator AddMarblesCoroutine(Transform marbles)
        {
            if (_correctBowl == null)
            {
                _correctBowl = _bowls[Random.Range(0, _bowls.Count)];
                _correctBowl.SetAsCorrectBowl();
            }
            _correctBowl.AddMarbles(marbles);
            yield return new WaitForSeconds(1.3f);
        }

        public IEnumerator ShuffleBowlsCoroutine()
        {
            foreach (var bowl in _bowls)
            {
                bowl.UpsideDown();
            }
            yield return new WaitForSeconds(0.5f);

            var swapTime = _suffleInterval / _suffleTime;
            var waitSwapComplete = new WaitForSeconds(swapTime);
            var swapCount = 0;
            while (swapCount < _suffleTime)
            {
                swapCount++;
                PerformSwapTwoRandomBowls(swapTime);
                yield return waitSwapComplete;
            }
        }

        public void SetClickableBowls(bool isClickable)
        {
            _bowls.ForEach(b => b.SetClickable(isClickable));
        }

        private void PerformSwapTwoRandomBowls(float duration)
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG14_SWAP);

            var randomTwo = _bowls.OrderBy(_ => Random.value).Take(2).ToList();

            Vector3 positionA = randomTwo[0].transform.position;
            Vector3 positionB = randomTwo[1].transform.position;

            Vector3 controlPointA = Vector3.Lerp(positionA, positionB, 0.5f);
            controlPointA.z += 2.5f;
            Vector3 controlPointB = Vector3.Lerp(positionB, positionA, 0.5f);
            controlPointB.z -= 2.5f;

            // Tween for objectA
            randomTwo[0].transform.DOPath(new Vector3[] { controlPointA, positionB }, duration, PathType.CatmullRom)
                .SetEase(Ease.InOutQuad);

            // Tween for objectB
            randomTwo[1].transform.DOPath(new Vector3[] { controlPointB, positionA }, duration, PathType.CatmullRom)
                .SetEase(Ease.InOutQuad);
        }

        private void OnBowlClicked(bool isCorrectBowl)
        {
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Selection);
            SetClickableBowls(false);
            if (isCorrectBowl)
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG14_CORRECT);
                ClickedCorrectBowl?.Invoke();
            }
            else
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG14_WRONG);
                ClickedWrongBowl?.Invoke();
            }
        }
    }
}