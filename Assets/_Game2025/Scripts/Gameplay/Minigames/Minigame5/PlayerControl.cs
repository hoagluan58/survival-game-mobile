using NFramework;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using UnityEngine;

namespace SquidGame.LandScape.Minigame5
{
    public class PlayerControl : MonoBehaviour
    {
        [Header("CONFIG")]
        [SerializeField] private float _speedUpPower = 5f;
        [SerializeField] private float _speedDownPower = 0.5f;
        [SerializeField] private float _speedMoveDrag;

        [Header("LEVEL")]
        [SerializeField] Vector2[] _levelPowerRange;
        [SerializeField] private Transform _ropeModel;

        private float _currentPower;
        private bool _isActive;
        private Vector2 _currentRange;
        private Game5Controller _controller;

        public int CurrentLevel
        {
            get => PlayerPrefs.GetInt("MINIGAME5", 0);
            set => PlayerPrefs.SetInt("MINIGAME5", value);
        }

        public void Init(Game5Controller controller, UIPowerBar uiPowerBar)
        {
            _controller = controller;
            _currentRange = GetPowerRequireRange();
            uiPowerBar.Init(CurrentLevel, _currentRange);
        }

        public void Active()
        {
            _isActive = true;
            _currentPower = 0f;
        }

        private void Update()
        {
            if (!_isActive) return;

            if (Input.GetMouseButtonDown(0))
            {
                _currentPower += Time.deltaTime * _speedUpPower;
                VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
            }
            else
            {
                _currentPower -= Time.deltaTime * _speedDownPower;
            }

            _currentPower = Mathf.Clamp01(_currentPower);
            _controller.InvokeOnPowerBarChanged(_currentPower);
            if (_currentPower >= _currentRange.x && _currentPower < _currentRange.y)
            {
                _controller.InvokeOnWarning(false);
                _ropeModel.transform.position -= _speedMoveDrag * Time.deltaTime * Vector3.forward;
            }
            else
            {
                _controller.InvokeOnWarning(false);
                _ropeModel.transform.position += _speedMoveDrag * Time.deltaTime * Vector3.forward;
            }

            if (_ropeModel.transform.position.z >= 4.5)
            {
                Lose();
            }
            else if (_ropeModel.transform.position.z <= -4.5)
            {
                Win();
            }
        }

        public void EndGame()
        {
            Lose();
        }

        [Button]
        public void Win()
        {
            CurrentLevel += 1;
            _isActive = false;
            _controller.ShowAnimationWin();
        }

        [Button]
        public void Lose()
        {
            _isActive = false;
            _controller.ShowAnimationLose();
        }

        Vector2 GetPowerRequireRange()
        {
            return _levelPowerRange[CurrentLevel >= _levelPowerRange.Length ? ^1 : CurrentLevel];
        }
    }
}
