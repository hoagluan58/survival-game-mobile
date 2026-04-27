using NFramework;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    public class NpcManager : MonoBehaviour
    {
        [SerializeField] private RingAreaSpawner _ringAreaSpawner;
        [SerializeField] private RoomManager _roomManager;
        [SerializeField] private NpcBase _npcPrefab;
        [SerializeField] private Transform _container;
        [SerializeField] private List<NpcBase> _npcs;

        public void SetContainer(Transform parent)
        {
            _container.parent = parent;
        }


#if UNITY_EDITOR
        [Button]
        public void SpawnRandom()
        {
            _npcs = new List<NpcBase>();
            for (int i = 0; i < 60; i++)
            {
                var npc = (NpcBase)PrefabUtility.InstantiatePrefab(_npcPrefab);
                npc.transform.parent = _container;
                npc.transform.position = _ringAreaSpawner.GetRandomPointArea();
                npc.name = "Npc_" + i.ToString("F0");
                npc.transform.position = _ringAreaSpawner.GetRandomPointArea();
                _npcs.Add(npc);
            }
        }
#endif

        public void Init()
        {
            _npcs.ForEach(x => x.SetRingAreaSpawner(_ringAreaSpawner).Init());
        }


        public void StartStep(int count)
        {
            var npcs = GetLiveNpcs();
            var rooms = _roomManager.GetLiveRooms();
            FitNpcsToRoom(npcs, rooms, count - 1);
            npcs.ForEach(x => x.SetRoom(rooms.PickRandom(1)[0]).SwitchState(UnityEngine.Random.Range(0, 10) < 2 ? NpcState.Wander : NpcState.EnterRoom));
        }


        private void FitNpcsToRoom(List<NpcBase> npcs, List<RoomBase> rooms, int count)
        {
            var roomCount = GetRoomCount(npcs.Count, count);
            for (int i = 0; i < roomCount; i++)
            {
                var npcsMove = npcs.GetRange(0, count);
                var room = rooms[UnityEngine.Random.Range(0, rooms.Count)];
                npcs.RemoveRange(0, count);
                rooms.Remove(room);
                GotoRoom(npcsMove, room);
            }
        }


        private int GetRoomCount(int max, int count)
        {
            var roomCount = 0;
            roomCount = UnityEngine.Random.Range(1, 4);
            var npcCount = roomCount * count;
            if (npcCount <= max)
                return roomCount;
            return GetRoomCount(max, count);
        }


        private void GotoRoom(List<NpcBase> npcs, RoomBase roomBase)
        {
            npcs.ForEach(x => x.SetRoom(roomBase).SwitchState(NpcState.EnterRoom));
        }


        public void GoToPlatform()
        {
            _ringAreaSpawner.GenerateSpawnPositionsInPodium();
            _npcs.ForEach(x => x.SwitchState(NpcState.GoToPodium));
        }


        public bool AllNpcInPodium()
        {
            return _npcs.TrueForAll(x => x.IsInPodium());
        }


        private List<NpcBase> GetLiveNpcs()
        {
            return _npcs.FindAll(x => x.IsLive);
        }


        public void KillAllWanderNpc(int round)
        {
            _npcs.ForEach(x =>
            {
                x.SetRound(round);
                x.ChangeStateToDieIfWander();
            });
        }


        public void KillAllNpcOutsidePodium()
        {

        }


        public void RemoveLiveNpcs(float percent, int round)
        {
            var liveNpcs = GetLiveNpcs().ToList();
            var npcRoom = _roomManager.GetPlayerRoom().GetAllNpcs();
            npcRoom.ForEach(x => liveNpcs.Remove(x));
            var removeCount = Mathf.FloorToInt(liveNpcs.Count * percent);
            liveNpcs.GetRange(0, removeCount).ForEach(x =>
            {
                x.SwitchState(NpcState.Die);
                x.SetRound(round);
            });
        }


        public void ClearDieNpcs()
        {
            _npcs.ForEach(x =>
            {
                if (!x.IsLive)
                {
                    x.gameObject.SetActive(false);
                }
            });
        }

        public void ReviveAllNpcsInRound(int stepIndex)
        {
            _npcs.ForEach((x) =>
            {
                if (x.Round == stepIndex)
                {
                    x.OnRevive();
                    x.gameObject.SetActive(true);
                    x.transform.position = _ringAreaSpawner.GetRandomPointPodium();
                    x.SetImmediateState(NpcState.Wander);
                    x.EnableDustFx();
                }
            });
        }
    }
}
