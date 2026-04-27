using SquidGame.LandScape.Core;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SquidGame.LandScape.Minigame2
{
    public class GlassPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private GameObject _fractureParent;
        [SerializeField] private GlassFracture _fractureEffect;
        [SerializeField] private Collider _collder;

        private bool _isBreak;
        private GlassPanelData _data;

        public GlassPanelData Data => _data;
        public bool IsBreak
        {
            get => _isBreak;
            set
            {
                _isBreak = value;
                _panel.SetActive(!_isBreak);
                _fractureParent.SetActive(_isBreak);
                if (_isBreak)
                {
                    Instantiate(_fractureEffect, _fractureParent.transform);
                }
            }
        }

        public void Init(GlassPanelData data)
        {
            _data = data;
            IsBreak = false;
            SetTrueMove(_data.IsTrueMove);
        }

        public void SetTrueMove(bool value)
        {
            _data.IsTrueMove = value;
        }

        public void Break()
        {
            if (IsBreak) return;
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_GLASS_BREAK);
            IsBreak = true;
        }

        public Vector3 RandomPositionOnGlass()
        {
            var center = _collder.bounds.center;
            var extents = _collder.bounds.extents;

            var randomX = Random.Range(center.x - extents.x, center.x + extents.x);
            var randomY = Random.Range(center.y - extents.y, center.y + extents.y);
            var randomZ = Random.Range(center.z - extents.z, center.z + extents.z);

            return new Vector3(randomX, randomY, randomZ);
        }
    }

    [Serializable]
    public class GlassPanelData
    {
        public int Row, Col;
        public bool IsTrueMove;

        public GlassPanelData(int row, int col, bool isTrueMove)
        {
            Row = row;
            Col = col;
            IsTrueMove = isTrueMove;
        }
    }
}
