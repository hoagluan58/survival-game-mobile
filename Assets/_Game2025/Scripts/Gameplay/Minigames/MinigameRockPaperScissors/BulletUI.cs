using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace SquidGame.LandScape.MinigameRockPaperScissors
{
    public enum BulletState
    {
        Mask,
        Empty,
        Bullet,
    }

    public class BulletUI : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletGo;
        [SerializeField] private GameObject _emptyGo;
        [SerializeField] private GameObject _maskGo;


        public void ChangeStatus(BulletState value) {
            _bulletGo.SetActive(value == BulletState.Bullet);
            _emptyGo.SetActive(value == BulletState.Empty);
            _maskGo.SetActive(value == BulletState.Mask);
        }


        [Button]
        public void RotateWorldEuler()
        {
            _bulletGo.transform.rotation = Quaternion.LookRotation(Vector3.forward,Vector3.up);
            _emptyGo.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            _maskGo.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }

        internal void Initialized()
        {
            ChangeStatus(BulletState.Mask);
        }

        private void LateUpdate()
        {
            RotateWorldEuler();
        }
    }
}
