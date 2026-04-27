using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] private Vector3 _dirRotate = default;
    [SerializeField] private float _speedRotate = default;
    [SerializeField] private Space _space = default;
    [SerializeField] private Vector2 _randomSpeedAdd = default;


    private float _speed;

    private void Start()
    {
        Random.InitState(gameObject.GetInstanceID() + System.DateTime.Now.Millisecond);
        _speed = _speedRotate + Random.Range(_randomSpeedAdd.x, _randomSpeedAdd.y);
    }

    private void Update()
    {
        transform.Rotate(_dirRotate * _speed * Time.deltaTime, _space);
    }
}
