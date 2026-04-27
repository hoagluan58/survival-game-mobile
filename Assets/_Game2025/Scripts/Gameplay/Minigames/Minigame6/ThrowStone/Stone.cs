using Cysharp.Threading.Tasks;
using SquidGame.LandScape.Core;
using System;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6.ThrowStoneGame
{
    public class Stone : MonoBehaviour, IThrowHandler
    {
        [SerializeField] private Rigidbody _rigidbody;

        private ForceControlUI _forceUI;
        private ThrowStoneGameController _controller;
        private ThrowStoneConfigSO _config;
        private Vector3 _initPosition;

        private void Awake() => _initPosition = transform.position;

        public void OnEnter(ThrowStoneGameController controller, ForceControlUI forceUI, ThrowStoneConfigSO config)
        {
            _config = config;
            _controller = controller;
            _forceUI = forceUI;
        }

        public void SpawnStone()
        {
            gameObject.SetActive(true);
            _forceUI.StartDirection();
            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector3.zero;
            transform.position = _initPosition;
            transform.eulerAngles = Vector3.zero;
        }

        public void Throw(float normalizeDirection, float normalizeForce)
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_STONE_DROP);
            var force = GetThrowForce(normalizeDirection, normalizeForce);
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(force, ForceMode.Impulse);
            TrySpawnStone().Forget();
        }

        private async UniTaskVoid TrySpawnStone()
        {
            var delayNextBall = 2.5f;
            await UniTask.Delay(TimeSpan.FromSeconds(delayNextBall));
            if (_controller.CurState == ThrowStoneGameController.EGameState.Playing)
            {
                SpawnStone();
            }
        }

        public Vector3 GetThrowForce(float normalizeDirection, float normalizeForce)
        {
            var offsetY = new Vector3(0, 0.2f, 0);
            var angle = Mathf.Lerp(_config.MinAngle, _config.MaxAngle, (normalizeDirection + 1) / 2f);
            var force = Mathf.Lerp(0f, _config.MaxForce, normalizeForce);
            var direction = Quaternion.Euler(0, angle, 0) * (Vector3.forward + offsetY);

            return direction * force;
        }
    }
}
