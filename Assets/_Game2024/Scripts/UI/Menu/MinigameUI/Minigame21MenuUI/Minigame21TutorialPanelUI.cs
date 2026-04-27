using NFramework;
using Redcode.Extensions;
using SquidGame.Core;
using SquidGame.LandScape;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.Minigame21.UI
{
    public class Minigame21TutorialPanelUI : MonoBehaviour
    {
        [SerializeField] private Image _mainIMG;
        [SerializeField] private TextMeshProUGUI _tutorialTMP;
        [SerializeField] private Button _nextBTN;
        [SerializeField] private Button _backBTN;
        [SerializeField] private Button _doneBTN;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private string[] _texts;
        [SerializeField] private Image[] _tabIcons;

        private int _curTabIndex = 0;

        private void OnEnable()
        {
            _nextBTN.onClick.AddListener(OnNextButtonClicked);
            _backBTN.onClick.AddListener(OnBackButtonClicked);
            _doneBTN.onClick.AddListener(OnDoneButtonClicked);

            _curTabIndex = 0;
            UIManager.I.Close(Define.UIName.GAMEPLAY_POPUP);
            SetData();
        }

        private void OnDisable()
        {
            _nextBTN.onClick.RemoveListener(OnNextButtonClicked);
            _backBTN.onClick.RemoveListener(OnBackButtonClicked);
            _doneBTN.onClick.RemoveListener(OnDoneButtonClicked);
        }

        private void OnNextButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            _curTabIndex++;
            SetData();
        }

        private void OnBackButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            _curTabIndex--;
            SetData();
        }

        private void OnDoneButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            PlayerPrefs.SetInt(Minigame21MenuUI.CHECK_TUTORIAL, 1);
            UIManager.I.Open(Define.UIName.GAMEPLAY_POPUP);
            gameObject.SetActive(false);
        }

        private void SetData()
        {
            UpdateTutorial();
            UpdateButtons();
            UpdateTabs();
        }

        private void UpdateTutorial()
        {
            _mainIMG.sprite = _sprites[_curTabIndex];

            var tutorialText = GameLocalization.I.GetStringFromTable(_texts[_curTabIndex]);
            _tutorialTMP.SetText(tutorialText);
        }

        private void UpdateButtons()
        {
            var isFirstTab = _curTabIndex == 0;
            var isLastTab = _curTabIndex == _sprites.Length - 1;

            _backBTN.gameObject.SetActive(!isFirstTab);
            _nextBTN.gameObject.SetActive(!isLastTab);
            _doneBTN.gameObject.SetActive(isLastTab);
        }

        private void UpdateTabs()
        {
            _tabIcons.ForEach(x => x.color = Color.white);
            _tabIcons[_curTabIndex].color = Color.green;
        }
    }
}
