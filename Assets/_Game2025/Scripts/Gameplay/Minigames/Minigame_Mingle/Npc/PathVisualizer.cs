using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    public class PathVisualizer : MonoBehaviour
    {
        [SerializeField] private Color color;
        [SerializeField] private bool _show; 
        public List<Vector3> pathPoints = new List<Vector3>();


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_show) return;

            if (pathPoints == null || pathPoints.Count < 2) return;

            Handles.color = color ;
            Handles.DrawPolyLine(pathPoints.ToArray());
        }
#endif
    }
}
