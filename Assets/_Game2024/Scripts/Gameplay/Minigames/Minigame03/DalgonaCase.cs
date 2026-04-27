using DG.Tweening;
using UnityEngine;

namespace SquidGame.Minigame03
{
    public class DalgonaCase : MonoBehaviour
    {
        [SerializeField] private GameObject _cover;

        private Dalgona _dalgona;
        private MinigameController _controller;

        public void Active(MinigameController controller)
        {
            _controller = controller;
            _dalgona = _controller.GetDalgona();
            _dalgona.gameObject.SetActive(true);
            _dalgona.transform.position = transform.position;
            _dalgona.Spline.RebuildImmediate();

            _cover.transform.DOMoveX(Mathf.Sign(transform.position.x) * 10f, 1f)
                .OnComplete(() =>
            {
                controller.NeedleController.Active(_dalgona);
                _cover.gameObject.SetActive(false);
            });
        }
    }
}