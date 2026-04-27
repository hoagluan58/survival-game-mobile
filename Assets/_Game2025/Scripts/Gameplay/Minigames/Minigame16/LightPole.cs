using System.Collections;
using UnityEngine;

namespace SquidGame.LandScape.Minigame16 { 
    public class LightPole : MonoBehaviour
    {
        [SerializeField] private ELightType _lightType;
        [SerializeField] private MeshRenderer _lightMeshRenderer;
        [SerializeField] private GameObject _pointLight;

        public ELightType LightType => _lightType;
        private bool _isTurnOn = false;

        public void Init() => TurnOff();

        public IEnumerator CRTurnOn(float time)
        {
            _isTurnOn = true;
            _lightMeshRenderer.material.EnableKeyword("_EMISSION");
            _pointLight.SetActive(true);
            yield return new WaitForSeconds(time);
            TurnOff();
        }

        public void TurnOff()
        {
            _isTurnOn = false;
            _lightMeshRenderer.material.DisableKeyword("_EMISSION");
            _pointLight.SetActive(false);
        }
    }
}
