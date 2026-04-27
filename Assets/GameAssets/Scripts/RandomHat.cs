using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHat : MonoBehaviour
{
    [SerializeField] private GameObject[] _allHats;

    public void StartRandom()
    {
        for (int i = 0; i < _allHats.Length; i++)
        {
            _allHats[i].gameObject.SetActive(false);
        }

        _allHats[Random.Range(0, _allHats.Length)].gameObject.SetActive(true);
    }
}
