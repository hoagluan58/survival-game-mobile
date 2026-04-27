using SquidGame.LandScape.Core;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6.ThrowStoneGame
{
    public class Target : MonoBehaviour
    {
        private float ANGLE = 45f;
        private ThrowStoneGameController _controller;
        private bool _isFallen;

        public void OnEnter(ThrowStoneGameController controller)
        {
            _controller = controller;
            _isFallen = false;
        }

        public void Update()
        {
            if (_isFallen) return;

            if (Vector3.Angle(transform.up, Vector3.up) > ANGLE)
            {
                _isFallen = true;
                _controller.Win().Forget();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<Stone>(out var stone))
            {
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_TARGET_FALL);
            }
        }
    }
}
