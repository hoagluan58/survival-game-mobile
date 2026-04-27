using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.Minigame21.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.Minigame21
{
    public class RoundController : MonoBehaviour
    {
        private const int COUNTDOWN_TIME = 25;

        private MinigameController _controller;
        private Minigame21MenuUI _ui;
        private int _timeLeft;
        private int _groupRequire;
        private int _currentRound;
        private Coroutine _coroutine;
        private List<int> _previousGroupCounts = new List<int>();

        public int TimeLeft => _timeLeft;
        public int CurrentRound => _currentRound;
        public int GroupRequire => _groupRequire;

        public void Init(MinigameController controller, Minigame21MenuUI ui)
        {
            _controller = controller;
            _ui = ui;
            _previousGroupCounts.Clear();
            _currentRound = 1;
        }

        public void OnPrepare()
        {
            _groupRequire = GetGroupRequire();
            _previousGroupCounts.Add(_groupRequire);
        }

        public void OnStart()
        {
            StartCountdown();
        }

        public void OnWinRound()
        {
            _currentRound++;
        }

        public void OnLoseRound()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }

        private void StartCountdown()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(CRCountdown());

            IEnumerator CRCountdown()
            {
                var waiter = new WaitForSeconds(1f);

                _timeLeft = COUNTDOWN_TIME;
                _ui.SetCountdownText(_timeLeft);

                while (_timeLeft > 0)
                {
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG21_TIME_COUNTDOWN);
                    _timeLeft--;
                    _ui.SetCountdownText(_timeLeft);
                    yield return waiter;
                }

                GameManager.I.Lose();
            }
        }

        private int GetGroupRequire()
        {
            var rndGroup = Random.Range(3, 7);

            while (_previousGroupCounts.Contains(rndGroup))
            {
                rndGroup = Random.Range(3, 7);
            }

            return rndGroup;
        }


    }
}
