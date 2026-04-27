using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    public class RoomArea : MonoBehaviour
    {
        [SerializeField] private float _radius = 2f;


        public Vector3 GetRandomPoint()
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Mathf.Sqrt(Random.Range(0, _radius * _radius));

            float x = transform.position.x + radius * Mathf.Cos(angle);
            float z = transform.position.z + radius * Mathf.Sin(angle);

            return new Vector3(x, transform.position.y, z);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = Color.cyan;
            Handles.DrawWireDisc(transform.position, Vector3.up, _radius);
        }

#endif
    }
}
