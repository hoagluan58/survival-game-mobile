using Old;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundFX : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private float _delayTime;


    private void OnEnable()
    {
        Invoke(nameof(PlayAudio), _delayTime);
    }

    private void PlayAudio()
    {
        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        if (DataManager.I && DataManager.I.GameData.IsSound)
            _audioSource.Play();
    }
}
