using Redcode.Extensions;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
namespace Game1
{
    [System.Serializable]
    public class RaycastData
    {
        public bool IsHit;
        public float Distance;
        public Vector3 HitPoint;
    }

    public class BotBrain : MonoBehaviour
    {
        [Header("CONFIGS")]
        [SerializeField] private float _rayDistance;
        [SerializeField] private float _rotationAngle = 30f;
        [SerializeField] private float _maxZ; 
        [SerializeField] private LayerMask _layerMask;

        private Bot _bot;
        private Vector3 _direction;
        private Vector3 _rotationAxis = Vector3.up;
        private Vector3 _targetPosition;
        private List<RaycastData> _datas;
        private List<Vector3> _positions;

        public List<Vector3> Positions => _positions;

        public void Init(Bot bot)
        {
            _bot = bot;
            _datas = new List<RaycastData>();
            for (int i = 0; i < 9; i++)
            {
                _datas.Add(new RaycastData());
            }
            InitPath();
        }


        private void InitPath()
        {
            _direction = Vector3.forward;
            _targetPosition = transform.position;
            _positions = new List<Vector3>();
            _positions.Add(transform.position);
            GenPath();
        }
        

        private void GenPath()
        {
            if (_targetPosition.z > _maxZ) return;
            Vector3[] directions = new Vector3[9];
            directions[4] = _direction;

  
            for (int i = 1; i <= 4; i++)
            {
                directions[4 - i] = Quaternion.AngleAxis(-_rotationAngle * i, _rotationAxis) * _direction; 
                directions[4 + i] = Quaternion.AngleAxis(_rotationAngle * i, _rotationAxis) * _direction; 
            }

            for (int i = 0; i < directions.Length; i++)
            {
                if (Physics.Raycast(_targetPosition, directions[i], out RaycastHit hit, _rayDistance, _layerMask))
                {
                    _datas[i].IsHit = true;
                    _datas[i].Distance = 0;
                    _datas[i].HitPoint = _targetPosition + directions[i]*_rayDistance;
                }
                else
                {
                    _datas[i].IsHit = false;
                    _datas[i].Distance = _rayDistance;
                    _datas[i].HitPoint = _targetPosition + directions[i] * _rayDistance;
                }
            }
            var data = _datas.FindAll(x => x.IsHit == false).OrderByDescending(x => x.HitPoint.z).ToList();
            _positions.Add(data[0].HitPoint);
            _targetPosition = data[0].HitPoint;
            GenPath();
        }


        public Vector3 GetDirection()
        {
            return Vector3.zero;
        }
    }
}
