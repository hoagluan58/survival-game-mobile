using SquidGame.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.Minigame15
{
    public class ScorePanel : MonoBehaviour
    {
        [SerializeField] private List<Image> _playerScoredImages;
        [SerializeField] private TextMeshProUGUI _playerNameTMP;

        [SerializeField] private List<Image> _opponentScoredImages;
        [SerializeField] private TextMeshProUGUI _opponentNameTMP;

        public void Init()
        {
            _playerNameTMP.SetText(GameLocalization.I.GetStringFromTable("STRING_YOU"));
            _opponentNameTMP.SetText($"No. {Random.Range(0, 455)}");
            UpdateScore(0, 0);
        }

        public void UpdateScore(int playerScore, int opponentScore)
        {
            _playerScoredImages.ForEach(x => x.gameObject.SetActive(false));
            _opponentScoredImages.ForEach(x => x.gameObject.SetActive(false));
            for (var i = 0; i < playerScore; i++)
            {
                _playerScoredImages[i].gameObject.SetActive(true);
            }
            for (var i = 0; i < opponentScore; i++)
            {
                _opponentScoredImages[i].gameObject.SetActive(true);
            }
        }
    }
}
