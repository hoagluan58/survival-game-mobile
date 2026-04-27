using UnityEngine;
using EPOOutline;
using Sirenix.OdinInspector;

namespace SquidGame.Gameplay
{
    public class BaseCharacter : MonoBehaviour
    {
        [SerializeField] private bool _isPlayer;
        [SerializeField] private CharacterHat _hat;
        [SerializeField] private CharacterSkin _skin;
        [SerializeField] private CharacterRagdoller _ragdoller;
        [SerializeField] private CharacterAnimationController _animator;
        [SerializeField] private Outlinable _outlinable;

        public CharacterHat Hat => _hat;
        public CharacterAnimationController Animator => _animator;
        public CharacterRagdoller Ragdoller => _ragdoller;

        private void Awake()
        {
            _animator.Init(this);
        }

        private void OnEnable()
        {
            _hat.Init(_isPlayer);
            _skin.Init(_isPlayer);
        }

        public void ToggleRagdoll(bool value)
        {
            _ragdoller.ToggleRagdoll(value);
            _animator.Animancer.enabled = !value;
        }

        public void ToggleGreyScale(bool value)
        {
            _hat.SetGreyScale(value);
            _skin.SetGreyScale(value);
        }

        public void ToggleOutline(bool value) => _outlinable.OutlineParameters.Enabled = value;
    }
}
