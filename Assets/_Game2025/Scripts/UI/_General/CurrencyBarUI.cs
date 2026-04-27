using DG.Tweening;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.UI
{
    public class CurrencyBarUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _amountTMP;

        private void OnEnable()
        {
            var userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
            UserData.OnCoinChanged += UpdateText;
            UpdateText(userData.Coin);
        }

        private void OnDisable() => UserData.OnCoinChanged -= UpdateText;

        private void UpdateText(int amount) => _amountTMP.SetText($"{amount}");


        /// <summary>
        /// Khong toi uu lam ,nhung dung tam di
        /// </summary>
        /// <param name="coin"></param>
        /// <param name="duration"></param>
        /// <param name="onCompleted"></param>
        public void PlayUpdateCoin(int coin, float duration, UnityAction onCompleted)
        {
            var userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);
            var currentCoin = userData.Coin;
            var newCoin = currentCoin + coin;
            GameSound.I.PlaySFX(Define.SoundPath.SFX_ADD_COIN);
            DOVirtual.Int(currentCoin, newCoin, duration, value => UpdateTextAnimation(value,0.1f)).OnComplete(() => {
                userData.Coin = newCoin;
                _amountTMP.transform.DOKill();
                _amountTMP.transform.localScale = Vector3.one;
                onCompleted?.Invoke();
            });
        }


        private void UpdateTextAnimation(int coin, float duration)
        {
            _amountTMP.transform.DOKill();
            _amountTMP.transform.localScale = Vector3.one;
            _amountTMP.transform.DOScale(Vector3.one * 1.1f, duration);
            _amountTMP.SetText($"{coin}");
        }
    }
}
