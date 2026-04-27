using DG.Tweening;
using NFramework;
using Redcode.Extensions;
using RotaryHeart.Lib.SerializableDictionary;
using SquidGame.LandScape.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame16
{
    public class LightController : MonoBehaviour
    {
        private int _maxRound = 2;
        private int _maxLights = 5;

        [SerializeField] private SerializableDictionaryBase<ELightType, LightPole> _lights;
        [SerializeField] private Transform _tfLightParent;
        [SerializeField] private LightConfig _lightConfig;
        [SerializeField] private LevelSaver LevelSaver;
        private int _roundWin;
        private List<ELightType> _curLightSequence;
        private MinigameController _controller;
        private Minigame16MenuUI _ui;

        private LightContent _content; 
        public List<ELightType> CurLightSequence => _curLightSequence;

        public void Init(MinigameController controller, Minigame16MenuUI ui)
        {
            _content = _lightConfig.GetContentByLevel(controller.Level);
            _maxRound = _content.MaxLights.Count;
            _roundWin = 0;
            _maxLights = _content.MaxLights[_roundWin];
            _controller = controller;
            _ui = ui;
            foreach (var light in _lights.Values)
            {
                light.Init();
            }
            _curLightSequence = new List<ELightType>();
            _tfLightParent.SetPositionY(0f);
            
        }

        public void PrepareGame()
        {
            var durationMoveLightParent = 1f;
            _tfLightParent.DOMoveY(2f, durationMoveLightParent);
            RandomLightSequence();
            _ui.LightPanelUI.Init(_curLightSequence.Count, _roundWin, _maxRound);
        }

        public void RandomCurrentRound()
        {
            RandomLightSequence();
            _ui.LightPanelUI.Init(_curLightSequence.Count, _roundWin, _maxRound);
        }

        public void GoNextRound()
        {
            _roundWin++;
            _maxLights = _content.MaxLights[_roundWin];
            RandomCurrentRound();
        }

        public IEnumerator CRPlayLightSequence()
        {
            var delay = new WaitForSeconds(0.2f);

            foreach (var type in _curLightSequence)
            {
                var pole = _lights[type];
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_M4_SHOOT);
                yield return pole.CRTurnOn(0.5f);
                yield return delay;
            }
        }

        public bool IsLastRound() => _roundWin + 1 == _maxRound;

        private void RandomLightSequence()
        {
            var keys = new List<ELightType>(_lights.Keys);
            _curLightSequence.Clear();
            foreach (var type in keys)
            {
                _curLightSequence.Add(type);
            }
            while (_curLightSequence.Count < _maxLights)
            {
                var rndLight = keys.RandomItem();
                _curLightSequence.Add(rndLight);
            }
            _curLightSequence.Shuffle();
        }
    }

    public enum ELightType
    {
        Red,
        Yellow,
        Green,
    }
}
