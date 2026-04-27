using DG.Tweening;
using SquidGame.Core;
using SquidGame.LandScape;
using System.Collections;
using UnityEngine;

namespace SquidGame.Minigame21
{
    public class IntroStartGame : MonoBehaviour
    {
        [SerializeField] private Transform _carouselTf;

        private Tween _tween;

        public IEnumerator CRIntroGame()
        {
            GameSound.I.PlayBGM(Define.SoundPath.BGM_MG21_MINGLE);
            var rndTime = Random.Range(7, 10);
            var waiter = new WaitForSeconds(rndTime);
            var rotation = new Vector3(0, 360f, 0);
            _carouselTf.localEulerAngles = Vector3.zero;
            _tween = _carouselTf.DOLocalRotate(rotation, rndTime, RotateMode.FastBeyond360).SetEase(Ease.Linear);
            yield return waiter;
            _tween.Kill();
            GameSound.I.StopBGM();
        }
    }
}
