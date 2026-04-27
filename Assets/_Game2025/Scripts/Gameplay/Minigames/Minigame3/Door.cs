using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private float _doorContainCharMax = 5f;
        [SerializeField] private DoorEffect _doorEffect;
        private MinigameController _controller;
        private float _doorContainCharCount;

        public void Init(MinigameController minigameController)
        {
            _controller = minigameController;
            _doorEffect.Init(minigameController.DalgonaController);
        }

        public void SetContain()
        {
            _doorContainCharCount++;
        }

        public bool CanHaveChar()
        {
            return _doorContainCharCount < _doorContainCharMax;
        }

        private void Awake()
        {
            DalgonaController.OnTriggerDoor += DoDoorEffect;
        }

        private void OnDestroy()
        {
            DalgonaController.OnTriggerDoor -= DoDoorEffect;
        }

        public async void TriggerDoor()
        {
            await UniTask.Delay(1500, cancellationToken: destroyCancellationToken);
            _controller.DalgonaController.SetRandomDalgano(_id);
            _controller.StartCutPagonalStep(_controller.DalgonaController.EffectTime + 1.5f, 1f);
        }

        private void DoDoorEffect(List<int> dalgonaTypeList)
        {
            DalgonaController.OnTriggerDoor -= DoDoorEffect;
            _doorEffect.SetResultSymbol((DalgonaType)dalgonaTypeList[_id]);
            _doorEffect.SetRandomSymbol();
            _doorEffect.DoSlide(_controller.DalgonaController.EffectTime);
        }
    }
}
