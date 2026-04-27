using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMarblesVer2
{
    public class NoticePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private CanvasGroup _canvasGroup; 
        public void SetText(string content)
        {
            _text.text = content;
        }

        public void SetActive(bool value, float duration)
        {
            _canvasGroup.DOFade(value ? 1: 0 , duration);
        }
    }
}
