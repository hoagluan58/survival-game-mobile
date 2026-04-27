using System.Collections;
using System.Collections.Generic;
using Redcode.Extensions;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class PiggyBreakCutScene : MonoBehaviour
    {
        [SerializeField] private GameObject _piggyBank, _piggyBankBreak;

        public void Show()
        {
            _piggyBank.SetActive(false);
            _piggyBankBreak.SetActive(true);
            // _rigidbody.AddExplosionForce(10000, transform.position, 10);
        }
    }
}
