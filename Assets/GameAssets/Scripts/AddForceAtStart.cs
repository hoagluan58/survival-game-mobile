using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceAtStart : MonoBehaviour
{
    [SerializeField] private float _valueForce;
    private Rigidbody _rigidbody;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Vector3 random = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f));
        random.Normalize();

        _rigidbody.AddForce(random * _valueForce);
    }
}
