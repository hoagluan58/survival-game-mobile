using NFramework;
using SquidGame.LandScape.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class DalgonaController : MonoBehaviour
    {
        public static Action<List<int>> OnTriggerDoor;
        [SerializeField] private SerializableDictionary<DalgonaType, Dalgona> _dalgonaDic = new SerializableDictionary<DalgonaType, Dalgona>();
        [SerializeField] private SerializableDictionary<DalgonaType, Sprite> _dalgonaSpriteDic = new SerializableDictionary<DalgonaType, Sprite>();
        [SerializeField] private float _effectTime = 3f;
        [SerializeField] private Transform _tfSpawnHolder;

        private DalgonaType _currentDalgonaType;
        private MinigameController _controller;
        private Dalgona _currentDalgona;

        public float EffectTime => _effectTime;
        public SerializableDictionary<DalgonaType, Sprite> DalgonaSpriteDic => _dalgonaSpriteDic;


        public void Init(MinigameController controller)
        {
            _controller = controller;            
        }

        public void SetRandomDalgano(int idDoorTouch)
        {
            List<int> typeList = new List<int> {0, 1, 2, 3};
            typeList.Shuffle();
            _currentDalgonaType = (DalgonaType)typeList[idDoorTouch];
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG03_RANDOM);
            OnTriggerDoor?.Invoke(typeList);            
        }    

        public DalgonaType GetRandomDalgano()
        {
            return (DalgonaType)UnityEngine.Random.Range(0, 4);
        }
        
        public void LoadDalgona(float delayTime, Action onComplete)
        {            
            StartCoroutine(COLoadMapDalgona(delayTime, onComplete));
        }

        private IEnumerator COLoadMapDalgona(float delayTime, Action onComplete)
        {
            SpawnDalgona();
            yield return new WaitForSeconds(delayTime);
            _currentDalgona.gameObject.SetActive(true);
            yield return null;
            onComplete?.Invoke();
        }

        public void SpawnDalgona()
        {
            if(_currentDalgona != null)
            {
                Destroy(_currentDalgona.gameObject);
                _currentDalgona = null;
            }
            _currentDalgona = Instantiate(_dalgonaDic[_currentDalgonaType], _tfSpawnHolder);
            _currentDalgona.transform.position = _dalgonaDic[_currentDalgonaType].transform.position;
            _currentDalgona.transform.localScale = _dalgonaDic[_currentDalgonaType].transform.localScale;
        }

        public Dalgona GetCurrentDalgona()
        {
            return _currentDalgona;
        }
    }

    public enum DalgonaType
    {
        None = -1,
        Circle = 0,
        Trigangle = 1,
        Star = 2,
        Umbrella = 3,
    }
}
