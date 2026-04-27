using DG.Tweening;
using NFramework;
using Redcode.Extensions;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.SaveData;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.UI
{
    public class WinChallengeModePopupUI : BaseUIView
    {
        [SerializeField] private TextMeshProUGUI _dayTMP;
        [SerializeField] private TextMeshProUGUI _totalRewardTMP;
        [SerializeField] private Briefcase3DViewUI _briefcase3DViewUI;

        [Header("PASS DAY")]
        [SerializeField] private GameObject _passDayPNL;
        [SerializeField] private Button _nextDayBTN;

        [Header("PASS CHALLENGE")]
        [SerializeField] private GameObject _passChallengePNL;
        [SerializeField] private TextMeshProUGUI _normalRewardTMP;
        [SerializeField] private TextMeshProUGUI _adsRewardTMP;
        [SerializeField] private Button _claimNormalBTN;
        [SerializeField] private Button _claimAdsBTN;

        [Header("ANIMATION")]
        [SerializeField] private RectTransform _winPanel;
        [SerializeField] private RectTransform _totalRewardRect;
        [SerializeField] private RectTransform _cashPanel;

        private int _totalReward;
        private AudioSource _winSFX;

        private int _day;

        public override void OnOpen()
        {
            base.OnOpen();
            _nextDayBTN.onClick.AddListener(OnNextDayButtonClicked);
            _claimNormalBTN.onClick.AddListener(OnNormalButtonClicked);
            _claimAdsBTN.onClick.AddListener(OnAdsButtonClicked);
            SetData();
        }

        public override void OnClose()
        {
            base.OnClose();
            _nextDayBTN.onClick.RemoveListener(OnNextDayButtonClicked);
            _claimNormalBTN.onClick.RemoveListener(OnNormalButtonClicked);
            _claimAdsBTN.onClick.RemoveListener(OnAdsButtonClicked);
            _winSFX.Stop();
            _claimAdsBTN.transform.DOKill();
        }

        private void SetData()
        {
            _winSFX = GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_SCREEN);
            _day = UserData.I.Day;
            _totalReward = _day * Define.DAY_REWARD;

            _dayTMP.SetText(!UserData.I.IsMaxDay ?
                GameLocalization.I.GetStringFromTable("STRING_DAY", _day) :
                GameLocalization.I.GetStringFromTable("STRING_CHALLENGE"));

            _briefcase3DViewUI.Init(30);
            _totalRewardTMP.SetText($"{_totalReward}");
            _normalRewardTMP.SetText($"{_totalReward}");
            _adsRewardTMP.SetText($"{_totalReward * Define.CLAIM_ADS_MULTIPLY}");

            StartCoroutine(CRPlayAnim());
        }

        private void OnNextDayButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            GameManager.I.GoNextDay();
            this.InvokeDelay(TransitionUI.DELAY_TIME, () => CloseSelf());
        }

        private void OnNormalButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UserData.I.Coin += _totalReward;
            GameManager.I.GoNextDay(true);
            this.InvokeDelay(TransitionUI.DELAY_TIME, () => CloseSelf());
        }

        private void OnAdsButtonClicked()
        {
            GameSound.I.PlaySFXButtonClick();
            UserData.I.Coin += _totalReward * Define.CLAIM_ADS_MULTIPLY;
            GameManager.I.GoNextDay(true);
            this.InvokeDelay(TransitionUI.DELAY_TIME, () => CloseSelf());
        }

        private IEnumerator CRPlayAnim()
        {
            var isMaxChallengeDay = UserData.I.IsMaxDay;
            _passChallengePNL.SetActive(isMaxChallengeDay);
            _passDayPNL.SetActive(!isMaxChallengeDay);

            _totalRewardRect.gameObject.SetActiveChilds(false);
            _claimNormalBTN.gameObject.SetActive(false);
            _claimAdsBTN.gameObject.SetActive(false);
            _nextDayBTN.gameObject.SetActive(false);

            _totalRewardRect.SetLocalScaleX(0f);
            _winPanel.SetLocalScaleX(0f);
            _cashPanel.SetAnchoredPositionY(-663f);

            yield return _winPanel.transform.DOScaleX(1f, 0.5f).WaitForCompletion();
            yield return _totalRewardRect.DOScaleX(1f, 0.5f).OnComplete(() =>
            {
                _totalRewardRect.gameObject.SetActiveChilds(true);
            }).WaitForCompletion();
            yield return _cashPanel.DOAnchorPosY(0, 0.5f).WaitForCompletion();

            if (isMaxChallengeDay)
            {
                _claimNormalBTN.gameObject.DOScaleShow();
                _claimAdsBTN.gameObject.DOScaleShow(() =>
                {
                    _claimAdsBTN.gameObject.DOScaleLoop(Vector3.one, Vector3.one * 1.1f, 0.5f);
                });
            }
            else
            {
                _nextDayBTN.gameObject.DOScaleShow();
            }
        }
    }
}
