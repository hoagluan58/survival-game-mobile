using Redcode.Extensions;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6.Ddakji
{
    public class DdakjiCollisionHandler : MonoBehaviour
    {
        private Ddakji _ddakji;

        private void Awake() => _ddakji = GetComponent<Ddakji>();

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponentInParent<Ddakji>(out var ddakji))
            {
                if (_ddakji.IsFlip)
                {
                    _ddakji.Flip(true);
                    _ddakji.SetFlipOnCollision(false);
                }
            }
        }
    }
}
