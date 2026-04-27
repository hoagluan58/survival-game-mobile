using UnityEngine;

namespace SquidGame.LandScape.Minigame6.ThrowStoneGame
{
    public class PlayerController : MonoBehaviour
    {
        [Header("REF")]
        [SerializeField] private ThrowStoneConfigSO _configSO;
        [SerializeField] private Stone _stone;


        private ThrowStoneUI _throwStoneUI;
        private ThrowStoneGameController _controller;

        public void OnEnter(ThrowStoneGameController controller, ThrowStoneUI throwStoneUI)
        {
            _controller = controller;
            _throwStoneUI = throwStoneUI;
            _throwStoneUI.ForceUI.OnEnter(_stone, _configSO);
            _stone.OnEnter(_controller, _throwStoneUI.ForceUI, _configSO);
        }

        public void StartGame()
        {
            _stone.SpawnStone();
            _throwStoneUI.ForceUI.StartDirection();
        }
    }
}
