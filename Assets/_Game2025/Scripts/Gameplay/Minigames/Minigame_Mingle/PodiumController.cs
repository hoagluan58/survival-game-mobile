using DG.Tweening;
using System;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    public class PodiumController : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] private GameObject _block; 

        public Transform GetPodium() => _pivot;

        public void Init()
        {

        }

        public void Rotate(float duration)
        {
           
            var rotationAngle = new Vector3(0, _pivot.eulerAngles.y + 720 , 0);
            _pivot.DORotate(rotationAngle, duration, RotateMode.WorldAxisAdd)
            .SetEase(Ease.InOutSine).OnComplete(() =>
            {
                _block.SetActive(false);
            });
        }

        public void SetActiveBlock(bool value)
        {
            _block.SetActive(value);
        }
    }
}
