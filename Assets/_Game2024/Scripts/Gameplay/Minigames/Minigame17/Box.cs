using DG.Tweening;
using Redcode.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SquidGame.Minigame17
{
    public class Box : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<EBoxShape, GameObject> _shapes;
        [SerializeField] private Transform _coverTf;

        private EBoxShape _shape;
        private bool _isOpen = false;
        private bool _isFullyOpen = false;

        public bool IsFullyOpen => _isFullyOpen;

        public EBoxShape Shape => _shape;

        public void Init(EBoxShape shape)
        {
            _shape = shape;
            _shapes.Values.ForEach(x => x.gameObject.SetActive(false));
            _shapes[shape].SetActive(true);
        }

        [Button]
        public void Open(bool isPlayAnim = true, float animDuration = 0.5f)
        {
            var endValue = new Vector3(0f, 0f, 90f);

            _isOpen = true;
            _coverTf?.DOKill();

            if (isPlayAnim)
            {
                _coverTf.SetLocalEulerAnglesZ(0f);
                _coverTf.DOLocalRotate(endValue, animDuration).SetEase(Ease.Linear);
            }
            else
            {
                _coverTf.eulerAngles = endValue;
            }
        }

        [Button]
        public void Close(bool isPlayAnim = true, float animDuration = 0.5f)
        {
            var endValue = Vector3.zero;

            _isOpen = false;
            _coverTf?.DOKill();

            if (isPlayAnim)
            {
                _coverTf.SetLocalEulerAnglesZ(90f);
                _coverTf.DOLocalRotate(endValue, animDuration).SetEase(Ease.Linear);
            }
            else
            {
                _coverTf.eulerAngles = endValue;
            }
        }

        public void FullyOpen()
        {
            _isFullyOpen = true;
            _coverTf.gameObject.SetActive(false);
        }
    }
}
