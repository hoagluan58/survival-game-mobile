using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Redcode.Extensions;


#if UNITY_EDITOR
using UnityEditor;
#endif
namespace SquidGame.LandScape.MinigameMingle
{
    public enum AreaType
    {
        Ring = 0,
        Podium = 1
    }

    public class RingAreaSpawner : MonoBehaviour
    {

        [SerializeField] private float _innerRadius = 1f; 
        [SerializeField] private float _outerRadius = 20f;
        [SerializeField] private float _podiumRadius = 10f;

        public float spacing = 2f;
        private List<Vector3> spawnPositions = new List<Vector3>();
        


        

     
        [Button]
        public void GenerateSpawnPositionsInPodium()
        {
            spawnPositions.Clear();
           
            Vector3 center = transform.position;
            float offset = 4f * spacing; 

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                   
                    Vector3 spawnPos = new Vector3(center.x + (i * spacing - offset), center.y, center.z + (j * spacing - offset));

                  
                    if ((i == 4 || i == 5) && (j == 4 || j == 5))
                        continue;

                   
                    if ((i == 0 && j == 0) || (i == 0 && j == 8) || (i == 8 && j == 0) || (i == 8 && j == 8))
                        continue;

                    spawnPositions.Add(spawnPos);
                  
                }
            }
        }


        public Vector3 GetRandomPositionPodium()
        {
            var vector = spawnPositions.GetRandomElement();
            spawnPositions.Remove(vector);
            return vector;
        }


        public Vector3 GetRandomPointArea()
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Mathf.Sqrt(Random.Range(_innerRadius * _innerRadius, _outerRadius * _outerRadius));

            float x = transform.position.x + radius * Mathf.Cos(angle);
            float z = transform.position.z + radius * Mathf.Sin(angle);

            return new Vector3(x, transform.position.y, z);
        }


        public Vector3 GetRandomPointPodium()
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Mathf.Sqrt(Random.Range(_innerRadius * _innerRadius, _podiumRadius * _podiumRadius));
            float x = transform.position.x + radius * Mathf.Cos(angle);
            float z = transform.position.z + radius * Mathf.Sin(angle);
            return new Vector3(x, transform.position.y, z);
        }


        public AreaType GetAreaType(Vector3 position)
        {
            float distance = Vector3.Distance(position, transform.position);
            if (distance < _podiumRadius)
            {
                return AreaType.Podium;
            }
            else if (distance < _outerRadius)
            {
                return AreaType.Ring;
            }
            return AreaType.Ring;
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.up, _innerRadius);

            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, Vector3.up, _outerRadius);

            Handles.color = Color.blue;
            Handles.DrawWireDisc(transform.position, Vector3.up, _podiumRadius);

            if (spawnPositions == null || spawnPositions.Count == 0) return;

            for (int i = 0; i < spawnPositions.Count; i++)
            {
                Gizmos.color =  Color.green;
                Gizmos.DrawSphere(spawnPositions[i], 0.1f);
            }
        }
#endif
    }
}
