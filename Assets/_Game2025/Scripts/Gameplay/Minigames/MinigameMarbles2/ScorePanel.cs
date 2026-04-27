using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMarblesVer2
{

    public class ScorePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private List<Point> _points;

        public ScorePanel SetName(string name)
        {
            _nameText.text = name;
            return this;
        }


        public void Show(float delay, bool value, float duration)
        {
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(value ? 1 : 0, duration).OnComplete(() => _canvasGroup.blocksRaycasts = value).SetDelay(delay);
        }


        public void OnInitialized()
        {
            _points.ForEach(point => point.OnInitialized());
        }


        public ScorePanel ClearData()
        {
            _points.ForEach(point => point.ClearData());
            return this;
        }


        public void Scored(int index, bool value)
        {
            _points[index].Scored(value);
        }


        public void Scored(bool value)
        {
            var index = _points.FindIndex(point => !point.IsCompleted);
            if (index == -1) return;
            _points[index].Scored(value);
        }
    }
}
