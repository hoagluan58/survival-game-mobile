using DG.Tweening;
using Redcode.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
namespace SquidGame.LandScape.Minigame4
{
    public class Scoreboard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private List<Point> _points;

        public Scoreboard SetName(string name)
        {
            _nameText.text = name;
            return this;
        }


        public void Show(float delay,  bool value, float duration)
        {
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(value ? 1 : 0, duration).OnComplete(() => _canvasGroup.blocksRaycasts = value).SetDelay(delay);
        }


        public void OnInitialized()
        {
            _points.ForEach(point => point.OnInitialized());
        }


        public Scoreboard ClearData()
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
