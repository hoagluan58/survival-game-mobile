using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    public class DoorEditor : MonoBehaviour
    {
        [SerializeField] private List<Transform> _doors;
        [SerializeField] private List<Vector3> _pos;


        [Button]
        private void AssignPositions()
        {
            _pos = new List<Vector3>();
            _doors.ForEach(x => { 
                _pos.Add(x.position);
            });
        }
    }
}
