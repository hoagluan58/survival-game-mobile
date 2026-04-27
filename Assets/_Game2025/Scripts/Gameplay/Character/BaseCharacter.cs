using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Game
{
    public class BaseCharacter : MonoBehaviour
    {
        [SerializeField] private List<CharacterComponent> _components;
        [SerializeField] private bool _isPlayer;

        public bool IsPlayer => _isPlayer;

        private void Awake() => Init();

        private void Init()
        {
            foreach (var component in _components)
            {
                component.Init(this);
            }
        }

        public void ToggleGreyScale(bool value)
        {
            GetCom<CharacterBodySkin>().SetGreyScale(value);
            // GetCom<CharacterHair>().SetGreyScale(value);
        }

        public T GetCom<T>() where T : CharacterComponent => _components.Find(c => c is T) as T;
    }
}