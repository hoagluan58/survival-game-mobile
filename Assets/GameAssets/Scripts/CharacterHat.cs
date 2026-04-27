using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHat : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] _meshRenderers;
    [SerializeField] private Material _characterHatMat;
    [SerializeField] private Material _characterNoHatMat;
    [SerializeField] private RandomHat _randomHat;

    private void Start()
    {
        bool isNoHat = Random.Range(0, 10) == 0;

        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            if (isNoHat)
                _meshRenderers[i].material = _characterNoHatMat;
            else
                _meshRenderers[i].material = _characterHatMat;
        }

        if (isNoHat == false)
            _randomHat.StartRandom();
    }
}
