using NFramework;
using Old;
using System.Collections.Generic;
using UnityEngine;

namespace MyLib
{
    public enum CategorySound
    {
       
    }

    public class SoundManager : SingletonMono<SoundManager>
    {
        [SerializeField] private AudioSource _sourceFX = default;
        [SerializeField] private AudioSource _sourceFX_2 = default;
        [SerializeField] private AudioSource _sourceMX = default;
        [SerializeField] SoundData _soundData = default;

        private Dictionary<CategorySound, SoundObject> _mapper = new Dictionary<CategorySound, SoundObject>();

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void OnChangeMusic(bool b)
        {
            _sourceMX.mute = !b;
        }

        private void Init()
        {
            for (int i = 0; i < _soundData.SoundObjects.Count; i++)
            {
                if (!_mapper.ContainsKey(_soundData.SoundObjects[i].type))
                {
                    _mapper.Add(_soundData.SoundObjects[i].type, _soundData.SoundObjects[i]);
                }
            }
        }

        private void Start()
        {
            OnChangeMusic(DataManager.I.GameData.IsMusic);
        }

        public void PlaySoundFX(CategorySound categorySound, int id = 0)
        {
            if (DataManager.I.GameData.IsSound)
            {
                if (id == 0)
                    _sourceFX.PlayOneShot(_mapper[categorySound].audioClip);
                else _sourceFX_2.PlayOneShot(_mapper[categorySound].audioClip);
            }
        }

        public void PlayMusicMX(CategorySound categoryMusic)
        {
            _sourceMX.clip = _mapper[categoryMusic].audioClip;
            _sourceMX.Play();
        }

        public void SetVolumnSound(int vol)
        {
            _sourceFX.volume = vol;
        }

        public void SetVolumnMusic(int vol)
        {
            _sourceMX.volume = vol;
        }

        public void StopMusic()
        {
            _sourceMX.Stop();
        }

        public void StopFX()
        {
            _sourceFX.Stop();
            _sourceFX_2.Stop();
        }

        public void PauseMusic()
        {
            _sourceMX.Pause();
        }

        public void UnPauseMusic()
        {
            _sourceMX.UnPause();
        }
    }

    [System.Serializable]
    public class SoundObject
    {
        [HideInInspector] public string name;
        public CategorySound type;
        public AudioClip audioClip;
        //public bool loop = false;
        //public float volumeScale = 1;
    }
}