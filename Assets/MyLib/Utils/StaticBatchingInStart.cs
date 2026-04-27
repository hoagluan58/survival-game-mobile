using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBatchingInStart : MonoBehaviour
{
    [SerializeField] private float _delayStart=default;
    private void Start()
    {
        Run.After(_delayStart, () =>
        {
            StaticBatchingUtility.Combine(gameObject);
        });
    }
}
