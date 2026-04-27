using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class MinigameSelectButton : MonoBehaviour
    {
        public static Action<int> Clicked;
        
        private int _levelId;
        private Button _button;

        [SerializeField] private TextMeshProUGUI _levelNameTMP;

        public void OnSpawn(int levelId)
        {
            gameObject.SetActive(true);
            _levelId = levelId;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnSelectButtonClicked);
            _levelNameTMP.text = $"Level {_levelId}";
        }

        private void OnSelectButtonClicked()
        {
            Clicked?.Invoke(_levelId);
        }
    }
}