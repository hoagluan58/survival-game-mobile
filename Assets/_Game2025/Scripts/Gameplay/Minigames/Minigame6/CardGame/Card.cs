using Cysharp.Threading.Tasks;
using DG.Tweening;
using Redcode.Extensions;
using Sirenix.Serialization;
using SquidGame.LandScape.Core;
using System;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6.CardGame
{
    public class Card : MonoBehaviour
    {
        [ReadOnly][SerializeField] private CardData _data;

        private bool _isTweening = false;
        public CardData Data => _data;

        public void Init(CardData data) => _data = data;

        public void Flip(bool isflipUp)
        {
            _isTweening = true;
            transform.DOKill();
            transform.DOLocalRotate(new Vector3(0f, 0f, isflipUp ? 180f : 0f), 0.5f).OnComplete(() =>
            {
                _isTweening = false;
            });
            _data.IsFlipUp = isflipUp;
        }

        public void OnRaycast()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG06_FLIP_CARD);
            Flip(true);
        }

        public void OnValid() => _data.IsCompleted = true;

        public async UniTaskVoid OnInvalid()
        {
            await UniTask.WaitUntil(() => !_isTweening);
            Flip(false);
        }

        public bool CanRaycast => !_data.IsCompleted && !_data.IsFlipUp;
    }

    [Serializable]
    public class CardData
    {
        public ECardType Type;
        public bool IsCompleted;
        public bool IsFlipUp;

        public CardData(ECardType type)
        {
            Type = type;
            IsCompleted = false;
            IsFlipUp = true;
        }
    }
}
