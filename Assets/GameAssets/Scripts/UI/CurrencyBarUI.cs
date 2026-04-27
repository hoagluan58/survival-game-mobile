using DG.Tweening;
using SquidGame.SaveData;
using TMPro;
using UnityEngine;

public class CurrencyBarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tmp;
    [SerializeField] private Transform _coinObject;

    private int _oldCoin;

    private void OnEnable()
    {
        UserData.OnCoinChanged += OnUpdateCoin;
        UpdateText(false);
    }

    private void OnDisable()
    {
        UserData.OnCoinChanged -= OnUpdateCoin;
    }

    private void OnDestroy()
    {
        DOTween.Kill(gameObject);
    }

    private void OnUpdateCoin() => UpdateText(true);

    private void UpdateText(bool isAnim)
    {
        if (isAnim)
        {
            Tweener t1 = _coinObject.DOScale(Vector3.one * 1.15f, 0.15f);
            t1.SetLoops(2, LoopType.Yoyo);
            t1.SetLink(gameObject);

            int startVal = _oldCoin;
            Tweener t = DOTween.To(() => startVal, x => startVal = x, (int)UserData.I.Coin, 1);
            t.OnUpdate(() =>
            {
                _tmp.text = startVal.ToString();
            });
            t.SetLink(gameObject);
        }
        else _tmp.text = UserData.I.Coin.ToString();
        _oldCoin = UserData.I.Coin;
    }
}
