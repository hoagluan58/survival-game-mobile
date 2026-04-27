using DG.Tweening;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMarblesVer2
{
    public class Point : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private bool _isCompleted;
        [SerializeField] private bool _isFailed;

        [Header("References")]
        [SerializeField] private GameObject _passedGo;
        [SerializeField] private GameObject _failedGo;

        public bool IsCompleted => _isCompleted;

        public void ClearData()
        {
            _isCompleted = false;
            _isFailed = false;
            _passedGo.SetActive(false);
            _failedGo.SetActive(false);
        }

        public void OnInitialized()
        {
            _isCompleted = false;
            _passedGo.SetActive(false);
            _failedGo.SetActive(false);
        }

        public void Scored(bool value)
        {
            _isFailed = value;
            _isCompleted = true;
            PlayAnimationGo(_passedGo.transform, value);
            PlayAnimationGo(_failedGo.transform, !value);
        }

        private void PlayAnimationGo(Transform tf, bool value)
        {
            if (!value)
            {
                tf.gameObject.SetActive(false);
                return;
            }
            tf.DOKill();
            tf.gameObject.SetActive(true);
            tf.localScale = Vector3.zero;
            tf.DOScale(Vector3.one, 0.35f).SetEase(Ease.OutBack);
        }
    }
}
