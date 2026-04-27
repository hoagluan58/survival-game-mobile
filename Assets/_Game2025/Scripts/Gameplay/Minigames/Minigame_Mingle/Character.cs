using DG.Tweening;
using EPOOutline;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    public interface IOutliner
    {
        public void SetEnableOutline(bool enable);
    }

    public class Character : MonoBehaviour, IOutliner
    {
        [SerializeField] protected Outlinable Outlinable;

        private Tweener _fadeTween;
        public void SetEnableOutline(bool value)
        {
            Outlinable.enabled = value; 
            CheckFadeOutline(value);
        }

        private void CheckFadeOutline(bool value)
        {
            if (!value) {
                _fadeTween.Kill();
                Outlinable.OutlineParameters.DilateShift = 1;
                return;
            }

            _fadeTween = DOVirtual.Float(0, 1, 2, (value) =>
            {
                Outlinable.OutlineParameters.DilateShift = value;
            }).SetLoops(-1,LoopType.Yoyo);

        }
    }
}
