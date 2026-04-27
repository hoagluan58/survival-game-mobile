using Redcode.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SquidGame.LandScape.Game;
using SquidGame.LandScape.Core;

namespace SquidGame.LandScape.Minigame16
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private LayerMask _layermask;

        private bool _isUpdate, _isLightOn;
        private LightPole _curlightPole;
        private Camera _camera;
        private List<ELightType> _playerLightSequence;

        private Minigame16MenuUI _ui;
        private MinigameController _minigameController;
        private LightController _lightController;

        public void Init(MinigameController minigameController, LightController lightController, Minigame16MenuUI ui)
        {
            _camera = Camera.main;
            _playerLightSequence = new List<ELightType>();
            _minigameController = minigameController;
            _lightController = lightController;
            _ui = ui;
        }

        public void OnNewRound()
        {
            _playerLightSequence.Clear();
            EnableInput(true);
        }

        private void EnableInput(bool value)
        {
            _isUpdate = value;
        }

        private void Update()
        {
            if (!_isUpdate) return;
            if (_isLightOn) return;

            HandleMouseInput();
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layermask))
                {
                    if (hit.collider.TryGetComponentInParent<LightPole>(out var newLightPole))
                    {
                        _curlightPole?.TurnOff();
                        _curlightPole = newLightPole;
                        StartCoroutine(CRTurnOnLight());
                    }
                }
            }
        }

        private IEnumerator CRTurnOnLight()
        {
            _isLightOn = true;
            var lightIndex = _playerLightSequence.Count;
            var rightLight = _lightController.CurLightSequence[lightIndex];
            var isRightLight = _curlightPole.LightType == rightLight;
            GameSound.I.PlaySFX(isRightLight ? Define.SoundPath.SFX_CORRECT_CHOICE : Define.SoundPath.SFX_WRONG_CHOICE);

            yield return _curlightPole.CRTurnOn(0.5f);

            if (isRightLight)
            {
                _ui.LightPanelUI.SetLightIndex(_curlightPole.LightType);
                _playerLightSequence.Add(_curlightPole.LightType);
                if (_playerLightSequence.Count == _lightController.CurLightSequence.Count)
                {
                    EnableInput(false);
                    yield return new WaitForSeconds(1f);
                    _minigameController.TryWin();
                }
            }
            else
            {
                EnableInput(false);
                _minigameController.LoseGame();


            }
            _isLightOn = false;
        }
    }
}
