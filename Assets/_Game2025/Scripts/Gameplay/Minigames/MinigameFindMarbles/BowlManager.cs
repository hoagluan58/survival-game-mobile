using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.LandScape.MinigameFindMarbles
{
    public class BowlManager : MonoBehaviour
    {
        public event Action ClickedCorrectBowl;
        public event Action ClickedWrongBowl;

        private List<Bowl> _bowls = new List<Bowl>();
        private Bowl _correctBowl;

        private bool _isDestroy;

        public void SetBowls(List<Bowl> bowls)
        {
            _bowls = bowls;
            SetClickableBowls(false);
            _bowls.ForEach(b => b.Clicked -= OnBowlClicked);
            _bowls.ForEach(b =>
            {
                b.Clicked += OnBowlClicked;
                b.SetCorrectBowl(false);
            });
        }

        public void AddMarblesToCorrectBowl(Transform marbles)
        {
            _correctBowl = null;
            _correctBowl = _bowls[UnityEngine.Random.Range(0, _bowls.Count)];
            _correctBowl.SetCorrectBowl(true);
            _correctBowl.AddMarbles(marbles);
        }

        public IEnumerator ShuffleBowlsCoroutine(float shuffleTime, float shuffleInverval)
        {
            foreach (var bowl in _bowls)
            {
                bowl.UpsideDown();
            }
            yield return new WaitForSeconds(0.5f);

            var swapTime = shuffleInverval / shuffleTime;
            var waitSwapComplete = new WaitForSeconds(swapTime);
            var swapCount = 0;
            while (swapCount < shuffleTime && !_isDestroy)
            {
                swapCount++;
                PerformSwapTwoRandomBowls(swapTime);
                yield return waitSwapComplete;
            }
        }

        public void SetClickableBowls(bool isClickable) => _bowls.ForEach(b => b.SetClickable(isClickable));

        private void PerformSwapTwoRandomBowls(float duration)
        {

            GameSound.I.PlaySFX(Define.SoundPath.SFX_SWAP);
            var randomTwo = _bowls.OrderBy(_ => UnityEngine.Random.value).Take(2).ToList();

            Vector3 positionA = randomTwo[0].transform.position;
            Vector3 positionB = randomTwo[1].transform.position;

            Vector3 controlPointA = Vector3.Lerp(positionA, positionB, 0.5f);
            controlPointA.z += 0.5f;
            Vector3 controlPointB = Vector3.Lerp(positionB, positionA, 0.5f);
            controlPointB.z -= 0.5f;

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
                GameSound.I.PlaySFX(Define.SoundPath.SFX_CORRECT_CHOICE);
                ClickedCorrectBowl?.Invoke();
            }
            else
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_WRONG_CHOICE);
                ClickedWrongBowl?.Invoke();
            }
        }

        private void OnDestroy()
        {
            _isDestroy= true;
        }
    }
}
