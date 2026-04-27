using UnityEngine;

namespace Game11
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform _posRoll;

        public Transform PosRoll => _posRoll;
    }
}
