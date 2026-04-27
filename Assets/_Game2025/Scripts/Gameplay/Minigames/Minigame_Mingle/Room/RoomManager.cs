using DG.Tweening;
using Redcode.Extensions;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Core;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.MinigameMingle
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private RoomBase _roomPrefab;
        [SerializeField] private Transform _parentRoom;
        [SerializeField] private List<RoomBase> _rooms;

        private Dictionary<bool, PlaySoundFx> _dicDoorSound;

        public void Init()
        {
            _rooms.ForEach(room => room.Init());
            _dicDoorSound = new Dictionary<bool, PlaySoundFx>();
            _dicDoorSound.Add(true, new PlaySoundFx(Define.SoundPath.SFX_MINGLE_OPEN_DOOR));
            _dicDoorSound.Add(false, new PlaySoundFx(Define.SoundPath.SFX_MINGLE_CLOSE_DOOR));
        }


        public void OpenRooms(bool value,float duration)
        {
            DOVirtual.DelayedCall(0.1f, () => GameSound.I.PlaySFX(value ? Define.SoundPath.SFX_MINGLE_OPEN_DOOR : Define.SoundPath.SFX_MINGLE_CLOSE_DOOR));
            // _dicDoorSound[value].PlaySound(true);
            _rooms.ForEach(room => room.OpenDoor(value, duration)); 
        }


        public void ZoomInRoom(int target, UnityAction onZoomInCompleted)
        {
            var room = GetRandomLiveRoom();
            room.Text.SetColor(TextColor.Yellow).SetText("?");
            room.ActiveFrontCamera(true);
            DOVirtual.DelayedCall(1, () =>
            {
                room.Text.SetColor(TextColor.Green);
                room.Text.SetTextAnimation(target, 2, () =>
                {
                    DOVirtual.DelayedCall(1, () => {
                        room.ActiveFrontCamera(false);
                        onZoomInCompleted?.Invoke();
                    });
                });
            });
        }

        //9
        public RoomManager SetActiveRooms(int roomCount)
        {
            _rooms.ForEach(room => room.SetLive(true));
            SetLiveRoom(_rooms.Count - roomCount, false);
            _rooms.ForEach(x => x.UpdateState());
            return this;
        }


        public void SetActiveBlocks(bool value)
        {
            _rooms.ForEach(x =>
            {
                if(x.IsLive)
                    x.SetActiveBlock(value);
            });
        }


        public List<RoomBase> GetLiveRooms()
        {
            return _rooms.Where(room => room.IsLive).ToList();
        }


        public RoomBase GetRandomLiveRoom()
        {
            return _rooms.Where(room => room.IsLive).GetRandomElement();
        }


        public void StartStep(int count)
        {
            _rooms.ForEach(room => room.SetMaxPlayerInRoom(count));
        }


        public RoomBase GetPlayerRoom()
        {
            return _rooms.FirstOrDefault(room => room.IsPlayerIn);
        }



        public void ResetTextRoom()
        {
            _rooms.ForEach(room => {
                if (room.IsLive)
                    room.Text.SetColor(TextColor.Red).SetText("?");
                else
                    room.Text.SetColor(TextColor.Red).SetText("x");
            });
        }


        private List<RoomBase> GetRandomRoom(int count)
        {
            return _rooms.GetRandomElements(count);
        }


        private void SetLiveRoom(int count, bool value)
        {
            var rooms = GetRandomRoom(count);
            rooms.ForEach(room => room.SetLive(value));
        }



#if UNITY_EDITOR
        [Button]
        private void GenRooms()
        {
            for (int i = 0; i < _rooms.Count; i++)
            {
                var room = (RoomBase)PrefabUtility.InstantiatePrefab(_roomPrefab, _parentRoom);
                room.transform.position = _rooms[i].transform.position;
                room.transform.rotation = _rooms[i].transform.rotation;
                room.transform.localScale = _rooms[i].transform.localScale;
                room.name = "Room_" + i;
            }
        }

        [Button]
        private void GetRooms()
        {
            _rooms = new List<RoomBase>();

            foreach (Transform room in _parentRoom)
            {
                RoomBase roomBase = room.GetComponent<RoomBase>();
                if (roomBase != null)
                {
                    _rooms.Add(roomBase);
                }
            }
        }

        [Button]
        private void SetId()
        {
            for (int i = 0; i < _rooms.Count; i++)
            {
                _rooms[i].SetId(i);
            }
        }






#endif
    }

}
