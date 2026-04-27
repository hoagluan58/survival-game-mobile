using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.Minigame6.ThrowStoneGame
{
    public class ThrowStoneUI : MonoBehaviour
    {
        [SerializeField] private ForceControlUI _forceUI;
        [SerializeField] private Button _tapButton;

        public ForceControlUI ForceUI => _forceUI;

        private void OnEnable() => _tapButton.onClick.AddListener(OnTapButtonClicked);

        private void OnDisable() => _tapButton.onClick.RemoveListener(OnTapButtonClicked);

        private void OnTapButtonClicked()
        {
            switch (_forceUI.State)
            {
                case ForceControlUI.EState.None:
                    break;
                case ForceControlUI.EState.Direction:
                    _forceUI.LockDirection();
                    break;
                case ForceControlUI.EState.Force:
                    _forceUI.LockForce();
                    break;
                default:
                    break;
            }
        }

    }
}
