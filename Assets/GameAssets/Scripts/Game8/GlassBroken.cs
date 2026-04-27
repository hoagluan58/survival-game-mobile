using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.Minigame08
{
    public class GlassBroken : MonoBehaviour
    {
        [SerializeField] private List<Transform> _pieces;

        private List<Vector3> _positions = new List<Vector3>();

        public void Init()
        {
            _pieces.ForEach(x => _positions.Add(x.localPosition));
        }

        public void Restore()
        {
            gameObject.SetActive(false);
            for (var i = 0; i < _pieces.Count; i++)
            {
                var piece = _pieces[i];
                piece.localPosition = _positions[i];
            }
        }
    }
}
