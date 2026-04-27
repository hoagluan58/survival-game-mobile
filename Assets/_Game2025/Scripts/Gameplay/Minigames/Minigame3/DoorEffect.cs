using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class DoorEffect : MonoBehaviour
    {
        [SerializeField] private List<DoorSymbol> _doorSymbolList;
        private DalgonaController _dalgonaController;

        public void Init(DalgonaController dalgonaController)
        {
            _dalgonaController = dalgonaController;
            SetInitSymbol();
        }

        public void SetResultSymbol(DalgonaType dalgonaType)
        {
            DoorSymbol doorSymbol = _doorSymbolList.Last();
            doorSymbol.SetSprite(_dalgonaController.DalgonaSpriteDic[dalgonaType]);            
        }

        public void SetRandomSymbol()
        {
            for (int i = 1; i < _doorSymbolList.Count - 1; i++)
            {
                DalgonaType randomType = _dalgonaController.GetRandomDalgano();
                _doorSymbolList[i].SetSprite(_dalgonaController.DalgonaSpriteDic[randomType]);
            }
        }
        
        public void SetInitSymbol()
        {
            _doorSymbolList[0].SetSprite(_dalgonaController.DalgonaSpriteDic[DalgonaType.None]);
        }

        public void DoSlide(float time)
        {
            foreach (var door in _doorSymbolList)
            {
                door.GetComponent<RectTransform>()
                    .DOAnchorPosY(-84f * (_doorSymbolList.Count - 1), time)
                    .SetRelative(true);
            }
        }

        [Button]
        public void LoadDoorSymbolList()
        {
            _doorSymbolList = GetComponentsInChildren<DoorSymbol>().ToList();
        }
    }    
}
