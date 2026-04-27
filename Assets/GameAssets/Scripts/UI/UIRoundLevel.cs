using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIRoundLevel : MonoBehaviour
{
    private TextMeshProUGUI _tmp;

    private void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _tmp.text = "Round " + (Static.CurrentRound + 1) + "\t" + "Level " + (Static.CurrentLevel + 1);
    }
}
