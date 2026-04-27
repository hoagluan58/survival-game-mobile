using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    public class PlaySoundFx
    {
        protected string _name;
        protected AudioSource _sound;

        public PlaySoundFx(string name)
        {
            _name = name;
        }

        public virtual void PlaySound(bool isPlay)
        {
            if (isPlay)
            {
                if (_sound == null) _sound = GameSound.I.PlaySFX(_name);
            }
            else
            {
                if (_sound != null)
                {
                    _sound.Stop();
                    _sound = null;
                }
            }
        }
    }

    public class DontPlayIfPlaying : PlaySoundFx
    {
        public DontPlayIfPlaying(string name) : base(name)
        {
        }

        public override void PlaySound(bool isPlay)
        {
            if (isPlay)
            {
                if (_sound == null) _sound = GameSound.I.PlaySFX(_name);
                else if (_sound.isPlaying) return;
            }
            else
            {
                if (_sound != null)
                {
                    _sound.Stop();
                    _sound = null;
                }
            }
        }
    }
}
