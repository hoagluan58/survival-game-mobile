using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SquidGame.Core;
using UnityEngine.UI;
using SquidGame.UI;
using SquidGame.LandScape;

namespace Game8
{
    public class UIButtonShape : MonoBehaviour
    {
        [SerializeField] private Image _img;
        [SerializeField] private Button _button;
        [SerializeField] private int _id;

        private Minigame08MenuUI _menu;
        private Game8Control _controller;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        public void Init(Game8Control controller, Minigame08MenuUI menuUI)
        {
            _controller = controller;
            _menu = menuUI;
        }

        public void OnClick()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_BUTTON_CLICK);
            TypeShape typeShape = (TypeShape)_id;

            if (_controller.PlayerControl.JumpToShape(typeShape))
            {
                _menu.ToggleCountdownPanel(true);

                Tweener t = transform.DOScale(Vector3.one * 1.25f, 0.125f);
                t.SetLoops(2, LoopType.Yoyo);
                t.OnComplete(() =>
                {
                    _img.color = Color.white;
                });
                _img.color = Color.green;
            }
        }
    }
}
