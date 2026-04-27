using SquidGame.LandScape.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private BaseCharacter _model;
        [SerializeField] private GameObject _bloodFX1;
        [SerializeField] private GameObject _bloodFX2;

        [SerializeField] private Transform _headPos;

        public Transform HeadPos => _headPos;
        public BaseCharacter Model => _model;

        public void OnDie()
        {
            _bloodFX1.SetActive(true);
            _bloodFX2.SetActive(true);
        }

        public void OnRevive()
        {
            _bloodFX1.SetActive(false);
            _bloodFX2.SetActive(false);
        }
    }
}
