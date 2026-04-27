using NFramework;
using SquidGame.Minigame15;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMarblesVer2
{
    public class Opponent : Character
    {
        [SerializeField] private CharacterAnimationHandler _characterAnimationHandler;
        [SerializeField] private ThrowHandler _throwHandler;
        [Header("FX")]
        [SerializeField] private ParticleSystem _bloodFx;

        public override void Init(Marblesver2Controller controller)
        {
            base.Init(controller);
            SetMarble(3);
        }


        private void Start()
        {
            _characterAnimationHandler.Stand();
        }



        public override void EndTurn()
        {
         
        }


        public override void OnLose()
        {
            _characterAnimationHandler.Died();
            _bloodFx.Play();
        }


        public override void OnWin()
        {
            
        }


        public override void StartTurn()
        {
            this.InvokeDelay(UnityEngine.Random.Range(0, 1f), Throw);
        }


        public void Throw()
        {
            _characterAnimationHandler.Throw(StartThrowMarble);
            RemoveMarble();
        }


        private void StartThrowMarble()
        {
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Selection);
            var throwDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0.25f, 1f);
            throwDirection = throwDirection.normalized;
            var throwForce = UnityEngine.Random.Range(5f, 15f);
            _throwHandler.Throw(throwDirection, throwForce, CompletedThrowMarble);
        }


        private void CompletedThrowMarble()
        {
            CurrentTurnScore = _throwHandler.GetScore();
            EndTurn();
            InvokeEndTurn();
        }

        internal void Revive()
        {
            _throwHandler.ClearMarbles();
        }
    }
}
