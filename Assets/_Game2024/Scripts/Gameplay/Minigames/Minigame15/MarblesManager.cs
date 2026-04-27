using System.Collections.Generic;
using System.Linq;
using SquidGame.Core;
using SquidGame.LandScape;
using UnityEngine;

namespace SquidGame.Minigame15
{
    public class MarblesManager : MonoBehaviour, IMarbleThrowHandler
    {
        [SerializeField] private Marble _greenMarblePrefab;
        [SerializeField] private Marble _redMarblePrefab;

        private List<Marble> _marbles = new List<Marble>();

        public Marble ThrowMarble(Side side, Vector3 spawnPosition, Vector3 direction, float force)
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG04_THROW_MARBLE);
            var marble = Instantiate(side == Side.Player ? _greenMarblePrefab : _redMarblePrefab, transform);
            marble.Throw(side, spawnPosition, direction, force);
            _marbles.Add(marble);
            return marble;
        }

        public void ClearMarbles()
        {
            _marbles.ForEach(m => m.gameObject.SetActive(false));
            _marbles.Clear();
        }

        public Side GetWinSide()
        {
            var furthestMarble = _marbles.OrderByDescending(m => m.transform.position.z).First();
            return furthestMarble.Side;
        }
    }

    public interface IMarbleThrowHandler
    {
        Marble ThrowMarble(Side side, Vector3 spawnPosition, Vector3 direction, float force);
    }
}