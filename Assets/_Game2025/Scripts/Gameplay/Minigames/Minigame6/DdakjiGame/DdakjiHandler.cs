using DG.Tweening;
using NFramework;
using SquidGame.LandScape.Core;
using System;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6.Ddakji
{
    public class DdakjiHandler : MonoBehaviour
    {
        [SerializeField] private Ddakji _redDdakji;
        [SerializeField] private Ddakji _blueDdakji;
        [SerializeField] private SpriteRenderer _targetSR;
        [SerializeField] private DdakjiGameConfigSO _configSO;

        private DdakjiGameController _controller;
        private DdakjiGameUI _ui;

        public void OnEnter(DdakjiGameController controller, DdakjiGameUI ui)
        {
            _controller = controller;
            _ui = ui;
        }

        public void Disable()
        {
            _targetSR.gameObject.SetActive(false);
            _ui.gameObject.SetActive(true);
        }

        public void StartMovingTarget()
        {
            _controller.SetGameState(DdakjiGameController.EGameState.Playing);
            _ui.gameObject.SetActive(true);
            _ui.StartMovingTarget();
            _targetSR.gameObject.SetActive(true);
            _targetSR.transform.localPosition = new Vector3(_configSO.MinTargetPos, 0f, 0f);
            var duration = 1f;
            _targetSR.transform.DOKill();
            _targetSR.transform.DOLocalMoveX(_configSO.MaxTargetPos, duration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo)
                .OnUpdate(() =>
                {
                    var isValid = _redDdakji.IsWithinCollider(_targetSR.transform.position);
                    _targetSR.color = isValid ? Color.green : Color.red;
                    _ui.UpdateTargetButton(isValid);
                });
        }

        public void EndMovingTarget()
        {
            _targetSR.transform.DOKill();
            _targetSR.gameObject.SetActive(false);
        }

        public void Throw(bool isFlip)
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_PAPER_PUNCH);
            SpawnBlueDdakji();
            _redDdakji.SetFlipOnCollision(isFlip);
            VibrationManager.I.Haptic(VibrationManager.EHapticType.MediumImpact);
            if (isFlip) _controller.Win().Forget();
            else
            {
                _blueDdakji.Flip(false);
                this.InvokeDelay(2f, () =>
                {
                    if (_controller.CurState != DdakjiGameController.EGameState.Playing) return;

                    _blueDdakji.gameObject.SetActive(false);
                    StartMovingTarget();
                });
            }
        }

        private void SpawnBlueDdakji()
        {
            var position = _redDdakji.transform.position + new Vector3(0, 0.3f, 0);
            _blueDdakji.transform.position = position;
            _blueDdakji.gameObject.SetActive(true);
        }
    }
}
