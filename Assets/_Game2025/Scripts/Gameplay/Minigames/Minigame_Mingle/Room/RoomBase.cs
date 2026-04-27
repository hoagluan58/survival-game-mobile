using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    public class RoomBase : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private int _id;
        [SerializeField] private bool _isLive;
        [SerializeField] private bool _isPlayerIn;
        [SerializeField] private int _characterCount;
        [ReadOnly][SerializeField] private int _maxCharacter;

        [Header("References")]
        [SerializeField] private Door _door;
        [SerializeField] private RoomText _text;
        [SerializeField] private RoomArea _roomArea;
        [SerializeField] private RoomTracker _roomTracker;
        [SerializeField] private GameObject _roomCameraGo;
        [SerializeField] private GameObject _frontCameraGo;
        [SerializeField] private GameObject _winCameraGo;
        [SerializeField] private Transform _outPoint;
        [SerializeField] private GameObject _blockGo;

        public GameObject BlockGo => _blockGo;
        public RoomText Text => _text;
        public Transform OutPoint => _outPoint;
        public bool IsLive => _isLive;
        public int Id => _id;
        public bool IsPlayerIn => _isPlayerIn;

        private bool _isFull = false;
        private List<Character> _characters;
        public void Init()
        {
            _isLive = true;
            _isFull = false;
            _characters = new List<Character>();
        }


        public void SetMaxPlayerInRoom(int count)
        {
            if (!_isLive) return;
            _maxCharacter = count;
            _characterCount = 0;
            UpdateText();
        }


        private void UpdateText()
        {
            _text.SetText($"{_characterCount}/{_maxCharacter}");
            _text.SetColor(_characterCount == _maxCharacter ? TextColor.Green : TextColor.Red);
        }

        public void SetActiveBlock(bool value)
        {
            _blockGo.SetActive(value);
        }


        private void OnEnable()
        {
            _roomTracker.OnNpcEnterAction += OnNpcEnterRoom;
            _roomTracker.OnNpcExitAction += OnNpcExitRoom;
            _roomTracker.OnPlayerEnterAction += OnPlayerEnterRoom;
            _roomTracker.OnPlayerExitAction += OnPlayerExitRoom;
        }


        private void OnDisable()
        {
            _roomTracker.OnNpcEnterAction -= OnNpcEnterRoom;
            _roomTracker.OnNpcExitAction -= OnNpcExitRoom;
            _roomTracker.OnPlayerEnterAction -= OnPlayerEnterRoom;
            _roomTracker.OnPlayerExitAction -= OnPlayerExitRoom;
        }


        public void OpenDoor(bool value, float duration)
        {
            if (!_isLive) return;
            _door.Open(value, duration);
        }


        public Vector3 GetRandomPoint()
        {
            return _roomArea.GetRandomPoint();
        }


        public void ActiveFrontCamera(bool value)
        {
            _frontCameraGo.SetActive(value);
        }



        public void ActiveRoomCamera(bool value)
        {
            _roomCameraGo.SetActive(value);
        }


        public void SetId(int i)
        {
            _id = i;
        }


        public void SetLive(bool value)
        {
            _isLive = value;
        }


        public bool CheckRoom()
        {
            return _characterCount == _maxCharacter;
        }

        public List<NpcBase> GetAllNpcs()
        {
            return _roomTracker.GetCharacters().FindAll(character => character is NpcBase).ConvertAll(character => character as NpcBase);
        }

        private void OnPlayerExitRoom(PlayerController player)
        {
            player.SetEnableOutline(false);
            player.ExitRoom();
            _isPlayerIn = false;
            _characterCount = Mathf.Max(0, --_characterCount);
            _roomCameraGo.SetActive(false);
            UpdateText();
            DisableOutlineIfFull();
        }


        private void OnPlayerEnterRoom(PlayerController player)
        {
            _isPlayerIn = true;
            _characterCount++;
            _roomCameraGo.SetActive(true);
            player.EnterRoom(_roomCameraGo.transform);
            UpdateText();
            CheckLimitCharactermEnter();
            player.SetEnableOutline(_isFull);
        }


        private void DisableOutlineIfFull()
        {
            if (!_isFull) return;
            _characters.ForEach(character => character.SetEnableOutline(false));
        }


        private void CheckLimitCharactermEnter()
        {
            _isFull = _characterCount > _maxCharacter;
            if (!_isFull) return;
            _isFull = true;

            _characters.ForEach(character => character.SetEnableOutline(true));
        }



        private void OnNpcExitRoom(NpcBase npc)
        {
            _characters.Remove(npc);
            _characterCount = Mathf.Max(0, --_characterCount);
            UpdateText();
        }


        private void OnNpcEnterRoom(NpcBase npc)
        {
            _characters.Add(npc);
            _characterCount++;
            UpdateText();
        }


        public void UpdateState()
        {
            if (!_isLive)
            {
                _text.SetText("X").SetColor(TextColor.Red);
                SetActiveBlock(true);
            }
        }

        public void ActiveWinCamera()
        {
            _roomCameraGo.SetActive(false);
            _winCameraGo.SetActive(true);
        }



        [Button]
        private void GetReferences()
        {

            foreach (Transform room in transform)
            {
                Door door = room.GetComponent<Door>();
                if (door != null)
                {
                    _door = door;
                }
            }
        }
        [Button]
        private void OpenDoor()
        {
            _door.Open(true, 1);
        }
        [Button]
        private void CloseDoor()
        {
            _door.Open(false, 1);
        }


    }
}
