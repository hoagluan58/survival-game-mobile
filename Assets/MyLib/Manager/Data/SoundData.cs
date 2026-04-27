using UnityEngine;
using System.Collections.Generic;
using MyLib;

[CreateAssetMenu(fileName = "SoundData", menuName = "MyData/SoundData", order = 2)]
public class SoundData : ScriptableObject
{
    [SerializeField] private List<SoundObject> _soundObjects = new List<SoundObject>();

    public List<SoundObject> SoundObjects { get { return _soundObjects; } }

    private void OnValidate()
    {
        for (int i = 0; i < _soundObjects.Count; i++)
        {
            _soundObjects[i].name = _soundObjects[i].type.ToString();
        }
    }
}