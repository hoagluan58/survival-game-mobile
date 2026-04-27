using DG.Tweening;
using EnhancedUI.EnhancedScroller;
using SquidGame.LandScape.Config;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Data;
using SquidGame.LandScape.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.UI
{
    public class ShopItemUI : EnhancedScrollerCellView
    {
        [Header("LOCKED")]
        [SerializeField] private GameObject _lockGO;
        [SerializeField] private GameObject _buttonsGO;
        [SerializeField] private Button _interactBTN;
        [SerializeField] private Button _adsBTN;
        [SerializeField] private Button _buyBTN;

        [Header("SELECTED")]
        [SerializeField] private GameObject _selectedGO;
        [SerializeField] private Light _light;

        [Header("BOUGHT")]
        [SerializeField] private GameObject _boughtGO;
        [SerializeField] private Button _selectBTN;

        [Header("3D VIEW")]
        [SerializeField] private List<RawImage> _rawImages;
        [SerializeField] private Camera _renderCamera;
        [SerializeField] private BaseCharacter _baseCharacter;
        // [SerializeField] private float _defaultLightIntensity;
        // [SerializeField] private float _lockLightIntensity;


        [Header("REF")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _selectTransform;
        [SerializeField] private Image _selectedBoughtImage;

        public RectTransform RectTransform => _rectTransform;

        private HairConfig _config;
        private UserData _userData;
        private CharacterHair _hair;
        private CharacterBodySkin _skin;
        private CharacterAnimator _animator;
        private Action _onSelectHair;
        private Action _onNotEnoughMoney;
        private DOTweenAnimation _focusAdsAnimation;
        private DG.Tweening.Tween _focusTween, _fadeTween;
        private Coroutine _focusAdsCoroutine;

        private void Awake()
        {
            _hair = _baseCharacter.GetCom<CharacterHair>();
            _skin = _baseCharacter.GetCom<CharacterBodySkin>();
            _animator = _baseCharacter.GetCom<CharacterAnimator>();
            _focusAdsAnimation = _adsBTN.GetComponent<DOTweenAnimation>();

            _rectTransform.pivot = new Vector2(0.5f, 0);
        }

        private void OnEnable()
        {
            _buyBTN.onClick.AddListener(OnBuyButtonClicked);
            _adsBTN.onClick.AddListener(OnAdsButtonClicked);
            _selectBTN.onClick.AddListener(OnSelectButtonClicked);
            _interactBTN.onClick.AddListener(OnClicked);
            _focusAdsCoroutine = StartCoroutine(RandomAdsButtonEffect());
        }

        private void OnDisable()
        {
            _buyBTN.onClick.RemoveListener(OnBuyButtonClicked);
            _adsBTN.onClick.RemoveListener(OnAdsButtonClicked);
            _selectBTN.onClick.RemoveListener(OnSelectButtonClicked);
            _interactBTN.onClick.RemoveListener(OnClicked);
            if (_focusAdsCoroutine != null)
                StopCoroutine(_focusAdsCoroutine);
        }

        private void OnBuyButtonClicked()
        {

            var isEnoughtCoin = _userData.Coin >= Define.HAIR_PRICE;
            if (!isEnoughtCoin)
            {
                GameSound.I.PlaySFXButtonClick();
                _onNotEnoughMoney?.Invoke();
                return;
            }

            _userData.Coin -= Define.HAIR_PRICE;
            _userData.UnlockHair(_config.Id);
            _userData.ChangeHair(_config.Id);
            _onSelectHair?.Invoke();
            _animator.PlayAnimation(EAnimStyle.Victory_2);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_BUY_ITEM);
        }

        private void OnAdsButtonClicked()
        {
            // Reward video removed — unlock is free locally
            _userData.UnlockHair(_config.Id);
            _userData.ChangeHair(_config.Id);
            _onSelectHair?.Invoke();
            _animator.PlayAnimation(EAnimStyle.Victory_2);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_EQUIP_ITEM);
        }

        private void OnSelectButtonClicked()
        {
            // GameSound.I.PlaySFXButtonClick();
            _userData.ChangeHair(_config.Id);
            _onSelectHair?.Invoke();
            _animator.PlayAnimation(EAnimStyle.Victory_2);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_EQUIP_ITEM);
        }

        private void OnClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            _focusTween = _selectTransform.DOScale(transform.localScale * 1.03f, 0.08f).SetLoops(2, LoopType.Yoyo);

            if (!_userData.IsHairUnlocked(_config.Id))
            {
                _fadeTween = _selectedBoughtImage.DOFade(0.5f, 0.25f).OnComplete(() => _selectedBoughtImage.DOFade(0f, 0.25f));
            }
        }

        public void SetData(HairConfig config, Action onSelectHair = null, Action onNotEnoughMoney = null)
        {
            _config = config;
            _onSelectHair = onSelectHair;
            _onNotEnoughMoney = onNotEnoughMoney;

            _hair.ChangeHair(_config.Id);
            _skin.ChangeSkin(CharacterBodySkin.ESkinName.Green);

            _renderCamera.targetTexture = _config.ShopRenderTexture;
            foreach (var rawImage in _rawImages)
            {
                rawImage.texture = _config.ShopRenderTexture;
            }

            Refresh();
        }

        public override void RefreshCellView()
        {
            base.RefreshCellView();
            Refresh();
        }

        public void Refresh()
        {
            _animator.PlayAnimation(EAnimStyle.Idle);
            _userData = GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA);

            var isHairUnlocked = _userData.IsHairUnlocked(_config.Id);
            var isCurrentHair = _userData.IsCurrentHair(_config.Id);

            _lockGO.SetActive(!isHairUnlocked);
            _buttonsGO.SetActive(_lockGO.activeSelf);
            _boughtGO.SetActive(isHairUnlocked && !isCurrentHair);
            _selectedGO.SetActive(isCurrentHair);
            _light.intensity = !isHairUnlocked ? 0.5f : 2f;

            Vector3 scale = Vector3.one * (isCurrentHair ? 1.1f : 1f);
            _focusTween = transform.DOScale(scale, 0.25f).SetEase(Ease.OutBack);
        }

        IEnumerator RandomAdsButtonEffect()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(6, 15));
            while (true)
            {
                _focusAdsAnimation.DORestart();
                yield return new WaitForSeconds(UnityEngine.Random.Range(6, 15));
            }
        }
    }
}
