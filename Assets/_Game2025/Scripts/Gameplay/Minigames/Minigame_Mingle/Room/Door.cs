using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    public class Door : MonoBehaviour
    {
        Collider _doorCollider;

        private void Awake()
        {
            _doorCollider = GetComponent<Collider>();
        }

        //-116 open , 0 close
        public void Open(bool value, float duration)
        {
            _doorCollider.enabled = false;
            transform.DOKill();
            transform.DOLocalRotate(new Vector3(0, value ? -100 : 0, 0), duration).OnComplete(() =>
            {
                _doorCollider.enabled = true;
            });
        }
    }
}
