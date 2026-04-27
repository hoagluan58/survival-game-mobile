using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using UnityEngine;

namespace SquidGame.Minigame03
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeReference] private LayerMask _layerSelect;
        [SerializeField] private GameObject _fxBlood;

        [Header("Animation")]
        [SerializeField] private CharacterAnimationController _animator;

        private MinigameController _controller;
        private bool _isActive;
        private bool _isSelectDone;

        private Camera _camera;

        private void Awake() => _camera = Camera.main;

        public void Init(MinigameController controller)
        {
            _controller = controller;
            _animator.PlayAnimation(EAnimStyle.Idle);
            _fxBlood.SetActive(false);
        }

        public void Active()
        {
            _controller.InvokeShowTutorial(true);
            gameObject.SetActive(true);
            _isActive = true;
        }

        private void Update()
        {
            if (!_isActive) return;

            if (_isSelectDone == false && Input.GetMouseButtonDown(0))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, _layerSelect))
                {
                    DalgonaCase dalgonaCase = hit.transform.GetComponent<DalgonaCase>();
                    if (dalgonaCase)
                    {
                        _controller.InvokeShowTutorial(false);
                        GameSound.I.PlaySFX(Define.SoundPath.SFX_MG03_CHOOSE_CASE);
                        dalgonaCase.Active(_controller);
                        _isSelectDone = true;
                    }
                }
            }
        }

        public void ShowWin()
        {
            _animator.PlayAnimation(EAnimStyle.Victory_1, EAnimStyle.Victory_2, EAnimStyle.Victory_3);
        }

        public void Die()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
            _fxBlood.SetActive(true);
            _animator.PlayAnimation(EAnimStyle.Die);
        }
    }
}
