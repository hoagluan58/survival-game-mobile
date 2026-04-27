using SquidGame.LandScape.Core;
using UnityEngine;

namespace SquidGame.LandScape.Minigame2
{
    public class GlassCollision : MonoBehaviour
    {
        private GlassPanel _panel;

        public void Init(GlassPanel panel)
        {
            _panel = panel;
        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    if (!AllowCollision(collision.gameObject.tag)) return;

        //    TryBreakPanel();
        //}

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (!AllowCollision(other.tag)) return;

        //    var isBreak = TryBreakPanel();
        //    if (!isBreak)
        //    {
        //        GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_JUMP_ON_GLASS);
        //        return;
        //    }
        //}

        //private bool TryBreakPanel()
        //{
        //    if (_panel.IsBreak) return false;
        //    if (_panel.Data.IsTrueMove) return false;
        //    _panel.Break();
        //    return true;
        //}

        //private bool AllowCollision(string tag) => tag == Define.TagName.PLAYER || tag == Define.TagName.BOT;
    }
}
