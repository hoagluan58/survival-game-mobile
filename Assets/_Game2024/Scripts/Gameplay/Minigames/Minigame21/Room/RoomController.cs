using DG.Tweening;
using EPOOutline;
using NFramework;
using SquidGame.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SquidGame.Minigame21
{
    public class RoomController : MonoBehaviour
    {
        [SerializeField] private GameObject _door;
        [SerializeField] private GameObject _disableObject;
        [SerializeField] private TextMeshPro _text;
        [SerializeField] private List<Transform> _positions;
        [SerializeField] private RoomHandler _roomHandler;
        [SerializeField] private Outlinable _outlinable;

        [SerializeField] private bool _isInfrontOfCamera;

        private Coroutine _closeDoorCoroutine;
        private List<Bot> _queueBots = new List<Bot>();
        private RoomManager _roomManager;
        private int _maxCapacity;

        private bool _isPlayerRoom;
        private bool _isDisable;
        private bool _isOpen;

        public bool IsInfrontOfCamera => _isInfrontOfCamera;
        public int MaxCapacity => _maxCapacity;
        public RoomHandler RoomHandler => _roomHandler;
        public bool IsPlayerRoom => _isPlayerRoom;
        public List<Bot> QueueBots => _queueBots;

        public void Init(RoomManager roomManager)
        {
            _roomManager = roomManager;
            _roomHandler.Init(roomManager.Controller, this);

            _roomHandler.CurCapacity = 0;
            _maxCapacity = 0;
            _disableObject.SetActive(false);
            _text.gameObject.SetActive(false);
            _isPlayerRoom = false;
            ToggleDoor(false, true);
            ToggleOutline(false);
        }

        public void OnPrepare(int capacity, bool isDisable, bool isPlayerRoom)
        {
            _isPlayerRoom = isPlayerRoom;
            _isDisable = isDisable;
            _maxCapacity = capacity;
            _roomHandler.OnPrepare();
            ToggleOutline(false);
        }

        public void OnActive()
        {
            _disableObject.SetActive(_isDisable);
            _text.gameObject.SetActive(!_isDisable);
            UpdateText(0);

            if (_isDisable)
            {
                return;
            }

            ToggleDoor(true, true);
        }

        public void Queue(Bot bot)
        {
            _queueBots.Add(bot);
        }

        public bool IsValid() => !_isDisable
                                 && _queueBots.Count < _maxCapacity
                                 && !_isPlayerRoom;

        public Vector3 RandomPositionInRoom() => _positions.RandomItem().position;

        public void ToggleDoor(bool isOpen, bool isForced)
        {
            _isOpen = isOpen;
            if (isForced)
            {
                _door.transform.localEulerAngles = new Vector3(180f, _isOpen ? 90f : 0f, 0f);
                _roomManager.Controller.BuildNavMesh();
            }
            else
            {
                _door.transform.DOKill();
                _door.transform.localEulerAngles = new Vector3(180f, _isOpen ? 0f : 90f, 0f);
                _door.transform.DOLocalRotate(new Vector3(180f, _isOpen ? 90f : 0f, 0f), 0.5f)
                               .SetEase(Ease.Linear)
                               .OnComplete(() =>
                               {
                                   _roomManager.Controller.BuildNavMesh();
                               });
            }
        }

        public void UpdateText(int count)
        {
            _text.SetText($"{count}");
        }

        public void TryCloseDoor()
        {
            if (_closeDoorCoroutine != null)
            {
                StopCoroutine(_closeDoorCoroutine);
            }
            _closeDoorCoroutine = StartCoroutine(CRCloseDoor());
        }

        public void ToggleOutline(bool value) => _outlinable.OutlineParameters.Enabled = value;

        private IEnumerator CRCloseDoor()
        {
            var waiter = new WaitUntil(() => _roomHandler.CurCapacity == MaxCapacity);
            if (_roomHandler.CurCapacity > MaxCapacity)
            {
                yield return waiter;
            }

            if (IsPlayerRoom)
            {
                ToggleDoor(false, true);
                yield break;
            }

            var allBotInRoom = _queueBots.All(x => x.IsInRoom);
            if (allBotInRoom)
            {
                ToggleDoor(false, true);
            }
            yield return null;
        }
    }
}
