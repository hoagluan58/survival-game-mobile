using NFramework;
using Sirenix.OdinInspector;
using SquidGame.Gameplay;
using SquidGame.Minigame03.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.Minigame03
{
    public class BreakDalgonaStep : MonoBehaviour
    {
        private const int MAX_BREAK_COUNT = 3;

        private float _curArrowSpeed;
        private int _breakCount = 0;
        private Dalgona _dalgona;
        private List<DalgonaPartOut> _parts;
        private DalgonaPartOut[] _dalgonaPartOuts;
        private int _partBreakEachTime;
        private MinigameController _controller;
        private BreakDalgonaPanelUI _ui;

        public void Init(MinigameController controller, BreakDalgonaPanelUI ui)
        {
            _controller = controller;
            _ui = ui;
            _ui.Init(this);
        }

        public void Active(Dalgona dalgona)
        {
            _dalgona = dalgona;
            _parts = new List<DalgonaPartOut>();
            foreach (var partOut in _dalgona.DalgonaPartOuts)
            {
                _parts.Add(partOut);
            }
            _partBreakEachTime = _parts.Count / MAX_BREAK_COUNT;
            _curArrowSpeed = 1.5f;

            StartCoroutine(CRActive());

            IEnumerator CRActive()
            {
                var position = _dalgona.transform.position;
                _controller.CameraController.SetPos(position.x, 8, position.z);
                yield return _controller.CameraController.CRSwitchCamera(CameraController.ECameraType.Move);
                _ui.SetActive(true);
                _ui.ToggleTweenArrow(true, _curArrowSpeed);
            }
        }

        [Button]
        public void BreakTrue()
        {
            _ui.SetActive(false);
            if (_breakCount >= MAX_BREAK_COUNT)
            {
                _parts.ForEach(x => x.Break());
                GameManager.I.Win(5f);
                return;
            }
            else
            {
                StartCoroutine(CRBreakPart());
            }
            _breakCount++;
            _curArrowSpeed -= 0.3f;

            IEnumerator CRBreakPart()
            {
                var parts = Random.Range(1, _partBreakEachTime);
                _parts.Shuffle();
                var selectedPart = _parts.Take(_partBreakEachTime).ToList();

                foreach (var part in selectedPart)
                {
                    part.Break();
                    _parts.Remove(part);
                }

                yield return new WaitForSeconds(0.5f);
                _ui.SetActive(true);
                _ui.ToggleTweenArrow(true, _curArrowSpeed);
            }
        }

        public void BreakWrong()
        {
            _ui.SetActive(false);
            foreach (var part in _dalgona.DalgonaPartIns)
            {
                part.Break();
            }
            GameManager.I.Lose();
        }
    }
}
