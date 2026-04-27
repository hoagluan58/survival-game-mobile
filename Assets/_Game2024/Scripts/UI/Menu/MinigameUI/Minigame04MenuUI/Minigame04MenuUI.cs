using Game4;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class Minigame04MenuUI : BaseUIView
    {
        [SerializeField] private Button _startButton;

        [Header("PLAYING")]
        [SerializeField] private GameObject _playingPNL;
        [SerializeField] private GameObject _tutorialPNL;
        [SerializeField] private BallInfoPanelUI _playerBallInfoPanel;
        [SerializeField] private BallInfoPanelUI _enemyBallInfoPanel;

        public BallInfoPanelUI PlayerBallInfoPanel => _playerBallInfoPanel;
        public BallInfoPanelUI EnemyBallInfoPanel => _enemyBallInfoPanel;
        public GameObject TutorialPNL => _tutorialPNL;

        private void OnEnable()
        {
            _startButton.onClick.AddListener(OnStartButtonClick);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClick);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _playingPNL.SetActive(false);
            _startButton.gameObject.SetActive(true);
            _tutorialPNL.SetActive(false);
        }

        private void OnStartButtonClick()
        {
            _playingPNL.SetActive(true);
            _startButton.gameObject.SetActive(false);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_BUTTON_CLICK);
            GameManager.I.StartMinigame();
        }
    }
}