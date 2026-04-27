using NFramework;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class GameSound : Singleton<GameSound>
    {
        public void PlayBGM(string audioClipPath, bool loop = true, float volume = 1f, float pitch = 1f,
                            bool ignoreListnerPause = false, bool ignoreLisnerVolume = false, float fadeTime = 0f)
            => SoundManager.I.PlayMusicResource(audioClipPath, loop, volume, pitch, ignoreListnerPause, ignoreLisnerVolume, fadeTime);

        public void StopBGM(float fadeTime = 0f)
        {
            if (SoundManager.IsSingletonAlive)
            {
                SoundManager.I.StopMusic(fadeTime);
            }
        }

        public AudioSource PlaySFX(string audioClipPath, bool loop = false, float volume = 1f, float pitch = 1f,
                                   bool ignoreListnerPause = false, bool ignoreLisnerVolume = false, float fadeTime = 0f)
            => SoundManager.I.PlaySFXResource(audioClipPath, loop, volume, pitch, ignoreListnerPause, ignoreLisnerVolume, fadeTime);

        public void PlaySFXButtonClick()
        {
            PlaySFX(Core.Define.SoundPath.SFX_BUTTON_CLICK);
        }
    }
}
