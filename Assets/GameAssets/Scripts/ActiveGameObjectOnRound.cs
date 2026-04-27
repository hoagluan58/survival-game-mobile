using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGameObjectOnRound : MonoBehaviour
{
    [SerializeField] private int _roundCompare;
    [SerializeField] private bool _isEqual;

    private void Start()
    {
        if (_isEqual)
        {
            if (_roundCompare != Static.CurrentRound)
                gameObject.SetActive(false);
        }
        else
        {
            if (_roundCompare == Static.CurrentRound)
                gameObject.SetActive(false);
        }
    }
}
