using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform _target=default;
    [SerializeField] private float _valueLerp=default;

    public void SetTargert(Transform target)
    {
        target = _target;
    }

    private void LateUpdate()
    {
        if (_target != null)
        {
            Vector3 posTarget = _target.transform.position;
            posTarget.z = 0;
            transform.position = Vector3.Lerp(transform.position,posTarget, _valueLerp);
        }
    }
}
