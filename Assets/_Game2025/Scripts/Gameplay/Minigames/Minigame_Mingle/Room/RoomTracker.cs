using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.MinigameMingle
{
    public class RoomTracker : MonoBehaviour
    {
        [SerializeField] private Collider _triggerCollider;
        
        public event UnityAction<PlayerController> OnPlayerEnterAction;
        public event UnityAction<PlayerController> OnPlayerExitAction;
        public event UnityAction<NpcBase> OnNpcEnterAction;
        public event UnityAction<NpcBase> OnNpcExitAction;



        public List<Character> GetCharacters()
        {
            var characterList = new List<Character>();
            var cols = Physics.OverlapBox(_triggerCollider.bounds.center, _triggerCollider.bounds.extents);
            for (int i = 0; i < cols.Length; i++)
            {
                var character = cols[i].GetComponent<Character>();
                if (character) characterList.Add(character);
            }
            return characterList;
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<PlayerController>();
            if(player) OnPlayerEnterAction?.Invoke(player);
            var npc = other.GetComponent<NpcBase>();
            if (npc) OnNpcEnterAction?.Invoke(npc);

        }


        private void OnTriggerExit(Collider other)
        {
            var player = other.GetComponent<PlayerController>();
            if (player) OnPlayerExitAction?.Invoke(player);
            var npc = other.GetComponent<NpcBase>();
            if (npc) OnNpcExitAction?.Invoke(npc);
        }
    }
}
