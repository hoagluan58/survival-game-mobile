using System.Collections;
using System.Collections.Generic;
using SquidGame.LandScape.Core;
using UnityEngine;

namespace SquidGame.LandScape.Home
{
    public class HomeManager : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        // AudioSource _musicSource;

        private void Start()
        {
            GameSound.I.PlayBGM(Define.SoundPath.BGM_MAIN);
            _skinnedMeshRenderer.materials[0].SetFloat(Define.MaterialPropertyName.BRIGHTNESS, 2);
        }

        void OnDisable()
        {
            GameSound.I.StopBGM();
        }
    }
}
