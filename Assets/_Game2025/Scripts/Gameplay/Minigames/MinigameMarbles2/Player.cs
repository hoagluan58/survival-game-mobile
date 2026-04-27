using DG.Tweening;
using NFramework;
using System;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMarblesVer2
{


    public class Player : Character
    {

        [SerializeField] private CharacterAnimationHandler _characterAnimationHandler;
        [SerializeField] private ThrowHandler _throwHandler;
        [SerializeField] private ForceSelector _forceSelector;

        [Header("FX")]
        [SerializeField] private ParticleSystem _bloodFx;

        public ForceSelector ForceSelector => _forceSelector;
        private bool IsPlayedTutorial{ get =>  PlayerPrefs.GetInt("MinigameMarblesVer2_TUTORIAL", 0) == 1; set => PlayerPrefs.SetInt("MinigameMarblesVer2_TUTORIAL", value ? 1 : 0); }


        private void OnEnable()
        {
            _forceSelector.OnCompleted += OnForceSelectorCompleted;
        }

        private void OnDisable()
        {
            _forceSelector.OnCompleted -= OnForceSelectorCompleted;
        }

        private void Start()
        {
            _characterAnimationHandler.Stand();
        }


        private void OnShowForceBar()
        {
            Controller.UI.ShowNotification("Tap to throw !");
        }

        private void OnShowDirectionArrow()
        {
            Controller.UI.ShowNotification("Tap to get direction !");
        }


        public override void Init(Marblesver2Controller controller)
        {
            base.Init(controller);
            SetMarble(3);
        }

        private void OnForceSelectorCompleted()
        {
            Throw();
        }


        public void Throw()
        {
            _characterAnimationHandler.Throw(StartThrowMarble);
            RemoveMarble();
        }


        private void StartThrowMarble()
        {
            VibrationManager.I.Haptic(VibrationManager.EHapticType.Selection);
            _throwHandler.Throw(_forceSelector.GetDirection(), _forceSelector.GetForce(), CompletedThrowMarble);
        }


        private void CompletedThrowMarble()
        {
            CurrentTurnScore = _throwHandler.GetScore();
            IsPlayedTutorial = true;
            EndTurn();
        }


        public void Dance()
        {
            _characterAnimationHandler.Dance();
        }


        public override void StartTurn()
        {
            CurrentTurnScore = 0;
            InvokeStartTurn();
            this.InvokeDelay(1.5f,() => _forceSelector.Show());
        }


        public override void EndTurn()
        {
            _forceSelector.Hide();
            InvokeEndTurn();
        }


        public override void OnWin()
        {
            transform.eulerAngles = Vector3.up * 180;
            _characterAnimationHandler.Dance();
        }


        public override void OnLose()
        {
            _characterAnimationHandler.Died();
            _bloodFx.gameObject.SetActive(true);
            _bloodFx.Play();
        }

        internal void Revive()
        {
            _bloodFx.gameObject.SetActive(false);
            _bloodFx.Stop();
            _characterAnimationHandler.Stand();
            _throwHandler.ClearMarbles();
        }
    }



}
