using Redcode.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game8
{
    public class Line : MonoBehaviour
    {
        private Tile[] _tiles;
        private SerializableDictionary<TypeShape, Tile> _shapeMapper = new SerializableDictionary<TypeShape, Tile>();
        private TypeShape _typeTrueShape;

        private void Awake()
        {
            _tiles = GetComponentsInChildren<Tile>();
        }

        public void Init(int id)
        {
            List<Tile> tiles = new List<Tile>(_tiles);
            for (int i = 0; i < 3; i++)
            {
                int rd = Random.Range(0, tiles.Count);
                _shapeMapper.Add(new SerializableDictionary<TypeShape, Tile>.Pair((TypeShape)i, tiles[rd]));
                tiles.RemoveAt(rd);
            }

            _typeTrueShape = (TypeShape)Random.Range(0, 3);
            foreach (var item in _shapeMapper)
            {
                if (item.Key == _typeTrueShape)
                    item.Value.Init(item.Key, true);
                else item.Value.Init(item.Key, false);
            }

            for (int i = 0; i < _tiles.Length; i++)
            {
                _tiles[i].StartAnim(id + i);
            }
        }

        public void Active()
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
                _tiles[i].Active();
            }
        }

        public Tile GetPos(TypeShape typeShape)
        {
            return _shapeMapper[typeShape];
        }

        public void Show()
        {
            StartCoroutine(IE_Show());
        }

        private IEnumerator IE_Show()
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
                _tiles[i].Show();
                yield return new WaitForSeconds(0.05f);
            }
        }

        public void BreakAll()
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
                _tiles[i].Break();
            }
        }
    }
}
