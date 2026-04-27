using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame16
{
    public class LightItemUI : MonoBehaviour
    {
        [SerializeField] private Image _lightIMG;
        [SerializeField] private Sprite _defaultLight;

        public void Init() => _lightIMG.sprite = _defaultLight;

        public void SetLight(Sprite sprite)
        {
            _lightIMG.sprite = sprite;
        }
    }
}