using System;
using System.Linq;
using SquidGame.Core;
using SquidGame.LandScape;
using UnityEngine;

namespace Game2
{
    public class GlassPiece : MonoBehaviour
    {
        public event Action<GlassPiece> OnStepped;

        [SerializeField] private EffectBreak _effectBreak;

        [Header("Materials")]
        [SerializeField] private Material _initMat;
        [SerializeField] private Material _trueMat;
        [SerializeField] private Material _chooseMat;
        [SerializeField] private MeshRenderer _meshRenderer;


        private bool _isBroken;
        private int _index;
        private (bool isFilled, Vector3 position)[] _botSlots =
            { (false, new Vector3(-0.6f, 0, -0.3f)), (false, new Vector3(0.6f, 0, -0.3f)) };

        public bool IsTrueMove { get; private set; }

        public int StepIndex => _index;
        public Vector3 PlayerPos => transform.position + Vector3.forward * 0.3f;
        public bool IsBroken => _isBroken;
        public bool IsFull => _botSlots.All(b => b.isFilled);

        public void Init(bool isTrue, int index)
        {
            IsTrueMove = isTrue;
            _index = index;
            if (isTrue)
            {
                _meshRenderer.material = _trueMat;
            }
        }

        public void HideTrueMove()
        {
            _meshRenderer.material = _initMat;
        }

        public void EnableJump(bool isSetActive = false)
        {
            _meshRenderer.material = _chooseMat;
            if (isSetActive && !_isBroken)
            {
                _meshRenderer.gameObject.SetActive(true);
            }
        }

        public void DisableJump()
        {
            _meshRenderer.material = _initMat;
        }

        public void Break(bool isPlaySound, bool isPlayEff)
        {
            if (_isBroken) return;
            _isBroken = true;
            if (isPlaySound)
                GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_GLASS_BREAK);
            if (isPlayEff)
            {
                var effect = Instantiate(_effectBreak, gameObject.transform);
                effect.Show(_meshRenderer.material);
            }
            _meshRenderer.gameObject.SetActive(false);
        }

        public void OnPlayerJumpIn()
        {
            OnStepped?.Invoke(this);
        }

        public void OnBotJumpIn(Vector3 position)
        {
            OnStepped?.Invoke(this);

            for (var i = 0; i < _botSlots.Length; i++)
            {
                if (_botSlots[i].position + _meshRenderer.transform.position != position) continue;
                _botSlots[i].isFilled = true;
                break;
            }
        }

        public void OnBotJumpOut(Vector3 lastPosition)
        {
            for (var i = 0; i < _botSlots.Length; i++)
            {
                if (_botSlots[i].position + _meshRenderer.transform.position != lastPosition) continue;
                _botSlots[i].isFilled = false;
                break;
            }
        }

        public Vector3 GetValidBotPosition()
        {
            return _meshRenderer.transform.position + _botSlots.FirstOrDefault(slot => !slot.isFilled).position;
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (IsTrueMove)
                Gizmos.DrawCube(_meshRenderer.transform.position, Vector3.one * 2.5f);
        }
#endif
    }
}