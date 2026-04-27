using Game10;
using NFramework;
using SquidGame.LandScape.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.BalanceBridge
{
    public class MinigameBalanceBridgeUI : BaseUIView
    {
        [SerializeField] private Button _jumbBTN;
        [SerializeField] private GameObject _notiTimerObject, _inputObject;
        [SerializeField] private TMP_Text _countText;

        [SerializeField] private UITouchPanel _uiTouchPanel;
        [SerializeField] private VariableJoystick _joystick;



        [Header("PLAYING")]
        [SerializeField] private GameObject _playingPNL;
        [SerializeField] private Transform _tfPointBar;
        [SerializeField] private Vector2 _rangePoint;
        [SerializeField] private UITapButton _tapLeftBTN;
        [SerializeField] private UITapButton _tapRightBTN;

        private void OnEnable()
        {
            _joystick.OnPointerUp(null);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            SetDefault();
            BalanceBridgeManager.OnShowTapLeft += Game10Control_OnShowTapLeft;
            BalanceBridgeManager.OnShowTapRight += Game10Control_OnShowTapRight;
            BalanceBridgeManager.OnTapLeftHighlight += Game10Control_OnTapLeftHighlight;
            BalanceBridgeManager.OnTapRightHighlight += Game10Control_OnTapRightHighlight;
            BalanceBridgeManager.OnProgressChanged += Game10Control_OnProgressChanged;
        }

        public override void OnClose()
        {
            base.OnClose();
            BalanceBridgeManager.OnShowTapLeft -= Game10Control_OnShowTapLeft;
            BalanceBridgeManager.OnShowTapRight -= Game10Control_OnShowTapRight;
            BalanceBridgeManager.OnTapLeftHighlight -= Game10Control_OnTapLeftHighlight;
            BalanceBridgeManager.OnTapRightHighlight -= Game10Control_OnTapRightHighlight;
            BalanceBridgeManager.OnProgressChanged -= Game10Control_OnProgressChanged;
        }

        void SetDefault()
        {
            _playingPNL.SetActive(false);
            _inputObject.SetActive(true);
            SetData();
            UpdateValueBar(0.5f);
        }

        private void Game10Control_OnShowTapLeft() => _tapLeftBTN.ShowTap();

        private void Game10Control_OnShowTapRight() => _tapRightBTN.ShowTap();

        private void Game10Control_OnTapLeftHighlight(bool value)
        {
            if (value)
                _tapLeftBTN.HightlightTap();
            else
                _tapLeftBTN.UnHightlightTap();
        }

        private void Game10Control_OnTapRightHighlight(bool value)
        {
            if (value)
                _tapRightBTN.HightlightTap();
            else
                _tapRightBTN.UnHightlightTap();
        }

        private void Game10Control_OnProgressChanged(float value) => UpdateValueBar(value);

        private void SetData()
        {
            _tapLeftBTN.UnHightlightTap();
            _tapRightBTN.UnHightlightTap();
        }

        private void UpdateValueBar(float value)
        {
            var rot = Vector3.zero;
            rot.z = Mathf.Lerp(_rangePoint.x, _rangePoint.y, value);
            _tfPointBar.eulerAngles = rot;
        }

        public void Init(PlayerMovement playerMovement, CinemachineFreeLookInput cinemachineFreeLookInput)
        {
            _jumbBTN.onClick.AddListener(playerMovement.Jump);
            playerMovement.Init(_joystick);
            cinemachineFreeLookInput.InitTouchPanel(_uiTouchPanel);
        }

        public void UpdateCountText(string value)
        {
            _countText.text = $"{value}";
        }

        public void ActiveNotiTimeCount(bool value)
        {
            _notiTimerObject.SetActive(value);
        }

        public void StartGame()
        {
            ActiveNotiTimeCount(false);
            _inputObject.SetActive(false);
            _playingPNL.SetActive(true);
        }

        public void OnRevive()
        {
            SetData();
            UpdateValueBar(0.5f);
            _playingPNL.SetActive(true);
            _inputObject.SetActive(false);
        }

        public void Close()
        {
            _inputObject.SetActive(false);
            _playingPNL.SetActive(false);
        }
    }
}
