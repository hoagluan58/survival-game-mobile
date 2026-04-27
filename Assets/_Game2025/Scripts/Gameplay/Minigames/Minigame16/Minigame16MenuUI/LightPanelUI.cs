using RotaryHeart.Lib.SerializableDictionary;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace SquidGame.LandScape.Minigame16
{
    public class LightPanelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _sequenceTMP;
        [SerializeField] private LightItemUI _lightItemPf;
        [SerializeField] private Transform _lightItemParent;
        [SerializeField] private SerializableDictionaryBase<ELightType, Sprite> _lightColorDic;

        private int _curLightIndex = 0;
        private List<LightItemUI> _lightItems = new List<LightItemUI>();
        private ObjectPool<LightItemUI> _poolLightItems;

        private void Awake()
        {
            _poolLightItems = new(
               () => Instantiate(_lightItemPf, _lightItemParent),
               item => item.gameObject.SetActive(true),
               item => item.gameObject.SetActive(false),
               item => Destroy(item.gameObject));
        }

        public void Init(int lightCount, int curRound, int maxRound)
        {
            ClearPool();
            for (var i = 0; i < lightCount; i++)
            {
                var lightItem = _poolLightItems.Get();
                lightItem.Init();
                lightItem.transform.SetAsLastSibling();
                _lightItems.Add(lightItem);
            }
            _curLightIndex = 0;
            UpdateRoundCounter(curRound, maxRound);
        }

        public void SetLightIndex(ELightType type)
        {
            var color = _lightColorDic[type];
            _lightItems[_curLightIndex].SetLight(color);
            _curLightIndex++;
        }

        private void ClearPool()
        {
            _lightItems.ForEach(x => _poolLightItems.Release(x));
            _lightItems.Clear();
        }

        private void UpdateRoundCounter(int roundWin, int maxRound)
        {
            _sequenceTMP.text = $"{roundWin}/{maxRound}";
        }
    }
}
