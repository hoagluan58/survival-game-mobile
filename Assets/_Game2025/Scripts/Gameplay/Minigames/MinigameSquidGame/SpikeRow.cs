using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NFramework;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class SpikeRow : MonoBehaviour
    {
        [SerializeField] private GameObject _spikeOject;
        [SerializeField] private GameObject _healLeft, _healRight;

        float _moveDuration;
        private void Start()
        {
            RandomSide();
        }

        public void InitMoveDuration(float duration)
        {
            _moveDuration = duration;
            RandomSide();
        }

        void RandomSide()
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
                _spikeOject.transform.SetPosX(_spikeOject.transform.position.x * -1);

            _spikeOject.transform.DOMoveX(_spikeOject.transform.position.x * -1, _moveDuration)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(false);

            _healLeft.transform.SetPosX(UnityEngine.Random.Range(-7, 0));
            _healRight.transform.SetPosX(UnityEngine.Random.Range(0, 7));
            gameObject.SetActive(true);

        }
    }
}
