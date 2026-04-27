using NFramework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SquidGame.LandScape.Minigame3
{
    public class BreakDalgonaController : MonoBehaviour
    {
        private const int MAX_BREAK_COUNT = 3;

        private float _curArrowSpeed;
        private float _percentageDescrease = 0.8f;
        private int _breakCount = 0;
        private Dalgona _dalgona;
        private List<DalgonaPartOut> _parts;
        private DalgonaPartOut[] _dalgonaPartOuts;
        private int _partBreakEachTime;
        private MinigameController _controller;
        private BreakDalgonaPanelUI _ui;
        private Minigame03SO _minigame03SO;

        public void Init(MinigameController controller, BreakDalgonaPanelUI ui, Minigame03SO minigame03SO)
        {
            _controller = controller;
            _minigame03SO = minigame03SO;
            _ui = ui;
            _ui.Init(this);
        }

        public void SetDalgona(Dalgona dalgona)
        {
            _dalgona = dalgona;
        }

        private void LoadConfig()
        {                       
            _curArrowSpeed = GetConfig().Speed;
            _ui.LoadConfig(GetConfig());
        }

        private Minigame03Config GetConfig()
        {            
            int currentLevel = PlayerPrefs.GetInt(_controller.GetKeySaveLevel());
            return _minigame03SO.GetConfig(currentLevel);
        }

        public void Active(float delayTime = 0)
        {
            LoadConfig();
            _parts = new List<DalgonaPartOut>();
            foreach (var partOut in _dalgona.DalgonaPartOuts)
            {
                _parts.Add(partOut);
            }
            _partBreakEachTime = _parts.Count / MAX_BREAK_COUNT;

            StartCoroutine(CRActive());

            IEnumerator CRActive()
            {
                yield return _controller.CameraController.CRChangeVirtualCam(VirtualCamType.DalgonaBreak, delayTime);                
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
                _controller.WinLevel();
                return;
            }
            else
            {
                StartCoroutine(CRBreakPart());
            }
            _breakCount++;
            _curArrowSpeed *= _percentageDescrease * GetConfig().PercentageSpeedEachBreak;

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
            _controller.LoseLevel();
            _controller.NeedleController.SetBroken();
        }

        public void ResetBreak()
        {
            _breakCount = 0;
        }
    }
}
