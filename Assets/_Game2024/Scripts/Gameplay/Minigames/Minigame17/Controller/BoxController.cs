using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.Minigame17.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.Minigame17
{
    public class BoxController : MonoBehaviour
    {
        [SerializeField] private List<Box> _boxes;

        [Header("CONFIG")]
        [SerializeField] private SerializableDictionary<EBoxShape, int> _config;

        private Minigame17MenuUI _ui;
        private MinigameController _controller;
        private List<EBoxShape> _levelShapes = new List<EBoxShape>();
        private Box _cacheBox;

        public void Init(MinigameController controller, Minigame17MenuUI ui)
        {
            _controller = controller;
            _ui = ui;

            GenLevel();
            InitBoxes();

            void GenLevel()
            {
                _levelShapes = new List<EBoxShape>();
                foreach (var pair in _config)
                {
                    for (var i = 0; i < pair.Value; i++)
                    {
                        _levelShapes.Add(pair.Key);
                    }
                }
                _levelShapes.Shuffle();
            }

            void InitBoxes()
            {
                for (var i = 0; i < _boxes.Count; i++)
                {
                    var index = i;
                    var box = _boxes[index];
                    var shape = _levelShapes[index];
                    box.Init(shape);
                }
            }
        }

        public IEnumerator CRShowAllBox()
        {
            var duration = 3;
            var waiter = new WaitForSeconds(duration);
            _boxes.ForEach(x => x.Open(true, 0.5f));
            _ui.ShowTutorialText(true);
            _ui.ShowAllCountdown(duration, true);
            yield return waiter;
            _boxes.ForEach(x => x.Close(true, 0.5f));
            _ui.ShowTutorialText(false);
            _ui.ShowAllCountdown(0, false);
        }

        public IEnumerator CRHandleBoxClicked(Box newBox)
        {
            // Assign first box
            if (_cacheBox == null)
            {
                _cacheBox = newBox;
                _cacheBox.Open(true, 0.1f);
                yield break;
            }
            else // Check box
            {
                if (_cacheBox == newBox) yield break; // same box
                else // Click on different box
                {
                    newBox.Open(true, 0.1f);
                    yield return new WaitForSeconds(0.2f);
                    if (IsSameShape(_cacheBox.Shape, newBox.Shape))
                    {
                        GameSound.I.PlaySFX(Define.SoundPath.SFX_MG14_CORRECT);
                        VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
                        _cacheBox.FullyOpen();
                        newBox.FullyOpen();
                        CheckWin();
                    }
                    else
                    {
                        GameSound.I.PlaySFX(Define.SoundPath.SFX_MG14_WRONG);
                        _cacheBox.Close(true, 0.1f);
                        newBox.Close(true, 0.1f);
                    }
                    yield return new WaitForSeconds(0.1f);
                    _cacheBox = null;

                }
            }

            bool IsSameShape(EBoxShape shape1, EBoxShape shape2) => shape1 == shape2;
        }

        private void CheckWin()
        {
            var isWin = _boxes.All(box => box.IsFullyOpen);
            if (isWin)
            {
                GameManager.I.Win(6f);
            }
        }
    }

    public enum EBoxShape
    {
        Circle,
        Triangle,
        Star,
        Umbrella,
    }
}
