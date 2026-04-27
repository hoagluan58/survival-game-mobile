using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape.UI
{
    public class ChallengeSeasonItemUI : MonoBehaviour
    {
        [Header("UNLOCK")]
        [SerializeField] private GameObject _unlockGroup;
        [SerializeField] private TextMeshProUGUI _titleTMP;
        [SerializeField] private TextMeshProUGUI _shadowTitleTMP;
        [SerializeField] private Image _thumbnailIMG;
        [SerializeField] private Button _startBTN;

        [Header("LOCK")]
        [SerializeField] private GameObject _lockGroup;
        [SerializeField] private TextMeshProUGUI _lockShadowTMP;
        [SerializeField] private TextMeshProUGUI _lockTitleTMP;
        [SerializeField] private Image _lockThumbnailIMG;
        [SerializeField] private Button _lockBTN;

        [Header("COMING SOON")]
        [SerializeField] private GameObject _comingSoonGroup;
        [SerializeField] private Button _comingSoonBTN;

        private Action _onStart, _onLock, _onComingSoon;

        private void OnEnable()
        {
            _startBTN.onClick.AddListener(OnStartButtonClicked);
            _lockBTN.onClick.AddListener(OnLockButtonClicked);
            _comingSoonBTN.onClick.AddListener(OnComingSoonButtonClicked);
        }

        private void OnDisable()
        {
            _startBTN.onClick.RemoveListener(OnStartButtonClicked);
            _lockBTN.onClick.RemoveListener(OnLockButtonClicked);
            _comingSoonBTN.onClick.RemoveListener(OnComingSoonButtonClicked);
        }

        private void OnStartButtonClicked() => _onStart?.Invoke();

        private void OnLockButtonClicked() => _onLock?.Invoke();

        private void OnComingSoonButtonClicked() => _onComingSoon?.Invoke();

        public void SetData(int seasonId, Sprite thumbnail, bool isUnlock, Action onStart = null, Action onLock = null)
        {
            _unlockGroup.SetActive(isUnlock);
            _lockGroup.SetActive(!isUnlock);
            _comingSoonGroup.SetActive(false);
            SetTitle($"Season {seasonId}");
            SetThumbnail(thumbnail);
            _onStart = onStart;
            _onLock = onLock;
        }

        public void SetComingSoon(Action onComingSoon = null)
        {
            _unlockGroup.SetActive(false);
            _lockGroup.SetActive(false);
            _comingSoonGroup.SetActive(true);
            _onComingSoon = onComingSoon;
        }

        private void SetTitle(string title)
        {
            _titleTMP.SetText(title);
            _shadowTitleTMP.SetText(title);
            _lockShadowTMP.SetText(title);
            _lockTitleTMP.SetText(title);
        }

        private void SetThumbnail(Sprite sprite)
        {
            _thumbnailIMG.sprite = sprite;
            _lockThumbnailIMG.sprite = sprite;
        }
    }
}
