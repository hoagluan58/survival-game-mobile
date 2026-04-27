using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SquidGame.Minigame13.UI
{
    public class ScorePanelUI : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _scoreObjects;
        [SerializeField] private TextMeshProUGUI _nameTMP;

        public void SetData(string name)
        {
            UpdateScore(0);
            _nameTMP.text = name;
        }

        public void UpdateScore(int score)
        {
            _scoreObjects.ForEach(x => x.SetActive(false));

            for (int i = 0; i < score; i++)
            {
                if (i < _scoreObjects.Count)
                {
                    _scoreObjects[i].SetActive(true);
                }
            }
        }
    }
}
