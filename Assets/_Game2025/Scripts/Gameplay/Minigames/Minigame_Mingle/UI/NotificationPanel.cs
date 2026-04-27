using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    public class NotificationPanel : MonoBehaviour
    {

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _text;

        public NotificationPanel SetText(string text)
        {
            _text.text = text;
            return this;
        }


        public void Show( float duration)
        {
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(1, duration);
        }

        public void Hide(float duration)
        {
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(0,duration);
        }
    }
}
