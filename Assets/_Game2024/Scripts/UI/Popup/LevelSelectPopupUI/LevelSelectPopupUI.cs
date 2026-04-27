using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class LevelSelectPopupUI : BaseUIView
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private MinigameSelectButton _minigameSelectButtonPrefab;

        private void Start()
        {
            MinigameSelectButton.Clicked += OnSelectLevelButtonClicked;
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            foreach (var minigameConfig in ConfigManager.I.MinigameConfig)
            {
                var newButton = Instantiate(_minigameSelectButtonPrefab, _scrollRect.content);
                newButton.OnSpawn(minigameConfig.Key);
            }
        }

        private void OnSelectLevelButtonClicked(int levelId)
        {
            CloseSelf();
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.CloseAllInLayer(EUILayer.Menu);
            UIManager.I.CloseAllInLayer(EUILayer.Popup);
            GameManager.I.LoadMinigame(levelId);
        }

        private void OnCloseButtonClicked()
        {
            CloseSelf();
        }
    }
}