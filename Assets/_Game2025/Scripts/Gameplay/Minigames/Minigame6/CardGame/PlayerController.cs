using NFramework;
using Redcode.Extensions;
using SquidGame.LandScape.Core;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6.CardGame
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;

        private bool _isActive;
        private RaycastHit[] _hits = new RaycastHit[1];
        private Camera _camera;
        private CardManager _cardManager;
        private Card _curCard;

        private void Awake() => _camera = Camera.main;

        private void Update()
        {
            if (!_isActive) return;

            if(UIManager.I.IsPointerOverUIObject()) return;

            if (Input.GetMouseButtonUp(0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                var hits = Physics.RaycastNonAlloc(ray, _hits, Mathf.Infinity, _layerMask);
                if (hits > 0)
                {
                    HandleRaycastHit(_hits[0]);
                }
            }
        }

        public void OnEnter(CardManager cardManager) => _cardManager = cardManager;

        public void OnExit() => SetActive(false);

        public void SetActive(bool value) => _isActive = value;

        private void HandleRaycastHit(RaycastHit hit)
        {
            if (hit.transform.TryGetComponentInParent<Card>(out var card))
            {
                if (!card.CanRaycast) return;

                card.OnRaycast();
                if (_curCard == null)
                {
                    _curCard = card;
                    return;
                }
                else
                {
                    CheckValidCard();
                }
            }

            void CheckValidCard()
            {
                var isValid = _curCard.Data.Type == card.Data.Type;
                if (isValid)
                {
                    GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_CORRECT_CARD);
                    _curCard.OnValid();
                    card.OnValid();
                    _cardManager.CheckWin();
                }
                else
                {
                    VibrationManager.I.Haptic(VibrationManager.EHapticType.MediumImpact);
                    _curCard.OnInvalid().Forget();
                    card.OnInvalid().Forget();
                }
                _curCard = null;
            }
        }
    }
}
