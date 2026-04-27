using SquidGame.LandScape.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame2
{
    public class NPCController : MonoBehaviour
    {
        [SerializeField] private Transform _npcHolder;
        [SerializeField] private Transform _minJumpPoint;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _spawnRadius = 3f;

        [Header("REF")]
        [SerializeField] private GlassBridgeNPC _npcPf;
        [SerializeField] private Transform _destinationPos;

        public Transform DestinationPos => _destinationPos;
        public Transform MinJumpPos => _minJumpPoint;

        private bool _isPaused;
        private GlassPanel[,] _glassPanels;
        private List<GlassPanel> _correctPath = new List<GlassPanel>();
        private List<GlassBridgeNPC> _npcList = new();
        private Queue<GlassBridgeNPC> _queue = new();

        public void Init(GlassPanel[,] glassPanels)
        {
            _glassPanels = glassPanels;
        }

        public IEnumerator CRSpawnInterval(int spawnAmount, float maxTime)
        {
            for (var i = 0; i < spawnAmount; i++)
            {
                var waiter = new WaitForSeconds(Random.Range(0f, maxTime));
                var spawnPos = RandomSpawnPosition();
                var npc = Instantiate(_npcPf, spawnPos, Quaternion.identity, _npcHolder);

                npc.Init(_glassPanels, _correctPath);
                npc.SetState(new List<INPCState>
                {
                    new IdleState(npc),
                    new WanderState(npc),
                    new JumpBridgeState(this, npc),
                });
                npc.TransitionTo(INPCState.EState.Wander);

                _npcList.Add(npc);
                _queue.Enqueue(npc);
                yield return waiter;
            }
        }

        public void StartJumpingBehaviour()
        {
            StartCoroutine(CRJumpingBehaviour());

            IEnumerator CRJumpingBehaviour()
            {
                var waiter = new WaitForSeconds(1f);

                while (_queue.Count > 0)
                {
                    yield return waiter;
                    if (_isPaused) continue;

                    var npc = _queue.Dequeue();
                    npc.TransitionTo(INPCState.EState.MoveToDestination);
                }
            }
        }

        public void TryAddCorrectPath(GlassPanel panel)
        {
            if (_correctPath.Contains(panel)) return;
            _correctPath.Add(panel);
        }

        public void KillNPCNotReachDestination()
        {
            foreach (var npc in _npcList)
            {
                if (npc.IsDeath || npc.IsReachDestination) continue;
                npc.TransitionTo(INPCState.EState.Idle);
                npc.Die();
            }
        }

        public void ToggleUpdate(bool value)
        {
            _isPaused = !value;
            foreach (var npc in _npcList)
            {
                if (npc.IsDeath || npc.IsReachDestination) continue;
                npc.ToggleUpdate(value);
            }
        }

        private Vector3 RandomSpawnPosition()
        {
            var rndX = Random.Range(-_spawnRadius, _spawnRadius);
            var rndZ = Random.Range(-_spawnRadius, _spawnRadius);

            return _spawnPoint.position + new Vector3(rndX, 0, rndZ);
        }
    }
}
