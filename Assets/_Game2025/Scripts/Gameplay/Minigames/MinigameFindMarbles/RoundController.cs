using Cysharp.Threading.Tasks;
using NFramework;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.MinigameFindMarbles
{
    public class RoundController : MonoBehaviour
    {
        private const int MAX_ROUND = 3;

        [SerializeField] private LevelConfigSO _levelConfigSO;
        [SerializeField] private RoundConfigSO _roundConfigSO;
        [SerializeField] private List<Bowl> _allBowls;
        [SerializeField] private Transform _marbles;

        private BowlManager _bowlManager;
        private MinigameController _controller;
        private int _curLevel;
        private int _curRound;

        public int CurRound => _curRound;

        public void Init(MinigameController controller, BowlManager bowlManager, int level)
        {
            _controller = controller;
            _bowlManager = bowlManager;
            _curRound = 0;
            _curLevel = level;
        }

        public void PlayNextRound()
        {
            _curRound++;
            PlayRound(_curRound);
        }

        public void PlayRound(int round)
        {
            var roundConfig = _roundConfigSO.GetConfig(round);

            // Show bowls
            var activeBowls = new List<Bowl>();

            _allBowls.ForEach(b => b.gameObject.SetActive(false));

            for (var i = 0; i < roundConfig.BowlCount; i++)
            {
                if (_allBowls.IsIndexOutOfList(i)) break;

                var bowl = _allBowls[i];
                var position = roundConfig.FirstBowlPos + (i * roundConfig.BowlSpacing);

                bowl.gameObject.SetActive(true);
                bowl.transform.localPosition = position;
                activeBowls.Add(bowl);
            }

            _bowlManager.SetBowls(activeBowls);

            // Add Marbles 
            _bowlManager.AddMarblesToCorrectBowl(_marbles);
        }

        public async UniTaskVoid StartRound()
        {
            var shuffleInterval = _levelConfigSO.GetConfig(_curLevel).ShuffleInterval;
            var shuffleTime = _roundConfigSO.GetConfig(_curRound).ShuffleTime;

            // Start Shuffle
            await _bowlManager.ShuffleBowlsCoroutine(shuffleTime, shuffleInterval);

            _bowlManager.SetClickableBowls(true);
        }

        public bool IsMaxRound() => _curRound == MAX_ROUND;
    }
}
