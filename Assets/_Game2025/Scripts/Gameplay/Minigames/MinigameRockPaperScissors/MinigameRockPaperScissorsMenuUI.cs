using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.MinigameMarblesVer2;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.MinigameRockPaperScissors
{
    public enum GameResult
    {
        Rock,
        Paper,
        Scissor,
    }

 

    public class MinigameRockPaperScissorsMenuUI : BaseUIView
    {
        public event Action<GameResult> OnPlayerSelectedAction;

        [SerializeField] private Button _settingButton;
        [SerializeField] private Button _rockButton;
        [SerializeField] private Button _paperButton;
        [SerializeField] private Button _scissorButton;
        [SerializeField] private GameObject _answerPanelGo;
        [SerializeField] private GameObject _optionPanelGo;
        [SerializeField] private MagazineUI _magazine;
        [SerializeField] private TextMeshProUGUI _noticeText;
        [SerializeField] private Image _opponentImage;
        [SerializeField] private Image _playerImage;
        [SerializeField] private NoticePanel _noticePanel;
        [SerializeField] private GameAsset _assets;
        [SerializeField] private TextMeshProUGUI _levelText;

        private bool _isPlayerSelected;

        public void Initialized(int maxBullet, int level)
        {
            _magazine.Initialized(maxBullet);
            _noticePanel.SetActive(false, 0.1f);
            _answerPanelGo.SetActive(false);
            _optionPanelGo.SetActive(false);
            SetActiveMagazineUI(false);
            _levelText.SetText($"Level {level+1}");
        }


        public override void OnOpen()
        {
            base.OnOpen();
        }

        private void OnEnable()
        {
            _rockButton.onClick.AddListener(OnRockButtonClicked);
            _paperButton.onClick.AddListener(OnPaperButtonClicked);
            _scissorButton.onClick.AddListener(OnScissorButtonClicked);
            _settingButton.onClick.AddListener(OnSettingButtonClicked);
        }

        private void OnDisable()
        {
            _rockButton.onClick.RemoveListener(OnRockButtonClicked);
            _paperButton.onClick.RemoveListener(OnPaperButtonClicked);
            _scissorButton.onClick.RemoveListener(OnScissorButtonClicked);
            _settingButton.onClick.RemoveListener(OnSettingButtonClicked);
        }


        private void OnSettingButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UIManager.I.Open(Define.UIName.SETTINGS_POPUP);
        }


        public override void OnClose()
        {
            base.OnClose();
        }


        private void OnScissorButtonClicked()
        {
            VibrationManager.I.Haptic(VibrationManager.EHapticType.SoftImpact);
            _isPlayerSelected = true;
            SetActiveOption(false);
            GameSound.I.PlaySFXButtonClick();
            OnPlayerSelectedAction?.Invoke(GameResult.Scissor);
        }


        private void OnPaperButtonClicked()
        {
            VibrationManager.I.Haptic(VibrationManager.EHapticType.SoftImpact);
            _isPlayerSelected = true;
            SetActiveOption(false);
            GameSound.I.PlaySFXButtonClick();
            OnPlayerSelectedAction?.Invoke(GameResult.Paper);
        }


        private void OnRockButtonClicked()
        {
            VibrationManager.I.Haptic(VibrationManager.EHapticType.SoftImpact);
            _isPlayerSelected = true;
            SetActiveOption(false);
            GameSound.I.PlaySFXButtonClick();
            OnPlayerSelectedAction?.Invoke(GameResult.Rock);
            
        }


        internal IEnumerator CRCountdownStartGame(int time)
        {
            _noticePanel.SetActive(true, 0.5f);
            while (time > 0)
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_TICK);
                _noticePanel.SetText($"Game start in {time}s");
                yield return new WaitForSeconds(1f);
                time--;
            }
            _noticePanel.SetActive(false, 0.1f);
        }


        internal void SetActiveOption(bool value)
        {
            _optionPanelGo.SetActive(value);
        }


        public void SetActiveMagazineUI(bool value)
        {
            _magazine.gameObject.SetActive(value);
        }


        internal IEnumerator CRCountdownSelectOption()
        {
            _isPlayerSelected = false;
            var time = 5;
            _noticePanel.SetActive(true, 0.5f);
            while (time > 0 && !_isPlayerSelected)
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_TICK);
                _noticePanel.SetText($"Choose your move ! {time}s seconds left ");
                yield return new WaitForSeconds(1f);
                time--;
            }
            _noticePanel.SetActive(false, 0.1f);
        }


        internal void ShowResult(GameResult playerResult, GameResult opponentResult)
        {
            _opponentImage.sprite = _assets.Get(opponentResult);
            _playerImage.sprite= _assets.Get(playerResult);
            _answerPanelGo.SetActive(true);
            this.InvokeDelay(2f, () => _answerPanelGo.SetActive(false));
        }


        internal void SetNoti(string content)
        {
            _noticePanel.SetActive(true, 0.1f);
            _noticePanel.SetText(content);
        }

        internal void UpdateMagazine(int index , BulletState state)
        {
            _magazine.UpdateStatusBullet(index, state);
        }

        internal void RotateMagazine(int currentRound)
        {
            _magazine.MoveTo(currentRound, 1f);
        }
    }
}
