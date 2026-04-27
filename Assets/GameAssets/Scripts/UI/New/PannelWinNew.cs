using SquidGame;
using SquidGame.Core;
using SquidGame.SaveData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PannelWinNew : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dayTMP;
    [SerializeField] private GameObject[] _greenDayObjects;
    [SerializeField] private Button _touchToContinueBTN;
    [SerializeField] private Button _x5RewardBTN;

    private void OnEnable()
    {
        _touchToContinueBTN.onClick.AddListener(OnTouchToContinueButtonClicked);
        _x5RewardBTN.onClick.AddListener(OnX5RewardClicked);
        SetData();
    }

    private void OnDisable()
    {
        _touchToContinueBTN.onClick.RemoveListener(OnTouchToContinueButtonClicked);
        _x5RewardBTN.onClick.RemoveListener(OnX5RewardClicked);
    }

    private void SetData()
    {
        var day = UserData.I.Day;

        _dayTMP.SetText($"DAY {day}");
        for (int i = 0; i < _greenDayObjects.Length; i++)
        {
            if (i < day)
            {
                _greenDayObjects[i].SetActive(true);
            }
            else
            {
                _greenDayObjects[i].SetActive(false);
            }
        }
    }

    private void OnX5RewardClicked()
    {
        //GameSound.I.PlaySFXButtonClick();
        //UserData.I.Coin += 5000;
        //GameManager.I.GoNextDay();
    }

    private void OnTouchToContinueButtonClicked()
    {
        //GameSound.I.PlaySFXButtonClick();
        //UserData.I.Coin += 1000;
        //GameManager.I.GoNextDay();
    }
}
