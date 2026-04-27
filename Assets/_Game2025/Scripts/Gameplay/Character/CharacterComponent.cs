using UnityEngine;

namespace SquidGame.LandScape.Game
{
    public class CharacterComponent : MonoBehaviour
    {
        protected BaseCharacter _baseCharacter;

        public virtual void Init(BaseCharacter character) => _baseCharacter = character;
    }
}
