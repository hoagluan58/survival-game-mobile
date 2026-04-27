using SquidGame;
using SquidGame.Gameplay;
using UnityEngine;

public class ResultScreenControl : MonoBehaviour
{
    [SerializeField] private GameObject _panelWin;
    [SerializeField] private GameObject _panelLose;
    [SerializeField] private GameObject _objectWin;
    [SerializeField] private GameObject _objectLose;

    private void Start()
    {
        if (GameManager.I.CurGameState == EGameState.Win)
        {
            _objectWin.SetActive(true);
            _panelWin.SetActive(true);
        }
        else if (GameManager.I.CurGameState == EGameState.Lose)
        {
            _objectLose.SetActive(true);
            _panelLose.SetActive(true);
        }
    }
}
