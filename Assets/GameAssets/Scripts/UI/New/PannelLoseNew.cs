using SquidGame;
using SquidGame.Core;
using SquidGame.SaveData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PannelLoseNew : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dayTMP;
    [SerializeField] private GameObject[] _greenDayObjects;
    [SerializeField] private GameObject[] _redDayObjects;
    [SerializeField] private Button _retryBTN;
    [SerializeField] private Button _nextBTN;

    private void OnEnable()
    {
        _retryBTN.onClick.AddListener(OnRetryButtonClicked);
        _nextBTN.onClick.AddListener(OnNextButtonClicked);
        SetData();
    }

    private void OnDisable()
    {
        _retryBTN.onClick.RemoveListener(OnRetryButtonClicked);
        _nextBTN.onClick.RemoveListener(OnNextButtonClicked);
    }

    private void SetData()
    {
        var day = UserData.I.Day;

        _dayTMP.SetText($"DAY {day}");

        for (int i = 0; i < _greenDayObjects.Length; i++)
        {
            if (i < day)
                _greenDayObjects[i].SetActive(true);
            else
                _greenDayObjects[i].SetActive(false);
        }

        for (var i = 0; i < _redDayObjects.Length; i++)
        {
            if (i == day - 1)
            {
                _redDayObjects[i].SetActive(true);
            }
            else
            {
                _redDayObjects[i].SetActive(false);
            }
        }
    }

    private void OnRetryButtonClicked()
    {
        //GameSound.I.PlaySFXButtonClick();
        //GameManager.I.Init();
    }

    private void OnNextButtonClicked()
    {
        //GameSound.I.PlaySFXButtonClick();
        //GameManager.I.GoNextDay();
    }
}
