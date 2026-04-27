using System.Collections;
using System.Linq;
using Redcode.Extensions;
using UnityEngine;

namespace Game2
{
    public class BotManager : MonoBehaviour
    {
        [SerializeField] private BotController[] _bots;

        private GameLevel _currentLevel;
        private Transform _endMap;
        private bool _isActive;

        public void Init(GameLevel currentLevel, Transform endMap)
        {
            _currentLevel = currentLevel;
            _endMap = endMap;

            _bots.ForEach(b => b.Init());
        }

        public void StartBotsBehaviour()
        {
            _isActive = true;
            StartCoroutine(BotsBehaviourCoroutine());
        }

        private IEnumerator BotsBehaviourCoroutine()
        {
            var waitForSeconds = new WaitForSeconds(1.8f);

            while (_isActive)
            {
                yield return null;
                // Get random alive bot
                var aliveBots = _bots.Where(b => b.IsAlive).ToArray();
                if (aliveBots.Length == 0) break;
                var randomBot = aliveBots[Random.Range(0, aliveBots.Length - 1)];

                // Get next step
                var currentStepIndex = randomBot.CurrentIndex;
                var nextStepIndex = currentStepIndex + 1;
                var nextStepInfo = _currentLevel.GetStepInfo(nextStepIndex);
                GlassPiece nextStepGlass = null;

                if (nextStepInfo.step.IsExposed)
                {
                    nextStepGlass = nextStepInfo.step.GetCorrectGlassPiece();
                }
                else
                {
                    var unbrokenGlass = nextStepInfo.step.GlassPieces.Where(gp => !gp.IsBroken).ToArray();
                    Debug.Log(unbrokenGlass.Length);
                    nextStepGlass = unbrokenGlass[Random.Range(0, unbrokenGlass.Length)];
                }

                if (nextStepGlass != null && !nextStepGlass.IsFull)
                {
                    // Perform jump
                    randomBot.JumpTo(nextStepGlass, nextStepInfo.step, () =>
                    {
                        if (nextStepInfo.isLastStep) randomBot.JumpToWin(_endMap.position);
                    });
                }
                else continue;


                yield return waitForSeconds;
            }
        }

        public void FallAllBots()
        {
            _isActive = false;
            StopAllCoroutines();
            foreach (var bot in _bots.Where(b => b.CurrentIndex >= 0 && b.IsAlive))
            {
                bot.FallDown();
            }
        }
    }
}