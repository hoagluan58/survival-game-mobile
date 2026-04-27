using SquidGame.LandScape.Core;
using SquidGame.Minigame15;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.MinigameMarblesVer2
{
    public class ThrowHandler : MonoBehaviour
    {
        [SerializeField] private Transform _throwPoint;
        [SerializeField] private Marble _redMarblePrefab;


        private List<Marble> _marbles;

        private void Awake()
        {
            _marbles = new List<Marble>();
        }

        public void Throw(Vector3 direction, float force, UnityAction onCompleted)
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG04_THROW_MARBLE);
            var marble = Instantiate(_redMarblePrefab, transform);
            marble.Throw(_throwPoint.position, direction, force, onCompleted);
            _marbles.Add(marble);
        }


        public int GetScore()
        {
            var marblePosZ = _marbles[^1].transform.position.z;
            var score = marblePosZ > 55 ? 0 :  Mathf.FloorToInt((marblePosZ - 10) / 0.45f);
            return score;
        }

        internal void ClearMarbles()
        {
            foreach (var marble in _marbles)
            {
                Destroy(marble.gameObject);
            }

            _marbles.Clear();
        }
    }
}
