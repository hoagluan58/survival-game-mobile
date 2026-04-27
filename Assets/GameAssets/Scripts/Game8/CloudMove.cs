using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game9
{
    public class CloudMove : MonoBehaviour
    {
        [SerializeField] private Vector2 _speedRange;
        [SerializeField] private Vector3 _dir;
        [SerializeField] private Vector2 _clampRange;

        private float _speed;

        private void Start()
        {
            _speed = Random.Range(_speedRange.x, _speedRange.y);
        }

        private void Update()
        {
            transform.Translate(_dir * _speed * Time.deltaTime, Space.World);

            Vector3 currentPos = transform.position;
            if (currentPos.x > _clampRange.y)
            {
                currentPos.x = _clampRange.x;
                transform.position = currentPos;
            }
        }
    }
}
