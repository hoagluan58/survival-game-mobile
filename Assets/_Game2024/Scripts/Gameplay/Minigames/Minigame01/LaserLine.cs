using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace SquidGame.Minigame01
{
    public class LaserLine : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        private ObjectPool<LaserLine> _pool;

        public void DrawLine(Transform[] points)
        {
            _lineRenderer.positionCount = points.Length;
            for (var i = 0; i < points.Length; i++)
            {
                var point = points[i];
                _lineRenderer.SetPosition(i, point.position);
            }
        }

        public void SetPool(ObjectPool<LaserLine> pool) => _pool = pool;


        public void Clearline()
        {
            _lineRenderer.positionCount = 0;
        }
    }
}
